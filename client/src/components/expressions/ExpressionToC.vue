<script setup lang="ts">

import {makeIdSafe} from "@/utilities/stringUtilities";
import Skeleton from 'primevue/skeleton';
import {BaseTree, Draggable} from "@he-tree/vue";
import {toRaw, isProxy, ref, watch} from 'vue';
import '@he-tree/vue/style/default.css'
import '@he-tree/vue/style/material-design.css'
import Button from "primevue/button";
import axios from "axios";
import toaster from "@/services/Toasters";
import {expressionStore} from "@/stores/expressionStore";
const expressionInfo = expressionStore();

const model = defineModel({ required: true, default: {}, type: Array });

const emit = defineEmits<{
  togglePreview: []
}>();

let originalModel;

const props = defineProps({
  canEdit:{
    type: Boolean
  },
  showSkeleton:{
    type: Boolean,
    required: true
  }
});

function saveChanges(){

  axios.put(`/expression/${expressionInfo.currentExpressionId}/updateHierarchy`, {
    expressionId: expressionInfo.currentExpressionId,
    items: getIdsWithDynamicSortForArray(model.value, null)
  }).then(() => {
    emit("togglePreview");
    showTocEdit.value = !showTocEdit.value;
    toaster.success("Successfully Updated Expression Tree!");
  });
}

const showTocEdit = ref(false);
function toggleEdit(){
  if(!showTocEdit.value)
    originalModel = JSON.parse(JSON.stringify(toRaw(model.value)));
  
  if(showTocEdit.value)
    model.value = originalModel;
  
  emit("togglePreview");
  showTocEdit.value = !showTocEdit.value;
}

/**
 * Recursively traverses the tree beginning with an array of nodes,
 * adding dynamically assigned "sort" values based on their position
 * in the current array. Handles Vue Proxy objects gracefully.
 *
 * @param {Array} nodes - The array of nodes to process (possibly a Proxy).
 * @returns {Array} - A new array with "id", "sort", and "subSections" for each node.
 */
function getIdsWithDynamicSortForArray(nodes, parentId) {
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



</script>

<template>
  <Draggable
      v-if="props.canEdit && showTocEdit"
      class="mtl-tree"
      v-model="model"
      childrenKey="subSections"
      updateBehavior="new"
      textKey="name">
    <template #default="{ node, stat }">
      <a class="p-1 tocItem" >{{ node.name }}</a>
    </template>
  </Draggable>
  <BaseTree v-else 
            v-model="model" 
            childrenKey="subSections"
            textKey="name">
    <template #default="{ node, stat }">
      <Skeleton v-if="props.showSkeleton" id="toc-skeleton" class="mb-2" height="1.5em" />
      <a v-else class="p-1 tocItem" :href="'#' + makeIdSafe(node.name)">{{ node.name }}</a>
    </template>
  </BaseTree>
  <div v-if="props.canEdit" >
    <Button v-if="showTocEdit" label="Save" class="mt-2 w-100" @click="saveChanges" />

    <Button :label="showTocEdit ? 'Cancel' : 'Edit Order'" class="mt-2 w-100" @click="toggleEdit" />
  </div>

</template>

<style scoped>

.tocItem{
  text-decoration: none;
  display: block;
  color: inherit;
}

.tocItem:hover {
  background: var(--p-form-field-disabled-background);
  cursor: pointer;
}

</style>
