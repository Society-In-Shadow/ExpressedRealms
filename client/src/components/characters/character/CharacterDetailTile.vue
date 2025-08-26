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
  <Card v-if="!showEdit" class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch">
    <template #content>
      <div class="d-flex flex-row justify-content-between" >
        <div>
          <h1 class="mt-0 pt-0 pb-0 mb-0">{{ name }}</h1>
          <div><em><span>XL: {{experienceInfo.getCharacterLevel()}}</span> - {{ expression }}</em></div>
          <div><em>{{ faction?.name ?? 'No Faction' }}</em></div>
        </div>
        <div class="d-flex flex-column gap-3" style="font-size: 2.5em">
          <Button type="button" @click="togglePopup" size="large" :label="`XP: ${experienceInfo.experienceBreakdown.total - experienceInfo.experienceBreakdown.setupTotal}`" />
          <Button class="float-end" label="Edit" @click="toggleEdit" />
        </div>
      </div>
    </template>
  </Card>
  <EditCharacterDetails v-else @close-dialog="toggleEdit" />
  <Popover ref="op" >
    <OverallExperience/>
  </Popover>
</template>
