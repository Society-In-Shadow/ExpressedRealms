import {makeIdSafe} from "@/utilities/stringUtilities";
import {isProxy, toRaw} from "vue";

/**
 * Smoothly scrolls to a specific section on the page by element ID.
 *
 * @param {string} sectionId - The ID of the target section.
 */
export function scrollToSection(sectionId: string) {
    const target = document.getElementById(makeIdSafe(sectionId)); // Ensure the ID is safe

    if (target) {
        target.scrollIntoView({
            behavior: "smooth", // Smoothly scroll to the section
            block: "start" // Align with the top of the viewport
        });
    }
}

/**
 * Recursively traverses the tree beginning with an array of nodes,
 * adding dynamically assigned "sort" values based on their position
 * in the current array. Handles Vue Proxy objects gracefully.
 *
 * @param {Array} nodes - The array of nodes to process (possibly a Proxy).
 * @returns {Array} - A new array with "id", "sort", and "subSections" for each node.
 */
export function getIdsWithDynamicSortForArray(nodes, parentId) {
    if (!Array.isArray(nodes)) {
        return []; // If not an array, return an empty array to safeguard the process
    }

    // Ensure we are working with raw data if it's a Vue Proxy
    const rawNodes = isProxy(nodes) ? toRaw(nodes) : nodes;

    // Process each node in the array, dynamically assigning "sort"
    return rawNodes.map((node, index) => {
        // Safeguard if node is not an object
        if (!node || typeof node !== "object") {
            return null;
        }

        // Convert node (if it's reactive) to raw, so we can handle subSections
        const rawNode = isProxy(node) ? toRaw(node) : node;

        // Build the processed result with sort and recursively processed subSections
        return {
            id: rawNode.id || null, // Use null for missing IDs
            parentId: parentId,
            sortOrder: index + 1, // Add sort based on array position (1-based index)
            subSections: getIdsWithDynamicSortForArray(rawNode.subSections || [], rawNode.id) // Recursively process subSections
        };
    }).filter(node => node !== null); // Filter out invalid (null) nodes
}
