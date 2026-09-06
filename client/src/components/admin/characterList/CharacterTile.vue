<script setup lang="ts">
import { onMounted, type PropType, ref } from 'vue'
import Button from 'primevue/button'
import type { PrimaryCharacter } from '@/components/admin/characterList/types.ts'
import { useRouter } from 'vue-router'
import { adminXpScheduleDialogs } from '@/components/admin/assignedXp/services/dialogs.ts'
import { can, userPermissionStore } from '@/stores/userPermissionStore.ts'
import { adminCharacterListStore } from '@/components/admin/characterList/stores/characterListStore.ts'
import { downloadFile } from '@/utilities/downloadUtility.ts'
import { characterGoFieldsDialog } from '@/components/admin/characterList/services/dialogs.ts'
import CommandButton, { type Command } from '@/uiComponents/CommandButton.vue'

const userPermissionInfo = userPermissionStore()
const permissionCheck = userPermissionInfo.permissionCheck
const router = useRouter()
const assignedXpDialogs = adminXpScheduleDialogs()
const characterGoFieldsDialogs = characterGoFieldsDialog()
const characterListInfo = adminCharacterListStore()

const props = defineProps({
  character: {
    type: Object as PropType<PrimaryCharacter>,
    required: true,
  },
})
const items = ref<Command[]>([])
onMounted(() => {
  if (can.CharacterManagement.ViewCharacterSheet) {
    items.value.push({
      label: 'CRB',
      command: async ($event) => {
        await downloadCharacterBooklet(props.character.id, props.character.name, props.character?.playerName)
      },
    })
    items.value.push({
      label: 'CRB (Ignore Diff)',
      command: async ($event) => {
        await downloadCharacterBookletOverride(props.character.id, props.character.name, props.character?.playerName)
      },
    })
  }
  if (can.PlayerExperience.View) {
    items.value.push({
      label: 'Assigned XP',
      command: async ($event) => {
        await assignedXpDialogs.showAssignedXp(props.character.id, false)
      },
    })
  }
  if (can.CharacterManagement.ModifyGoFields) {
    items.value.push({
      label: 'GO Fields',
      command: async ($event) => {
        await characterGoFieldsDialogs.showUpdateGoFields(props.character.id)
      },
    })
  }
})

async function redirectToCharacterSheet() {
  await router.push({ name: 'characterSheet', params: { id: props.character.id } })
}

async function downloadCharacterBooklet(characterId: number, characterName: string, playerName: string) {
  await downloadFile(`/characters/${characterId}/getcrb?&UseLatestApproved=true`, `${characterName} - ${playerName} - CRB.pdf`)
  await characterListInfo.fetchCharacters()
}

async function downloadCharacterBookletOverride(characterId: number, characterName: string, playerName: string) {
  await downloadFile(`/characters/${characterId}/getcrb?&UseLatestApproved=true&OverwriteArchiveDiff=true`, `${characterName} - ${playerName} - CRB.pdf`)
  await characterListInfo.fetchCharacters()
}

</script>

<template>
  <div class="d-flex flex-column flex-md-row pt-3 pb-3">
    <div class="d-flex align-content-end flex-row order-last order-md-first pr-3">
      <Button v-if="permissionCheck.CharacterManagement.ViewCharacterSheet" label="Character Sheet" class="mr-2 text-md-nowrap" size="small" @click="redirectToCharacterSheet()" />
      <CommandButton :commands="items" size="small" />
    </div>
    <div class="align-self-md-center align-content-start">
      <h3 class="m-0 p-0">
        {{ props.character?.name }} - {{ props.character.expression }} - <em class="small">{{ props.character?.playerName }} ({{ props.character.playerNumber.toString().padStart(3, '0') }})</em>
      </h3>
    </div>
  </div>
</template>
