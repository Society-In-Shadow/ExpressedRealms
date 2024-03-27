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
    <h1 v-if="currentLevel == 1" :id="makeIdSafe(value.name)">
      {{ value.name }}
    </h1>
    <h2 v-if="currentLevel == 2" :id="makeIdSafe(value.name)">
      {{ value.name }}
    </h2>
    <h3 v-if="currentLevel == 3" :id="makeIdSafe(value.name)">
      {{ value.name }}
    </h3>
    <h4 v-if="currentLevel == 4" :id="makeIdSafe(value.name)">
      {{ value.name }}
    </h4>
    <h5 v-if="currentLevel == 5" :id="makeIdSafe(value.name)">
      {{ value.name }}
    </h5>
    <h6 v-if="currentLevel == 6" :id="makeIdSafe(value.name)">
      {{ value.name }}
    </h6>
    <div v-html="value.content" />
    <div>
      <ExpressionSection v-if="value.subSections" :sections="value.subSections" :current-level="props.currentLevel + 1" />
    </div>
  </div>
</template>

<style scoped>

</style>
