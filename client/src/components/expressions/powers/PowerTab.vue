<script setup lang="ts">

import AddPower from "@/components/expressions/powers/AddPower.vue";

const props = defineProps({
  expressionId: {
    type: Number
  }
});

</script>

<template>
 <AddPower :expression-id="props.expressionId" />
</template>

<style scoped>

</style>