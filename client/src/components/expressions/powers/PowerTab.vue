<script setup lang="ts">
import ListPowerPaths from "@/components/expressions/powerPaths/ListPowerPaths.vue";
const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  }
});
</script>

<template>
  <ListPowerPaths :expression-id="props.expressionId" />
</template>
