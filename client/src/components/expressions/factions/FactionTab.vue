<script setup lang="ts">
import FactionList from '@/components/expressions/factions/FactionList.vue'

const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  },
})
</script>

<template>
  <FactionList :expression-id="props.expressionId" />
</template>
