<script setup lang="ts">

import {makeIdSafe} from "@/utilities/stringUtilities";
import Skeleton from 'primevue/skeleton';

const props = defineProps({
  sections: {
    type: Array,
    required: true,
  },
  currentLevel: {
    type: Number,
    required: true
  },
  showSkeleton:{
    type: Boolean,
    required: true
  }
});

</script>

<template>
  <div v-for="(value) in props.sections" :key="value.id">
    <Skeleton v-if="showSkeleton" id="toc-skeleton" class="mb-2" :style="{ marginLeft: currentLevel * 10 + 'px' }" height="1.5em" />
    <a v-else class="p-1 tocItem" :style="{ marginLeft: currentLevel * 10 + 'px' }" :href="'#' + makeIdSafe(value.name)">{{ value.name }}</a>
    <div>
      <ExpressionToC v-if="value.subSections" :sections="value.subSections" :current-level="props.currentLevel + 1" :show-skeleton="showSkeleton" />
    </div>
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
