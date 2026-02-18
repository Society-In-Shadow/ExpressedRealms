<script setup lang="ts">
import type { PropType } from 'vue'
import Button from 'primevue/button'
import Card from 'primevue/card'
import type { PrimaryCharacter } from '@/components/admin/characterList/types.ts'
import { useRouter } from 'vue-router'
import axios from 'axios'
import { adminXpScheduleDialogs } from '@/components/admin/assignedXp/services/dialogs.ts'

const router = useRouter()
const assignedXpDialogs = adminXpScheduleDialogs()

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
  const res = await axios.get(`/characters/${characterId}/getcrb`, {
    responseType: 'blob',
  })
  const url = URL.createObjectURL(res.data)
  const a = document.createElement('a')
  a.href = url
  a.download = `${characterName} - ${playerName} - CRB.pdf`
  document.body.appendChild(a)
  a.click()
  a.remove()
  URL.revokeObjectURL(url)
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
          <Button label="Character Sheet" class="m-2" @click="redirectToCharacterSheet()" />
          <Button label="Assigned XP" class="m-2" @click="assignedXpDialogs.showAssignedXp(props.character.id, false)" />
          <Button label="CRB" class="m-2" @click="downloadCharacterBooklet(props.character.id, props.character.name, props.character?.playerName)" />
        </div>
      </div>
    </template>
  </Card>
</template>
