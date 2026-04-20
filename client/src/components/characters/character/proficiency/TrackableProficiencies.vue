<script setup lang="ts">

import Card from 'primevue/card'
import Accordion from 'primevue/accordion'
import ProficiencyAccordionPanel from '@/components/characters/character/proficiency/ProficiencyAccordionPanel.vue'
import { ref } from 'vue'
import { proficienciesByCharacterId } from '@/components/characters/character/proficiency/stores/proficiencyStore'
import { useRoute } from 'vue-router'
import Skeleton from 'primevue/skeleton'
import { useQueryWithLoading } from '@/utilities/queryOverride.ts'

const route = useRoute()

const props = defineProps({
  showTitle: {
    type: Boolean,
    required: false,
    default: true,
  },
  showHelpText: {
    type: Boolean,
    required: false,
    default: false,
  },
})

const { data, isLoading } = useQueryWithLoading(proficienciesByCharacterId(Number.parseInt(route.params.id)))

const openItems = ref([])
</script>

<template>
  <Card class="p-1 p-md-3">
    <template #header>
      <h1 v-if="props.showTitle" class="text-center p-0 m-0">
        Secondary Statistics
      </h1>
    </template>
    <template #content>
      <p v-if="showHelpText">
        Secondary Statistics are, for the most part, derived from your character's primary Statistics and serve as a
        measure of your character's capacity and inner nature.
      </p>
      <p v-if="showHelpText">
        These are your resources that you will spending and keeping track of throughout the course of your adventure.
      </p>
      <p v-if="showHelpText">
        Click on each one below to get more details on how they are calculated
      </p>
      <div v-if="isLoading">
        <Skeleton height="3em" />
        <Skeleton height="3em" />
        <Skeleton height="3em" />
        <Skeleton height="3em" />
        <Skeleton height="3em" />
        <Skeleton height="3em" />
        <Skeleton height="3em" />
        <Skeleton height="3em" />
      </div>
      <div v-else>
        <Accordion
          :value="openItems" multiple :lazy="true" expand-icon="pi pi-info-circle"
          collapse-icon="pi pi-times-circle"
        >
          <ProficiencyAccordionPanel v-for="proficiency in data?.secondary ?? []" :key="proficiency.id" :value="proficiency.id" :proficiency="proficiency" />
        </Accordion>
      </div>
    </template>
  </Card>
</template>
