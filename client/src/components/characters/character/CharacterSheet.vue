<script setup lang="ts">

import Tabs from 'primevue/tabs'
import TabList from 'primevue/tablist'
import Tab from 'primevue/tab'
import TabPanels from 'primevue/tabpanels'
import TabPanel from 'primevue/tabpanel'

import Card from 'primevue/card'
import SkillTile from '@/components/characters/character/skills/SkillTile.vue'
import DataTable from 'primevue/datatable'

import ProficiencyTableTile from '@/components/characters/character/proficiency/ProficiencyTableTile.vue'
import CharacterDetailTile from '@/components/characters/character/CharacterDetailTile.vue'
import KnowledgeTile from '@/components/characters/character/knowledges/KnowledgeTile.vue'
import PowerTile from '@/components/characters/character/powers/PowerTile.vue'
import TrackableProficiencies from '@/components/characters/character/proficiency/TrackableProficiencies.vue'
import BlessingTab from '@/components/characters/character/blessings/BlessingTab.vue'
import StatTile from '@/components/characters/character/stats/StatTile.vue'
import {characterStore} from '@/components/characters/character/stores/characterStore.ts'
import Message from 'primevue/message'
import ReviewCharacter from '@/components/characters/character/ReviewCharacter.vue'

const characterData = characterStore()

</script>

<template>
  <div class="d-none">
    <DataTable />
  </div>
  <Message v-if="!characterData.isOwner && !characterData.isLoading" severity="warn" class="mb-3 mt-3">
    You have read only access to this character sheet.
  </Message>
  <CharacterDetailTile />
  <div class="flex flex-column flex-md-row gap-3 m-1 m-sm-3 m-md-3 m-lg-3 m-xl-3 center-content">
    <div class="static-width">
      <Card class="mb-3">
        <template #content>
          <StatTile />
        </template>
      </Card>
      <Tabs value="0" class="w-100" scrollable :lazy="true">
        <TabList>
          <Tab value="-1" class="d-block d-md-none">
            Statistics
          </Tab>
          <Tab value="0">
            Proficiencies
          </Tab>
          <Tab value="1">
            Skills
          </Tab>
          <Tab value="2">
            Knowledges
          </Tab>
          <Tab value="3">
            Powers
          </Tab>
          <Tab value="4">
            Advantages / Disadvantages
          </Tab>
          <Tab value="5">
            XP Breakdown
          </Tab>
        </TabList>
        <TabPanels class="p-2 p-md-3">
          <TabPanel value="-1">
            <TrackableProficiencies :show-title="false" />
          </TabPanel>
          <TabPanel value="0">
            <ProficiencyTableTile />
          </TabPanel>
          <TabPanel value="1">
            <SkillTile />
          </TabPanel>
          <TabPanel value="2">
            <KnowledgeTile />
          </TabPanel>
          <TabPanel value="3">
            <PowerTile />
          </TabPanel>
          <TabPanel value="4">
            <BlessingTab />
          </TabPanel>
          <TabPanel value="5">
            <ReviewCharacter :is-read-only="true" />
          </TabPanel>
        </TabPanels>
      </Tabs>
    </div>
    <div class="flex-grow-1 d-none d-md-block">
      <TrackableProficiencies />
    </div>
  </div>
</template>

<style scoped>
@media (max-width: 576px) {
  .flex-xs-column {
    flex-direction: column !important;
  }
}

@media (min-width: 577px) {
  .static-width {
    width: 60em
  }
}

.center-content {
  margin: 0 auto !important;
}

</style>
