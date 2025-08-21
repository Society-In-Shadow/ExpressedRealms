<script setup lang="ts">

import Tabs from 'primevue/tabs';
import TabList from 'primevue/tablist';
import Tab from 'primevue/tab';
import TabPanels from 'primevue/tabpanels';
import TabPanel from 'primevue/tabpanel';

import Card from "primevue/card";
import SmallStatDisplay from "@/components/characters/character/SmallStatDisplay.vue";
import SkillTile from "@/components/characters/character/skills/SkillTile.vue";
import DataTable from "primevue/datatable";

import ProficiencyTableTile from "@/components/characters/character/proficiency/ProficiencyTableTile.vue";
import CharacterDetailTile from "@/components/characters/character/CharacterDetailTile.vue";
import TrackableProficiencies from "@/components/characters/character/proficiency/TrackableProficiencies.vue";
import KnowledgeTile from "@/components/characters/character/knowledges/KnowledgeTile.vue";
import {FeatureFlags, userStore} from "@/stores/userStore.ts";
import {onMounted, ref} from "vue";
import PowerTile from "@/components/characters/character/powers/PowerTile.vue";


const userData = userStore();

const showPowersTab = ref(false);

onMounted(async() =>{
  showPowersTab.value = await userData.hasFeatureFlag(FeatureFlags.ShowCharacterPowers)
})

</script>

<template>
  <div class="d-none">
    <DataTable />
  </div>
  <div class="flex flex-xs-column flex-sm-column flex-lg-row flex-md-row gap-3 m-1 m-sm-3 m-md-3 m-lg-3 m-xl-3 flex-wrap center-content">
    <CharacterDetailTile />
    <Card class="align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch">
      <template #content>
        <SmallStatDisplay />
      </template>
    </Card>
    
    <TrackableProficiencies />
    
    <Tabs value="0" class="w-100" scrollable :lazy="true" >
      <TabList>
        <Tab value="0">
          Proficiencies
        </Tab>
        <Tab value="1">
          Skills
        </Tab>
        <Tab value="2">
          Knowledges
        </Tab>
        <Tab value="3" v-if="showPowersTab" >
          Powers
        </Tab>
      </TabList>
      <TabPanels class="p-2 p-md-3">
        <TabPanel value="0">
          <ProficiencyTableTile />
        </TabPanel>
        <TabPanel value="1">
          <SkillTile />
        </TabPanel>
        <TabPanel value="2">
          <KnowledgeTile />
        </TabPanel>
        <TabPanel v-if="showPowersTab" value="3">
          <PowerTile/>
        </TabPanel>
      </TabPanels>
    </Tabs>
  </div>
</template>

<style scoped>
@media (max-width: 576px) {
  .flex-xs-column {
    flex-direction: column !important;
  }
}

.center-content {
  max-width: 72em;
  margin: 0 auto !important;
}

</style>
