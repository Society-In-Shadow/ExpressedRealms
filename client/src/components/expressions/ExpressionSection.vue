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
    <div v-if="showSkeleton">
      <Skeleton id="expression-section-title-skeleton" class="mb-2" height="1.5em" />
      <Skeleton id="expression-section-body-skeleton" class="mb-2" height="5em" />
    </div>
    
    <div v-else>
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
    </div>
    <div>
      <ExpressionSection v-if="value.subSections" :sections="value.subSections" :current-level="props.currentLevel + 1" :show-skeleton="showSkeleton" />
    </div>
  </div>
</template>
