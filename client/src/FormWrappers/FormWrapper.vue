<script setup lang="ts">

import { computed, provide } from 'vue'

const props = defineProps({
  showSkeleton: {
    type: Boolean,
    default: false,
  },
  isDisabled: {
    type: Boolean,
    default: false,
  },
})

const showSkeleton = computed(() => props.showSkeleton)
provide('showSkeleton', showSkeleton)

const isDisabled = computed(() => props.isDisabled)
provide('isDisabled', isDisabled)

</script>

<template>
  <form v-bind="$attrs">
    <slot />
  </form>
</template>
