<script setup lang="ts">
import SplitButton from 'primevue/splitbutton'
import {computed, onBeforeMount} from 'vue'
import {useRoute} from 'vue-router'
import {characterPowersStore} from '@/components/characters/character/powers/stores/characterPowerStore.ts'
import PowerCard from '@/components/characters/character/powers/PowerCard.vue'

const characterKnowledgeData = characterPowersStore()
const route = useRoute()

onBeforeMount(async () => {
  await characterKnowledgeData.getCharacterPowers(route.params.id)
})

const noPowers = computed(() => {
  return characterKnowledgeData.powers.length === 0
})

const items = [
  {
    label: '2x6 Tile Letter Cutout',
    command: () => {
      characterKnowledgeData.downloadPowerCards(route.params.id, 'foo', false)
    },
  },
]

</script>

<template>
  <div v-if="!noPowers" class="d-flex flex-row justify-content-end mb-2">
    <SplitButton class="pr-3" label="Download Power Cards" :model="items" @click="characterKnowledgeData.downloadPowerCards(route.params.id, 'foo', true)" />
  </div>
  <div style="max-width: 650px; margin: 0 auto;">
    <div v-for="path in characterKnowledgeData.powers">
      <h1>{{ path.name }}</h1>
      <PowerCard :power-path="path" />
    </div>
  </div>
</template>
