<script setup lang="ts">

import Skeleton from 'primevue/skeleton'

const props = defineProps({
  expressionName: {
    type: String,
    required: false,
  },
  isLoading: {
    type: Boolean,
    required: false,
    default: false,
  },
})

</script>

<template>
  <Skeleton v-if="props.isLoading" width="150px" height="150px" />
  <img v-else :src="'/expressions/' + props.expressionName.toLowerCase() + '_square.png'" class="w-100" style="max-width: 250px" alt="round circle">
</template>
