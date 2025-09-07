<script setup lang="ts">

import {onBeforeMount, ref} from "vue";
import {useRoute} from "vue-router";
import {characterPowersStore} from "@/components/characters/character/powers/stores/characterPowerStore.ts";
import SplitButton from "primevue/splitbutton";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import PowerCard from "@/components/characters/character/wizard/powers/supporting/PowerCard.vue";
import PickPowerCard from "@/components/characters/character/wizard/powers/supporting/PickPowerCard.vue";

const characterKnowledgeData = characterPowersStore();
const route = useRoute();
const experienceInfo = experienceStore();

onBeforeMount(async () => {
  await characterKnowledgeData.getSelectableCharacterPowers(route.params.id);
  await characterKnowledgeData.getCharacterPowers(route.params.id)
  if(characterKnowledgeData.powers.length === 0)
  {
    noPowers.value = true;
    await toggleEdit();
  }
})

const noPowers = ref(false);


const items = [
  {
    label: '2x6 Tile Letter Cutout',
    command: () => {
      characterKnowledgeData.downloadPowerCards(route.params.id, 'foo', false)
    }
  },
];

</script>

<template>
  <div style="max-width: 650px; margin: 0 auto;">
    <div class="text-right pb-3" v-if="experienceInfo.showAllExperience">{{ experienceInfo.experienceBreakdown.powersXp}} Total XP - {{experienceInfo.experienceBreakdown.setupPowersXp}} Creation XP = {{experienceInfo.experienceBreakdown.powersXp - experienceInfo.experienceBreakdown.setupPowersXp}} XP</div>
    <div v-if="!noPowers || characterKnowledgeData.powers.length > 0" class="d-flex flex-row justify-content-between mb-2">
      <SplitButton class="pr-3" label="Download Power Cards" @click="characterKnowledgeData.downloadPowerCards(route.params.id, 'foo', true)" :model="items" />
    </div>
    <div v-for="path in characterKnowledgeData.powers">
      <h1>{{path.name}}</h1>
      <PowerCard :power-path="path" :show-edit="true"/>
    </div>
    
    <div class="mb-2">
      <hr v-if="characterKnowledgeData.powers.length !== 0">
      <h1>Choose Powers</h1>
      <div v-if="characterKnowledgeData.powers.length === 0">
        <p>No Powers detected, please pick one below.</p>
      </div>
      <div v-for="path in characterKnowledgeData.selectablePowers">
        <h2>{{path.name}}</h2>
        <div v-for="power in path.powers" :key="power.id">
          <PickPowerCard :power="power" :show-pick-button="true"/>
        </div>
      </div>
    </div>
  </div>
</template>
