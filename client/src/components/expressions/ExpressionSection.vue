<script setup lang="ts">

import EditExpressionSection from "@/components/expressions/EditExpressionSection.vue";

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
  },
  showEdit:{
    type: Boolean,
    required: true
  }
});

</script>

<template>
  <div v-for="(value) in props.sections" :key="value.id">
    <EditExpressionSection :section-info="value" :current-level="currentLevel" :show-skeleton="showSkeleton" :show-edit="showEdit" />
    <div>
      <ExpressionSection v-if="value.subSections" :sections="value.subSections" :current-level="props.currentLevel + 1" :show-skeleton="showSkeleton" :show-edit="showEdit" />
    </div>
  </div>
</template>
