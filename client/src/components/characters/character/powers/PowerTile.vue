<script setup lang="ts">

import Button from "primevue/button";
import {onBeforeMount, ref} from "vue";
import {useRoute} from "vue-router";
import {characterPowersStore} from "@/components/characters/character/powers/stores/characterPowerStore.ts";
import PowerCard from "@/components/characters/character/powers/PowerCard.vue";
import PickPowerCard from "@/components/characters/character/powers/PickPowerCard.vue";

const characterKnowledgeData = characterPowersStore();
const route = useRoute();

onBeforeMount(async () => {
  await characterKnowledgeData.getCharacterPowers(route.params.id)
  if(characterKnowledgeData.powers.length === 0)
  {
    noPowers.value = true;
    await toggleEdit();
  }
})

const showEdit = ref(false);
const noPowers = ref(false);

async function toggleEdit(){
  await characterKnowledgeData.getSelectableCharacterPowers(route.params.id);
  showEdit.value = !showEdit.value;
}

</script>

<template>
  <div style="max-width: 650px; margin: 0 auto;">
    <div v-if="!noPowers || characterKnowledgeData.powers.length > 0" class="text-right mb-2">
      <Button v-if="!showEdit" class="btn btn-primary" label="Edit" @click="toggleEdit" />
      <Button v-else class="btn btn-primary" label="Cancel" @click="toggleEdit" />
    </div>

    <div v-for="path in characterKnowledgeData.powers">
      <h1>{{path.name}}</h1>
      <PowerCard :power-path="path"  :power-path-id="path.id"/>
    </div>
    
    <div v-if="showEdit" class="mb-2">
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
