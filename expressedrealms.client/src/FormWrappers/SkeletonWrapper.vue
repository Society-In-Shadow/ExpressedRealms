<script setup lang="ts">

import Skeleton from 'primevue/skeleton';

const props = defineProps({
  showSkeleton: {
    type: Boolean,
    required: true,
  },
});

</script>

<template>
  <div>
    <skeleton v-if="showSkeleton" v-bind="$attrs"></skeleton>
    <slot v-else></slot>
  </div>
</template>

<style scoped>

</style>