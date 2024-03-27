<script setup lang="ts">

import {computed, ref} from 'vue';


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
  <div v-for="(value, key) in props.sections" :key="value.id">
    <h1 v-if="currentLevel == 1">{{value.name}}</h1>
    <h2 v-if="currentLevel == 2">{{value.name}}</h2>
    <h3 v-if="currentLevel == 3">{{value.name}}</h3>
    <h4 v-if="currentLevel == 4">{{value.name}}</h4>
    <h5 v-if="currentLevel == 5">{{value.name}}</h5>
    <h6 v-if="currentLevel == 6">{{value.name}}</h6>
    <div>{{value.content}}</div>
    <div>
      <ExpressionSection v-if="value.subSections" :sections="value.subSections" :current-level="props.currentLevel + 1"></ExpressionSection>
    </div>
    
  </div>
  
</template>

<style scoped>

</style>