<script setup lang="ts">
import type { PropType } from 'vue'
import { ref } from 'vue'
import Button from 'primevue/button'
import Card from 'primevue/card'
import type { PrimaryCharacter } from '@/components/admin/characterList/types.ts'
import { useRouter } from 'vue-router'
import { adminCharacterDialogs } from '@/components/admin/characterList/services/dialogs.ts'
import axios from 'axios'
import AssignedXpList from '@/components/admin/assignedXp/AssignedXpList.vue'

const router = useRouter()
const showInfo = ref(false)
const dialogs = adminCharacterDialogs()

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
        <div>
          <h2 class="m-0 p-0">
            {{ props.character?.name }}
          </h2>
          <em class="small">{{ props.character?.playerName }} ({{ props.character.playerNumber.toString().padStart(3, '0') }})</em>
          <div>
            {{ props.character.expression }}
          </div>
        </div>
        <div>
          <Button :label="showInfo ? 'Cancel' : 'Quick Notes'" class="m-2" @click="showInfo = !showInfo" />
          <Button label="Character Sheet" class="m-2" @click="redirectToCharacterSheet()" />
          <Button label="Update Character" class="m-2" @click="dialogs.showUpdateXp(props.character.id, props.character.playerNumber, props.character.assignedXp)" />
          <Button label="CRB" class="m-2" @click="downloadCharacterBooklet(props.character.id, props.character.name, props.character?.playerName)" />
        </div>
      </div>
    </template>
    <template #content>
      <div v-if="showInfo">
        <h1>Assigned XP</h1>
        <AssignedXpList :character-id="props.character.id" :is-read-only="false" />
        <h3>Backstory</h3>
        {{ props.character.background ?? "No Background has been posted for this character." }}
      </div>
    </template>
  </Card>
</template>
