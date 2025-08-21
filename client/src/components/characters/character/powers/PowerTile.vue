<script setup lang="ts">

import Button from "primevue/button";
import {onBeforeMount, ref} from "vue";
import {knowledgeStore} from "@/components/knowledges/stores/knowledgeStore";
import {characterKnowledgeStore} from "@/components/characters/character/knowledges/stores/characterKnowledgeStore";
import {useRoute} from "vue-router";
import Tag from "primevue/tag";
import AccordionPanel from "primevue/accordionpanel";
import Accordion from "primevue/accordion";
import AccordionContent from "primevue/accordioncontent";
import AccordionHeader from "primevue/accordionheader";
import {addKnowledgeDialog} from "@/components/characters/character/knowledges/services/dialogs";
import {confirmationPopup} from "@/components/characters/character/knowledges/services/confirmationService";
import {characterPowersStore} from "@/components/characters/character/powers/stores/characterPowerStore.ts";
import PowerCard from "@/components/characters/character/powers/PowerCard.vue";

const characterKnowledgeData = characterPowersStore();
const route = useRoute();
const dialogService = addKnowledgeDialog();
const popupService = confirmationPopup(route.params.id)

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
const openKnowledgeItems = ref([]);

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
      
      <Accordion :value="openKnowledgeItems" multiple :lazy="true" expand-icon="pi pi-info-circle" collapse-icon="pi pi-times-circle">
        <AccordionPanel v-for="power in path.powers" :key="power.id" :value="power.id">
          <AccordionHeader>
            <div class="d-flex flex-column flex-grow-1 pr-3">
              <div class="d-flex flex-fill align-content-between d-block">
                <div class="flex-grow-1 font-bold text-900">
                  {{ power.name }} - <em>{{ power.powerLevel.name }}</em>
                </div>
                <div>
                  {{ power.areaOfEffect.name }} ({{ power.powerActivationType.name }})
                </div>
              </div>
              <div class="d-flex d-block mt-1">
                <div class="flex-grow-1">
                  <Tag v-if="power.category.length == 0" value="No Specializations" />
                  <Tag v-for="special in power.category" v-else class="mr-1" :value="special.name" />
                </div>
                <div>{{power.powerDuration.name}}</div>
              </div>
            </div>
          </AccordionHeader>
          <AccordionContent>
            <PowerCard :power="power"  :power-path-id="path.id"/>
<!--            <div v-if="showEdit" class="text-right mt-2">
              <Button v-if="power.specializationCount > power.specializations.length" class="btn btn-primary text-right" label="Add Specialization" @click="dialogService.showAddSpecialization(power)" />
            </div>-->
          </AccordionContent>
        </AccordionPanel>
      </Accordion>

    </div>
    

    <div v-if="showEdit" class="mb-2">
      <hr v-if="characterKnowledgeData.powers.length !== 0">
      <h1>Choose Powers</h1>
      <div v-if="characterKnowledgeData.powers.length === 0">
        <p>No Powers detected, please pick one below.</p>
      </div>
      <div v-for="path in characterKnowledgeData.selectablePowers">
        <h1>{{path.name}}</h1>

        <Accordion :value="openKnowledgeItems" multiple :lazy="true" expand-icon="pi pi-info-circle" collapse-icon="pi pi-times-circle">
          <AccordionPanel v-for="power in path.powers" :key="power.id" :value="power.id">
            <AccordionHeader>
              <div class="d-flex flex-column flex-grow-1 pr-3">
                <div class="d-flex flex-fill align-content-between d-block">
                  <div class="flex-grow-1 font-bold text-900">
                    {{ power.name }} - <em>{{ power.powerLevel.name }}</em>
                  </div>
                  <div>
                    {{ power.areaOfEffect.name }} ({{ power.powerActivationType.name }})
                  </div>
                </div>
                <div class="d-flex d-block mt-1">
                  <div class="flex-grow-1">
                    <Tag v-if="power.category.length == 0" value="No Specializations" />
                    <Tag v-for="special in power.category" v-else class="mr-1" :value="special.name" />
                  </div>
                  <div>{{power.powerDuration.name}}</div>
                </div>
              </div>
            </AccordionHeader>
            <AccordionContent>
              <PowerCard :power="power"  :power-path-id="path.id"/>
              <!--            <div v-if="showEdit" class="text-right mt-2">
                            <Button v-if="power.specializationCount > power.specializations.length" class="btn btn-primary text-right" label="Add Specialization" @click="dialogService.showAddSpecialization(power)" />
                          </div>-->
            </AccordionContent>
          </AccordionPanel>
        </Accordion>

      </div>
    </div>
  </div>
</template>
