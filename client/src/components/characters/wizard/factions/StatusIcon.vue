<script setup lang="ts">
defineProps<{
  value: boolean | null | undefined
}>()
</script>

<template>
  <span>{{ value ? '✅' : '❌' }}</span>
</template>
