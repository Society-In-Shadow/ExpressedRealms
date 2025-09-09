<script setup lang="ts">

import Button from "primevue/button";
import {onBeforeMount, ref} from "vue";
import {useRoute} from "vue-router";
import {characterPowersStore} from "@/components/characters/character/powers/stores/characterPowerStore.ts";
import PowerCard from "@/components/characters/character/powers/PowerCard.vue";
import PickPowerCard from "@/components/characters/character/powers/PickPowerCard.vue";
import SplitButton from "primevue/splitbutton";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import {FeatureFlags, userStore} from "@/stores/userStore.ts";

const characterKnowledgeData = characterPowersStore();
const route = useRoute();
const experienceInfo = experienceStore();
const userInfo = userStore();

onBeforeMount(async () => {
  await characterKnowledgeData.getCharacterPowers(route.params.id)
  if(characterKnowledgeData.powers.length === 0)
  {
    noPowers.value = true;
    await toggleEdit();
  }
  showCharacterWizard.value = await userInfo.hasFeatureFlag(FeatureFlags.ShowCharacterWizard);
})

const showEdit = ref(false);
const noPowers = ref(false);
const showCharacterWizard = ref(false);

async function toggleEdit(){
  await characterKnowledgeData.getSelectableCharacterPowers(route.params.id);
  showEdit.value = !showEdit.value;
}

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
    <div class="text-right pb-3" v-if="experienceInfo.showAllExperience && !showCharacterWizard">{{ experienceInfo.experienceBreakdown.powersXp}} Total XP - {{experienceInfo.experienceBreakdown.setupPowersXp}} Creation XP = {{experienceInfo.experienceBreakdown.powersXp - experienceInfo.experienceBreakdown.setupPowersXp}} XP</div>
    <div v-if="!showCharacterWizard && (!noPowers || characterKnowledgeData.powers.length > 0)" class="d-flex flex-row justify-content-between mb-2">
      <SplitButton class="pr-3" label="Download Power Cards" @click="characterKnowledgeData.downloadPowerCards(route.params.id, 'foo', true)" :model="items" />
      <Button v-if="!showEdit" class="btn btn-primary" label="Edit" @click="toggleEdit" />
      <Button v-else class="btn btn-primary" label="Cancel" @click="toggleEdit" />
    </div>
    <div v-for="path in characterKnowledgeData.powers">
      <h1>{{path.name}}</h1>
      <PowerCard :power-path="path" :show-edit="showEdit"/>
    </div>
    
    <div v-if="showEdit && !showCharacterWizard" class="mb-2">
      <hr v-if="characterKnowledgeData.powers.length !== 0">
      <h1>Choose Powers</h1>
      <div v-if="characterKnowledgeData.powers.length === 0">
        <p>No Powers detected, please pick one below.</p>
      </div>
      <div v-for="path in characterKnowledgeData.selectablePowers">
        <h1>{{path.name}}</h1>
        <div v-for="power in path.powers" :key="power.id">
          <PickPowerCard :power="power" :show-pick-button="true"/>
        </div>
      </div>
    </div>
  </div>
</template>
