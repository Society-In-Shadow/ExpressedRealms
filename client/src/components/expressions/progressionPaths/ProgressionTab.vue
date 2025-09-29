<script setup lang="ts">

import ListProgressions from "@/components/expressions/progressionPaths/ListProgressions.vue";

const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  }
});
</script>

<template>
  <ListProgressions :expression-id="props.expressionId" />
</template>
