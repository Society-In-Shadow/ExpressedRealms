<script setup lang="ts">

import EditExpressionSection from "@/components/expressions/expressionSection/EditExpressionSection.vue";
import ExpressionSectionTile from "@/components/expressions/expressionSection/ExpressionSectionTile.vue";

const emit = defineEmits<{
  refreshList: []
}>();

function passThroughAddedSection(){
  emit("refreshList");
}

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
    <ExpressionSectionTile  :section-info="value" :current-level="currentLevel" :show-skeleton="showSkeleton" :show-edit="showEdit" @refresh-list="passThroughAddedSection"/>
    <div>
      <ExpressionSection
        v-if="value.subSections" :sections="value.subSections" :current-level="props.currentLevel + 1" :show-skeleton="showSkeleton" :show-edit="showEdit"
        @refresh-list="passThroughAddedSection"
      />
    </div>
  </div>
</template>
