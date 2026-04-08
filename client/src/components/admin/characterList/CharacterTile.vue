<script setup lang="ts">
import type { PropType } from 'vue'
import Button from 'primevue/button'
import Card from 'primevue/card'
import type { PrimaryCharacter } from '@/components/admin/characterList/types.ts'
import { useRouter } from 'vue-router'
import { adminXpScheduleDialogs } from '@/components/admin/assignedXp/services/dialogs.ts'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'
import { adminCharacterListStore } from '@/components/admin/characterList/stores/characterListStore.ts'
import { downloadFile } from '@/utilities/downloadUtility.ts'

const userPermissionInfo = userPermissionStore()
const permissionCheck = userPermissionInfo.permissionCheck
const router = useRouter()
const assignedXpDialogs = adminXpScheduleDialogs()
const characterListInfo = adminCharacterListStore()

const props = defineProps({
  character: {
    type: Object as PropType<PrimaryCharacter>,
    required: true,
  },
})

async function redirectToCharacterSheet() {
  await router.push({ name: 'characterSheet', params: { id: props.character.id } })
}

async function downloadCharacterBooklet(characterId: number, characterName: string, playerName: string) {
  await downloadFile(`/characters/${characterId}/getcrb`, `${characterName} - ${playerName} - CRB.pdf`)
  await characterListInfo.fetchCharacters()
}

</script>

<template>
  <Card class="mb-3">
    <template #title>
      <div class="d-flex flex-column flex-md-row justify-content-between">
        <div class="w-100">
          <h2 class="m-0 p-0">
            {{ props.character?.name }}
          </h2>
          <em class="small">{{ props.character?.playerName }} ({{ props.character.playerNumber.toString().padStart(3, '0') }})</em>
          <div>
            {{ props.character.expression }}
          </div>
        </div>
        <div class="text-right">
          <Button v-if="permissionCheck.CharacterManagement.ViewCharacterSheet" label="Character Sheet" class="m-2" @click="redirectToCharacterSheet()" />
          <Button v-if="permissionCheck.PlayerExperience.View" label="Assigned XP" class="m-2" @click="assignedXpDialogs.showAssignedXp(props.character.id, false)" />
          <Button v-if="permissionCheck.CharacterManagement.ViewCharacterSheet" label="CRB" class="m-2" @click="downloadCharacterBooklet(props.character.id, props.character.name, props.character?.playerName)" />
        </div>
      </div>
    </template>
  </Card>
</template>
