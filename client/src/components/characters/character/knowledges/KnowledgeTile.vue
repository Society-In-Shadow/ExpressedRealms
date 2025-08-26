<script setup lang="ts">

import Button from "primevue/button";
import {computed, onBeforeMount, ref} from "vue";
import {knowledgeStore} from "@/components/knowledges/stores/knowledgeStore";
import SelectKnowledgeItem from "@/components/characters/character/knowledges/SelectKnowledgeItem.vue";
import {characterKnowledgeStore} from "@/components/characters/character/knowledges/stores/characterKnowledgeStore";
import {useRoute} from "vue-router";
import Tag from "primevue/tag";
import AccordionPanel from "primevue/accordionpanel";
import Accordion from "primevue/accordion";
import AccordionContent from "primevue/accordioncontent";
import AccordionHeader from "primevue/accordionheader";
import {addKnowledgeDialog} from "@/components/characters/character/knowledges/services/dialogs";
import {confirmationPopup} from "@/components/characters/character/knowledges/services/confirmationService";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";

const knowledgeData = knowledgeStore();
const characterKnowledgeData = characterKnowledgeStore();
const route = useRoute();
const dialogService = addKnowledgeDialog();
const popupService = confirmationPopup(route.params.id)
const experienceInfo = experienceStore();

onBeforeMount(async () => {
  await characterKnowledgeData.getCharacterKnowledges(route.params.id)
  if(characterKnowledgeData.knowledges.length === 0)
  {
    noKnowledges.value = true;
    await toggleEdit();
  }
})

const showEdit = ref(false);
const noKnowledges = ref(false);
const openKnowledgeItems = ref([]);

async function toggleEdit(){
  await knowledgeData.getKnowledges();
  showEdit.value = !showEdit.value;
}

const filteredKnowledges = computed(() => {
  const existingKnowledgeIds = characterKnowledgeData.knowledges.map(ck => ck.knowledge.id);
  return knowledgeData.knowledges.filter(knowledge =>
      !existingKnowledgeIds.includes(knowledge.id)
  );

})

</script>

<template>
  <div style="max-width: 650px; margin: 0 auto;">
    <div class="text-right pb-3" v-if="experienceInfo.showAllExperience">{{ experienceInfo.experienceBreakdown.knowledgeXp}} Total XP - {{experienceInfo.experienceBreakdown.setupKnowledgeXp}} Creation XP = {{experienceInfo.experienceBreakdown.knowledgeXp - experienceInfo.experienceBreakdown.setupKnowledgeXp}} XP</div>
    <div v-if="!noKnowledges || characterKnowledgeData.knowledges.length > 0" class="text-right mb-2">
      <Button v-if="!showEdit" class="btn btn-primary" label="Edit" @click="toggleEdit" />
      <Button v-else class="btn btn-primary" label="Cancel" @click="toggleEdit" />
    </div>

    <Accordion :value="openKnowledgeItems" multiple :lazy="true" expand-icon="pi pi-info-circle" collapse-icon="pi pi-times-circle">
      <AccordionPanel v-for="knowledge in characterKnowledgeData.knowledges" :key="knowledge.name" :value="knowledge.mappingId">
        <AccordionHeader>
          <div class="d-flex flex-column flex-grow-1 pr-3">
            <div class="d-flex flex-fill align-content-between d-block">
              <div class="flex-grow-1 font-bold text-900">
                {{ knowledge.knowledge.name }} - <em>{{ knowledge.knowledge.type }}</em>
              </div>
              <div>
                {{ knowledge.levelName }} ({{ knowledge.level }})
              </div>
            </div>
            <div class="d-flex d-block mt-1">
              <div class="flex-grow-1">
                <Tag v-if="knowledge.specializations.length == 0" value="No Specializations" />
                <Tag v-for="special in knowledge.specializations" v-else class="mr-1" :value="special.name" />
              </div>
              <div>Stones: +{{ knowledge.stoneModifier }}</div>
            </div>
          </div>
        </AccordionHeader>
        <AccordionContent>
          <p class="m-0">
            {{ knowledge.knowledge.description }}
          </p>
        
          <h3 v-if="knowledge.notes" class="mt-3">
            Notes
          </h3>
          <p v-if="knowledge.notes">
            {{ knowledge.notes }}
          </p>

          <div v-if="showEdit" class="mt-3 d-flex justify-content-between">
            <Button class="btn btn-primary" label="Delete" severity="danger" @click="popupService.deleteConfirmation($event, knowledge.mappingId)" />
            <Button label="Edit" @click="dialogService.showEditCharacter(knowledge)" />
          </div>
          <hr v-if="knowledge.specializations.length > 0" class="mt-2 mb-2">
          <h1 v-if="knowledge.specializations.length > 0" class="mt-3">
            Specializations
          </h1>
          <div v-if="knowledge.specializations.length > 0">
            <div v-for="special in knowledge.specializations" :key="special.id">
              <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
                <div>
                  <h2 class="m-0 p-0">
                    {{ special.name }}
                  </h2>
                </div>
              </div>

              <p>{{ special.description }}</p>
              <h4 v-if="special.notes">
                Notes
              </h4>
              <p v-if="special.notes">
                {{ special.notes }}
              </p>

              <div class="p-0 m-0 d-flex justify-content-between">
                <Button v-if="showEdit" label="Delete" severity="danger" @click="popupService.deleteSpecializationConfirmation($event, knowledge.mappingId, special.id)" />
                <Button v-if="showEdit" label="Edit" @click="dialogService.showEditSpecialization(knowledge, special)" />
              </div>
            </div>
          </div>
          <div v-if="showEdit" class="text-right mt-2">
            <Button v-if="knowledge.specializationCount > knowledge.specializations.length" class="btn btn-primary text-right" label="Add Specialization" @click="dialogService.showAddSpecialization(knowledge)" />
          </div>
        </AccordionContent>
      </AccordionPanel>
    </Accordion>
    
    <div v-if="showEdit" class="mb-2">
      <hr v-if="characterKnowledgeData.knowledges.length !== 0">
      <h1>Choose Knowledges</h1>
      <div v-if="characterKnowledgeData.knowledges.length === 0">
        <p>No Knowledges detected, please pick one below.</p>
      </div>
      <div v-for="knowledge in filteredKnowledges" :key="knowledge.id">
        <SelectKnowledgeItem :is-read-only="false" :knowledge="knowledge" />
      </div>
    </div>
  </div>
</template>
