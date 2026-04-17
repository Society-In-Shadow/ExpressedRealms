<script setup lang="ts">
import ExpressionLogo from '@/components/common/ExpressionLogo.vue'
import Badge from 'primevue/badge'
import { CharacterState } from '@/components/navbar/types.ts'
import { navigateWithNewTabSupport } from '@/router/navigationHelpers.ts'

const props = defineProps({
  item: {
    type: Object,
    required: true,
  },
})

async function redirect(event: MouseEvent) {
  let route = { name: 'characterSheet', params: { id: props.item?.id } }

  if (props.item.id === -1) {
    route = { name: 'characters' }
  }
  if (props.item.id === -2) {
    route = { name: 'addWizard' }
  }
  navigateWithNewTabSupport(route, event)
}

</script>
<template>
  <div class="flex flex-shrink-1 align-items-center p-3 cursor-pointer gap-2" @click="redirect" @click.middle="redirect">
    <span v-if="item.id == -1" class="inline-flex flex-none align-items-center justify-content-center border-circle bg-primary w-3rem h-3rem ">
      <i class="material-symbols-outlined text-white">list</i>
    </span>
    <span v-else-if="item.id == -2" class="inline-flex flex-none align-items-center justify-content-center border-circle bg-primary w-3rem h-3rem ">
      <i class="material-symbols-outlined text-white">add</i>
    </span>
    <span v-else class="inline-flex flex-none align-items-center justify-content-center border-circle bg-primary w-3rem h-3rem ">
      <ExpressionLogo :expression-name="item.expression" />
    </span>
    <span class="inline-flex flex-column gap-1 pl-2">
      <span class="font-medium text-lg text-900">
        {{ item.name }}
        <Badge v-if="item.state === CharacterState.Primary" value="Primary Character" severity="info" class="ml-2" />
        <Badge v-if="item.state === CharacterState.Retired" value="Retired" severity="warn" />
      </span>
      <span v-if="item.id > 0">{{ item.expression }}</span>
    </span>
  </div>
</template>
