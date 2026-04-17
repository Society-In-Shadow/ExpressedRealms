<script setup lang="ts">
import { PiniaColadaDevtools } from '@pinia/colada-devtools'
import { computed } from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()

// Temporary workaround for refreshing character sheet upon navigation
// Character Sheet needs to use watch
const routeKey = computed(() => {
  if (route.meta.reloadOnParamChange) {
    return Number.parseInt(route.params.id)
  }

  return 'noReset'
})

</script>
<template>
  <router-view :key="routeKey" />

  <PiniaColadaDevtools />
</template>
