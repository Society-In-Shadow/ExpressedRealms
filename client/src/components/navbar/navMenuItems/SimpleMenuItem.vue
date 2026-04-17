<script setup lang="ts">

import type { SimpleMenuItem } from '@/components/navbar/navMenuItems/types.ts'
import { type PropType } from 'vue'
import { navigateWithNewTabSupport } from '@/router/navigationHelpers.ts'

let props = defineProps({
  item: {
    type: Object as PropType<SimpleMenuItem>,
    required: true,
  },
})

function redirect(event: MouseEvent) {
  navigateWithNewTabSupport({ name: props.item.pushComponentRouteName }, event)
}

</script>
<template>
  <div class="flex flex-shrink-1 align-items-center cursor-pointer gap-2 mb-2" @click="redirect" @click.middle="redirect">
    <div class="flex gap-3 align-items-center flex-grow-1 p-3">
      <span class="inline-flex flex-none align-items-center justify-content-center border-circle bg-primary w-3rem h-3rem">
        <i :class="['material-symbols-outlined', 'text-white']"> {{ item.navMenuIcon }}</i>
      </span>
      <span class="inline-flex flex-grow-1 flex-column gap-1">
        <span class="font-medium text-lg text-900">{{ item.label }}</span>
        <span>{{ item.description }}</span>
      </span>
    </div>
  </div>
</template>
