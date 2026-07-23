<script setup lang="ts">

import Card from 'primevue/card'
import Skeleton from 'primevue/skeleton'
import { useQueryWithLoading } from '@/utilities/queryOverride.ts'
import { factionListQuery } from '@/components/expressions/factions/stores/factionStore.ts'
import { characterStore } from '@/components/characters/character/stores/characterStore.ts'
import { computed } from 'vue'
import FactionItem from '@/components/characters/wizard/factions/FactionItem.vue'
import { pickedFactionQuery } from '@/components/characters/wizard/factions/stores/factionStore.ts'
import CharacterFactionEdit from '@/components/characters/wizard/factions/CharacterFactionEdit.vue'

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
  <div>
    <p>All players are encouraged to join a faction, as you get a free power.  These are completely optional, and you can only join one at a time.</p>
    <p>Each rank has a requirement in terms of a knowledge you need to know, and you also need to complete one or more quests to level up.</p>
  </div>
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
  <div v-if="selectedFaction">
    <CharacterFactionEdit :item="selectedFaction" />
  </div>
  <div v-else>
    <div v-for="item in data?.factions" :key="item.id">
      <FactionItem :item="item" />
    </div>
  </div>
</template>
