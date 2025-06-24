import {isProxy, toRaw} from "vue";
import type {PowerPath} from "@/components/expressions/powerPaths/types";
import type {Power} from "@/components/expressions/powers/types";

export function getSortAndIdsForPowerPaths(nodes:PowerPath[] | Power[]) {

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
            id: rawNode.id, // Use null for missing IDs
            sortOrder: index + 1, // Add sort based on array position (1-based index)
        };
    }).filter(node => node !== null); // Filter out invalid (null) nodes
}
