<script setup lang="ts">

import { ref } from 'vue'
import { useRoute } from 'vue-router'
import Button from 'primevue/button'
import Popover from 'primevue/popover'
import { proficienciesByCharacterId } from '@/components/characters/character/proficiency/stores/proficiencyStore'
import Skeleton from 'primevue/skeleton'
import { useQueryWithLoading } from '@/utilities/queryOverride.ts'

const route = useRoute()

const { data, isLoading } = useQueryWithLoading(proficienciesByCharacterId(Number.parseInt(route.params.id)))

const op = ref()
const toggle = (event) => {
  op.value.toggle(event)
}

</script>

<template>
  <Button type="button" label="Sec. Prof." class="d-block d-md-none" @click="toggle" />
  <div v-if="isLoading">
    <div class="d-md-flex flex-row justify-content-end flex-wrap d-none">
      <Skeleton height="3em" width="6em" />
      <Skeleton height="3em" width="6em" />
      <Skeleton height="3em" width="6em" />
      <Skeleton height="3em" width="6em" />
      <Skeleton height="3em" width="6em" />
      <Skeleton height="3em" width="6em" />
      <Skeleton height="3em" width="6em" />
      <Skeleton height="3em" width="6em" />
    </div>
  </div>
  <div v-else>
    <div class="d-md-flex flex-row justify-content-end flex-wrap d-none">
      <div v-for="proficiency in data?.secondary ?? []" :key="proficiency.id" class="statTile pt-2 pb-2 pr-3 pl-3 mr-2 ml-2 mt-3">
        <span class="mr-3">{{ proficiency.name }}</span>
        <span>{{ proficiency.value }}</span>
      </div>
    </div>

    <Popover ref="op" class="d-md-none">
      <div class="d-flex flex-row justify-content-end flex-wrap">
        <div v-for="proficiency in data?.secondary ?? []" :key="proficiency.id" class="statTile pt-2 pb-2 pr-3 pl-3 mr-2 ml-2 mt-3">
          <span class="mr-3">{{ proficiency.name }}</span>
          <span>{{ proficiency.value }}</span>
        </div>
      </div>
    </Popover>
  </div>
</template>

<style scoped>
  .statTile {
    border-radius: 4px; /* Rounding of corners */
    border: 1px solid var(--p-form-field-disabled-background);
    background-color: var(--p-card-background);
  }

</style>
