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
import { characterStore } from '@/components/characters/character/stores/characterStore.ts'
import Checkbox from 'primevue/checkbox'

const route = useRoute()
const statData = statStore()
const characterData = characterStore()

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
  <p>
    Purchase character statistics agility (AGL), constitution (CON), dexterity (DEX), strength (STR), intelligence (INT),
    and willpower (WIL).
  </p>
  <p>These statistics affect your proficiencies, secondary statistics, feat tests, and many powers.</p>
  <p>
    Each starts at level 1, and may be purchased to increase its level. 2 is considered a human average, and 7 is
    considered the absolute pinnacle.
  </p>
  <ShowXPCosts :section-type="XpSectionTypes.stats" />
  <div>
    <DataTable :value="statData.stats" data-key="statTypeId" class="pb-3">
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
    <Message v-if="characterData.isInCharacterCreation" severity="warn">
      Extra Mortis can only be purchased outside of character creation.
    </Message>
    <div v-else>
      <h3>Mortis</h3>
      <p>
        Mortis represents your character’s ability to withstand incapacitation without dying, and is therefore the
        ultimate measure of your character’s deepest bodily reserves. You may think of it as your character’s personal
        death-danger meter.
      </p>
      <p>
        The higher your Mortis, the better the chance you will survive being incapacitated. Supernatural characters
        begin with a max Mortis of 7. It is possible for a supernatural character to develop it, with experience, to 8.
      </p>
      <p>Extra Mortis is a one time purchase you can opt into after character creation for 12xp.  As it implies, it gives you one extra mortis point.</p>
      <Checkbox v-model="statData.hasExtraMortis" input-id="extra-mortis" binary /><label class="ml-2" for="extra-mortis">+1 Mortis - 12xp</label>
    </div>
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
