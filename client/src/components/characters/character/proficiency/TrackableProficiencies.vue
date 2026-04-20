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
