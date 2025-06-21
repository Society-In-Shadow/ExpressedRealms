<script setup lang="ts">

import {makeIdSafe} from "@/utilities/stringUtilities";
import Skeleton from 'primevue/skeleton';
import {BaseTree, Draggable} from "@he-tree/vue";
import {scrollToSection} from "@/components/expressions/expressionUtilities";
import {powerPathStore} from "@/components/expressions/powerPaths/stores/powerPathStore";

const powerPaths = powerPathStore();

const props = defineProps({
  canEdit:{
    type: Boolean
  },
  showSkeleton:{
    type: Boolean,
    required: true
  }
});

</script>

<template>
  <Draggable
    v-if="props.canEdit"
    v-model="powerPaths.powerPaths"
    class="mtl-tree"
    children-key="powers"
    update-behavior="new"
    text-key="name"
  >
    <template #default="{ node }">
      <div class="p-1">
        <i class="pi pi-bars mr-2" />{{ node.name }}
      </div>
    </template>
  </Draggable>
  <BaseTree
    v-else 
    v-model="powerPaths.powerPaths" 
    children-key="powers"
    text-key="name"
  >
    <template #default="{ node }">
      <Skeleton v-if="props.showSkeleton" id="toc-skeleton" class="mb-2" height="1.5em" />
      <a v-else class="p-1 tocItem" :href="'#' + makeIdSafe(node.name)" @click.prevent="scrollToSection(node.name)">{{ node.name }}</a>
    </template>
  </BaseTree>
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
