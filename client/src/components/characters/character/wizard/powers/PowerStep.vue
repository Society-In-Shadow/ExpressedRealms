<script setup lang="ts">

import {onBeforeMount, ref} from "vue";
import {useRoute} from "vue-router";
import {characterPowersStore} from "@/components/characters/character/powers/stores/characterPowerStore.ts";
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
    <h1>Selected Powers</h1>
    <div v-for="path in characterKnowledgeData.powers">
      <h2>{{path.name}}</h2>
      <PowerCard :power-path="path" :show-edit="true"/>
    </div>
    
    <div class="mb-2">
      <hr v-if="characterKnowledgeData.powers.length !== 0">
      <h1 class="pb-0 mb-0">Choose Powers</h1>
      <div v-if="experienceInfo.showAllExperience">{{ experienceInfo.experienceBreakdown.powersXp}} Total XP - {{experienceInfo.experienceBreakdown.setupPowersXp}} Creation XP = {{experienceInfo.experienceBreakdown.powersXp - experienceInfo.experienceBreakdown.setupPowersXp}} XP</div>

      <div v-if="characterKnowledgeData.powers.length === 0">
        <p>No Powers detected, please pick one below.</p>
      </div>
      <div v-for="path in characterKnowledgeData.selectablePowers">
        <h2>{{path.name}}</h2>
        <div v-for="power in path.powers" :key="power.id" class="pb-2">
          <PickPowerCard :power="power" :show-pick-button="true"/>
        </div>
      </div>
    </div>
  </div>
</template>
