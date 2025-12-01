<script setup lang="ts">

import { computed, provide } from 'vue'

const props = defineProps({
  showSkeleton: {
    type: Boolean,
    default: false,
  },
})

const showSkeleton = computed(() => props.showSkeleton)
provide('showSkeleton', showSkeleton)

</script>

<template>
  <form v-bind="$attrs">
    <slot />
  </form>
</template>
