<script setup lang="ts">

import Card from 'primevue/card'
import Skeleton from 'primevue/skeleton'
import { useQueryWithLoading } from '@/utilities/queryOverride.ts'
import { factionListQuery } from '@/components/expressions/factions/stores/factionStore.ts'
import { characterStore } from '@/components/characters/character/stores/characterStore.ts'
import { computed } from 'vue'
import { pickedFactionQuery } from '@/components/characters/character/factions/stores/factionStore.ts'
import CharacterFactionEdit from '@/components/characters/character/factions/CharacterFactionEdit.vue'

const characterInfo = characterStore()

const { data, isLoading, error } = useQueryWithLoading(factionListQuery(characterInfo.expressionId))

const { data: characterData, isLoading: characterDataLoading } = useQueryWithLoading(pickedFactionQuery(characterInfo.characterId))

const selectedFaction = computed(() => {
  if (characterData.value?.factionId) {
    return data.value?.factions.find(
      faction => faction.id === characterData.value!.factionId,
    )
  }
  return null
})
</script>

<template>
  <div v-if="isLoading || characterDataLoading">
    <Skeleton v-for="height in 3" :key="height" class="mb-3 mt-3" height="100px" />
  </div>
  <div v-else-if="error">
    <Card>
      <template #title>
        Error Loading Factions
      </template>
      <template #content>
        Please try again, or open an issue on discord
      </template>
    </Card>
  </div>
  <div v-else-if="data && data.factions.length === 0">
    <Card>
      <template #title>
        No Factions
      </template>
      <template #content>
        <p>
          There are no known factions for this expression
        </p>
      </template>
    </Card>
  </div>
  <div v-else-if="!selectedFaction">
    <p>You have not joined a faction.</p>
  </div>
  <div v-if="selectedFaction">
    <CharacterFactionEdit :item="selectedFaction" />
  </div>
</template>
