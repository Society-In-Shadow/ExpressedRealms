<script setup lang="ts">

import Card from 'primevue/card'
import Skeleton from 'primevue/skeleton'
import { useQueryWithLoading } from '@/utilities/queryOverride.ts'
import { participantsQuery } from '@/components/admin/factions/stores/factionStore.ts'
import CharacterItem from '@/components/admin/factions/CharacterItem.vue'

const { data, isLoading, error } = useQueryWithLoading(participantsQuery)

</script>

<template>
  <div class="d-flex flex-row align-items-center">
    <div class="flex-fill">
      <h1>Faction Participants</h1>
    </div>
  </div>
  <div v-if="isLoading">
    <Skeleton v-for="height in 3" :key="height" class="mb-3 mt-3" height="100px" />
  </div>
  <div v-else-if="error">
    <Card>
      <template #title>
        Error Loading Archetypes
      </template>
      <template #content>
        Please try again, or open an issue on discord
      </template>
    </Card>
  </div>
  <div v-else>
    <div v-for="expression in data.expressions" :key="expression.id">
      <Card class="mb-3">
        <template #content>
          <h2>{{ expression.name }}</h2>
        </template>
      </Card>

      <Card v-if="expression.factions.length == 0" class="ml-3 ml-md-5 mb-3">
        <template #content>
          <h3>No Factions for this Expression</h3>
        </template>
      </Card>
      <div v-for="faction in expression.factions" :key="faction.id" class="ml-3 ml-md-5">
        <Card class="mb-3">
          <template #content>
            <h2>{{ faction.name }}</h2>
          </template>
        </Card>

        <Card v-if="faction.players.length == 0" class="ml-3 ml-md-5 mb-3">
          <template #content>
            <h3>No Players for this Faction</h3>
          </template>
        </Card>
        <div v-for="player in faction.players" :key="player.id" class="ml-3 ml-md-5">
          <CharacterItem :item="player" />
        </div>
      </div>
    </div>
  </div>
</template>
