<script setup lang="ts">

import { type PropType } from 'vue'
import Card from 'primevue/card'
import { useRouter } from 'vue-router'
import type { PlayerDto } from './types.ts'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'
import Button from 'primevue/button'

const userPermissionInfo = userPermissionStore()
const permissionCheck = userPermissionInfo.permissionCheck
const router = useRouter()

const props = defineProps({
  item: {
    type: Object as PropType<PlayerDto>,
    required: true,
  },
})

async function redirectToCharacterSheet() {
  await router.push({ name: 'characterSheet', params: { id: props.item.id } })
}
</script>

<template>
  <Card class="mb-3">
    <template #title>
      <div class="d-flex flex-column flex-md-row justify-content-between">
        <div class="w-100">
          <h2 class="m-0 p-0">
            {{ props.item?.characterName }}
          </h2>
          <em class="small">{{ props.item?.player }}</em>
          <div>
            {{ props.item?.levelName }}
          </div>
        </div>
        <div class="text-right">
          <Button v-if="permissionCheck.CharacterManagement.ViewCharacterSheet" label="Character Sheet" class="m-2" @click="redirectToCharacterSheet()" />
        </div>
      </div>
    </template>
  </Card>
</template>
