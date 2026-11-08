<script setup lang="ts">

import { makeIdSafe } from '@/utilities/stringUtilities'
import Skeleton from 'primevue/skeleton'
import { scrollToSection } from '@/components/expressions/expressionUtilities'
import { factionListQuery } from '@/components/expressions/factions/stores/factionStore.ts'
import { expressionStore } from '@/stores/expressionStore.ts'
import { useQuery } from '@pinia/colada'

const expression = expressionStore()

const { data, isLoading } = useQuery(() => factionListQuery(expression.currentExpressionId))

</script>

<template>
  <Skeleton v-if="isLoading && !data" class="mb-2" height="1.5em" />
  <div v-for="faction in data?.factions ?? []" v-else :key="faction.id">
    <a class="p-1 tocItem" :href="'#' + makeIdSafe(faction.name)" @click.prevent="scrollToSection(faction.name)">{{ faction.name }}</a>
    <div v-for="level in faction.factionLevels" :key="level.id" class="ps-4">
      <a v-if="level.power" class="p-1 tocItem" :href="'#' + makeIdSafe(level.power?.name)" @click.prevent="scrollToSection(level.power?.name)">{{ level.power?.name ?? "No Power" }}</a>
    </div>
  </div>
</template>

<style scoped>

.tocItem{
  text-decoration: none;
  display: block;
  color: inherit;
}

.tocItem:hover {
  background: var(--p-form-field-disabled-background);
  cursor: pointer;
}

</style>
