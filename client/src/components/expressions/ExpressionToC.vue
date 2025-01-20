<script setup lang="ts">

import {makeIdSafe} from "@/utilities/stringUtilities";
import Skeleton from 'primevue/skeleton';
import {BaseTree, Draggable} from "@he-tree/vue";
import {onMounted, onUpdated, watch} from 'vue';

import '@he-tree/vue/style/default.css'
import '@he-tree/vue/style/material-design.css'

const model = defineModel({ required: true, default: {}, type: Object });

let plainModel;
watch(model, (newValue, oldValue) => {
  plainModel = JSON.parse(JSON.stringify(model.value));
}, { deep: true });

const props = defineProps({
  editMenu:{
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
      v-if="props.editMenu"
      class="mtl-tree"
      v-model="plainModel"
      childrenKey="subSections"
      textKey="name">
    <template #default="{ node, stat }">
      <Skeleton v-if="props.showSkeleton" id="toc-skeleton" class="mb-2" height="1.5em" />
      <a v-else class="p-1 tocItem" :href="'#' + makeIdSafe(node.name)">{{ node.name }}</a>
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
