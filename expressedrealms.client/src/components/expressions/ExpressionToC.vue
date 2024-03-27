<script setup lang="ts">

import {makeIdSafe} from "@/utilities/stringUtilities";

const props = defineProps({
  sections: {
    type: Array,
    required: true,
  },
  currentLevel: {
    type: Number,
    required: true
  }
});

</script>

<template>
  <div v-for="(value) in props.sections" :key="value.id">
    <a class="p-1 tocItem" :style="{ marginLeft: currentLevel * 10 + 'px' }" :href="'#' + makeIdSafe(value.name)">{{ value.name }}</a>
    <div>
      <ExpressionToC v-if="value.subSections" :sections="value.subSections" :current-level="props.currentLevel + 1" />
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
  background: green;
  cursor: pointer;
}

</style>
