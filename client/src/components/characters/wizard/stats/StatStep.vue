<script setup lang="ts">

import { onMounted } from 'vue'
import { useRoute } from 'vue-router'
import SkeletonWrapper from '@/FormWrappers/SkeletonWrapper.vue'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import StatTile from '@/components/characters/wizard/stats/StatTile.vue'
import ShowXPCosts from '@/components/characters/wizard/ShowXPCosts.vue'
import { statStore } from '@/components/characters/wizard/stats/stores/statStore.ts'
import { wizardContentStore } from '@/components/characters/wizard/stores/wizardContentStore.ts'
import type { WizardContent } from '@/components/characters/wizard/types.ts'
import { XpSectionTypes } from '@/components/characters/character/stores/experienceBreakdownStore.ts'
import TrackableProficiencies from '@/components/characters/character/proficiency/TrackableProficiencies.vue'

const route = useRoute()
const statData = statStore()

onMounted(async () => {
  await statData.loadData(route.params.id)
})

const wizardContentInfo = wizardContentStore()
const updateWizardContent = (statTypeId: number) => {
  wizardContentInfo.updateContent(
    {
      headerName: 'Edit Stat Type',
      component: StatTile,
      props: { statTypeId: statTypeId },
    } as WizardContent,
  )
}

</script>

<template>
  <h2>Stats</h2>
  <ShowXPCosts :section-type="XpSectionTypes.stats" />
  <div>
    <DataTable :value="statData.stats" data-key="statTypeId">
      <Column field="name" header="Name">
        <template #body="slotProps">
          <SkeletonWrapper height="1.5rem" width="2rem" :show-skeleton="statData.isLoading">
            {{ slotProps.data.name }}
          </SkeletonWrapper>
        </template>
      </Column>
      <Column field="level" header="Level (Bonus)" header-class="text-center" body-class="text-center">
        <template #body="slotProps">
          <SkeletonWrapper height="1.5rem" width="2rem" :show-skeleton="statData.isLoading">
            {{ slotProps.data.level }} (<span v-if="slotProps.data.bonus > 0">+</span>{{ slotProps.data.bonus }})
          </SkeletonWrapper>
        </template>
      </Column>
      <Column>
        <template #body="slotProps">
          <Button class="float-end " size="small" label="View" @click="updateWizardContent(slotProps.data.statTypeId)" />
        </template>
      </Column>
    </DataTable>
    <div class="mt-3">
      <TrackableProficiencies :show-help-text="true" />
    </div>
  </div>
</template>

<style scoped>

:deep(th.text-center .p-datatable-column-header-content) {
  justify-content: center;
}

</style>
