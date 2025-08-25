<script setup lang="ts">

import Card from "primevue/card";
import Popover from "primevue/popover";
import {computed, onMounted, ref} from "vue";
import {useRoute} from 'vue-router'
import {characterStore} from "@/components/characters/character/stores/characterStore";
import Button from "primevue/button";
import EditCharacterDetails from "@/components/characters/character/EditCharacterDetails.vue";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import OverallExperience from "@/components/characters/character/OverallExperience.vue";

const route = useRoute()
const characterInfo = characterStore();
const experienceInfo = experienceStore();
onMounted(async () =>{
  await characterInfo.getCharacterDetails(Number(route.params.id))
      .then(() => {
        name.value = characterInfo.name;
        expression.value = characterInfo.expression;
        faction.value = characterInfo.faction;
      });
  
  await experienceInfo.updateExperience(route.params.id);
});

const name = ref("");
const faction = ref("");
const expression = ref("");
const showEdit = ref(false);

function toggleEdit() {
  showEdit.value = !showEdit.value;
}


const op = ref();

const togglePopup = (event) => {
  op.value.toggle(event);
}
const xpTitle = computed(() => {
  return `(XP - Setup Costs (${experienceInfo.experienceBreakdown.setupTotal})) = ${experienceInfo.experienceBreakdown.total - experienceInfo.experienceBreakdown.setupTotal})`;
});

</script>

<template>
  <Card v-if="!showEdit" class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" style="max-width: 30em">
    <template #content>
      <h1 class="mt-0 pt-0 pb-0">
        {{ name }} 
      </h1>
      <div class="d-flex flex-row justify-content-between">
        <div><em>XL: {{ experienceInfo.getCharacterLevel() }}</em></div>
        <div v-if="experienceInfo.showAllExperience" :title="xpTitle">XP: {{experienceInfo.experienceBreakdown.total - experienceInfo.experienceBreakdown.setupTotal}}<span class="material-symbols-outlined" style="" @click="togglePopup">help</span></div>
      </div>
      <div>{{ expression }}</div>
      <div>{{ faction?.name ?? 'No Faction' }}</div>
      <Button class="float-end" label="Edit" @click="toggleEdit" />
    </template>
  </Card>
  <EditCharacterDetails v-else @close-dialog="toggleEdit" />
  <Popover ref="op" >
    <OverallExperience/>
  </Popover>
</template>
