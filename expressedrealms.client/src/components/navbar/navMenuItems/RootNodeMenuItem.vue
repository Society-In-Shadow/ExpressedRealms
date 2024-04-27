<script setup lang="ts">
defineProps({
  item: {
    type: Object,
    required: true,
  }
});

</script>
<template>
  <a
    v-ripple
    class="flex align-items-center cursor-pointer px-3 py-2 overflow-hidden relative font-semibold text-lg uppercase"
  >
    <span>{{ item.label }}</span>
  </a>
</template>