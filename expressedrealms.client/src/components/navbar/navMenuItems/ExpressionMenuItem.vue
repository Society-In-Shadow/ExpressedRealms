<script setup lang="ts">
defineProps({
  item: {
    type: Object,
    required: true,
  }
});
</script>
<template>
  <a class="flex flex-shrink-1 align-items-center p-3 cursor-pointer mb-2 gap-2">
    <span class="inline-flex flex-none align-items-center justify-content-center border-circle bg-primary w-3rem h-3rem">
      <i :class="[item.icon, 'text-lg']" />
    </span>
    <span class="inline-flex flex-column gap-1">
      <span class="font-medium text-lg text-900">{{ item.label }}</span>
      <span class="">{{ item.subtext }}</span>
    </span>
  </a>
</template>