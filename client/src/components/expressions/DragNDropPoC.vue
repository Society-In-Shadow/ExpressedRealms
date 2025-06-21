<script setup lang="ts">

import { Draggable, dragContext} from "@he-tree/vue";

const model = [
  { name: 'Parent 1', id: 1, parentId: null, subSections: [
      { name: 'Child 1', id: 1, parentId: 1, subSections: []},
      { name: 'Child 2', id: 2, parentId: 1, subSections: []},
      { name: 'Child 3', id: 3, parentId: 1, subSections: []},
      { name: 'Child 4', id: 4, parentId: 1, subSections: []},
    ]},
  { name: 'Parent 2', id: 2, parentId: null, subSections: [
      { name: 'Child 1', id: 1, parentId: 2, subSections: []},
      { name: 'Child 2', id: 2, parentId: 2, subSections: []},
      { name: 'Child 3', id: 3, parentId: 2, subSections: []},
      { name: 'Child 4', id: 4, parentId: 2, subSections: []},
    ]},
  { name: 'Parent 3', id: 3, parentId: null, subSections: []},
  { name: 'Parent 4', id: 4, parentId: null, subSections: []},
]

function eachDroppable(targetStat) {
  // Restrict dragging to nodes with the same parentId (siblings)
  const draggedParentId = dragContext.dragNode.data.parentId;
  const targetParentId = targetStat.data.parentId;

  // Only allow drop targets that share the same parentId as dragged node
  if (targetParentId !== draggedParentId) {
    return false;
  }

  // Prevent dropping on a child node's drop area (which would create nesting)
  // Check if the target node is at the same level as the drag node
  if (targetStat.level !== dragContext.dragNode.level) {
    return false;
  }

  return true;
}



function rootDroppable(targetStat) {
  return targetStat.data.parentId === dragContext.dragNode.data.parentId;
}

</script>

<template>
  <Draggable
    v-model="model"
    class="mtl-tree"
    children-key="subSections"
    update-behavior="new"
    text-key="name"
    :eachDroppable="eachDroppable"
  >
  </Draggable>

</template>

<style>

.he-tree-drag-placeholder {
  background: var(--p-form-field-disabled-background) !important;
  border: 1px dashed var(--p-button-primary-background);
  height: 1.5em;
  width: 100%;
}

.mtl-tree .tree-node:hover {
  background-color: var(--p-form-field-disabled-background);
  cursor: move;
}

.mtl-tree .tree-node-inner {
  display: flex;
  align-items: center;
  font-size: inherit;
}

</style>

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
