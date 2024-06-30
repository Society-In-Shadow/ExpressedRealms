<script setup lang="ts">

import Skeleton from 'primevue/skeleton';

defineProps({
  showSkeleton: {
    type: Boolean,
    required: true,
  },
});

</script>

<template>
  <div>
    <skeleton v-if="showSkeleton" v-bind="$attrs" />
    <slot v-else />
  </div>
</template>
