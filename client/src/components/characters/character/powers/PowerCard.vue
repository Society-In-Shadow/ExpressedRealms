<script setup lang="ts">

import Card from "primevue/card";
import {type PropType, ref} from "vue";
import EditPower from "@/components/expressions/powers/EditPower.vue";
import {powerConfirmationPopups} from "@/components/expressions/powers/services/powerConfirmationPopupService";
import {UserRoles, userStore} from "@/stores/userStore";
import {isNullOrWhiteSpace, makeIdSafe} from "@/utilities/stringUtilities";
import {scrollToSection} from "@/components/expressions/expressionUtilities";
import AccordionHeader from "primevue/accordionheader";
import Accordion from "primevue/accordion";
import AccordionPanel from "primevue/accordionpanel";
import AccordionContent from "primevue/accordioncontent";
import Tag from "primevue/tag";
import type {PowerPath} from "@/components/characters/character/powers/types.ts";

let userInfo = userStore();
const props = defineProps({
  powerPath: {
    type: Object as PropType<PowerPath>,
    required: true,
  },
  powerPathId:{
    type: Number,
    required: true
  },
  isReadOnly:{
    type: Boolean,
    required: false
  }
});

const popups = powerConfirmationPopups(props.powerPath.id, props.powerPath.name, props.powerPathId);
const openKnowledgeItems = ref([]);
const showEdit = ref(false);

const toggleEdit = () =>{
  showEdit.value = !showEdit.value;
}

</script>

<template>
  <EditPower
    v-if="showEdit && userInfo.hasUserRole(UserRoles.PowerManagementRole) && !props.isReadOnly" :power-id="props.powerPath.id"
    :power-path-id="props.powerPathId" @canceled="toggleEdit"
  />

  <Accordion :value="openKnowledgeItems" multiple expand-icon="pi pi-info-circle" collapse-icon="pi pi-times-circle">
    <AccordionPanel v-for="power in props.powerPath.powers" :key="power.id" :value="power.id">
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
          <div class="d-flex d-block mt-1">
            <div class="flex-grow-1">Is Power Use: {{ power.isPowerUse ? "Yes" : "No" }}
            </div>
            <div>                
              <span v-if="!isNullOrWhiteSpace(power.cost)">{{ power.cost }}</span>
              <span v-else>N/A</span>
            </div>
          </div>
        </div>
      </AccordionHeader>
      <AccordionContent>
        <Card :id="makeIdSafe(power.name)" class="card-body-fix">
          <!-- <template #title>
             <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
               <div
                 v-if="!showEdit && userInfo.hasUserRole(UserRoles.PowerManagementRole) && !props.isReadOnly"
                 class="p-0 m-0 d-inline-flex align-items-start"
               >
                 <Button class="mr-2" severity="danger" label="Delete" @click="popups.deleteConfirmation($event)" />
                 <Button class="float-end" label="Edit" @click="toggleEdit" />
               </div>
             </div>
           </template>-->
          <template #subtitle>
            <div class="pt-0 mt-0" v-html="power.description" />
          </template>
          <template #content>
      
            <h2>Game Mechanic Effect</h2>
            <div v-html="power.gameMechanicEffect" />
      
            <h2 v-if="!isNullOrWhiteSpace(power.limitation)">
              Limitations
            </h2>
            <div v-if="!isNullOrWhiteSpace(power.limitation)" v-html="power.limitation" />
      
      
      
      
            <h2 v-if="power.prerequisites">
              Prerequisites
            </h2>
            <div v-if="power.prerequisites">
              <div v-if="power.prerequisites.powers.length == 1">
                <a :href="'#' + makeIdSafe(power.prerequisites.powers[0])" @click.prevent="scrollToSection(power.prerequisites.powers[0])">{{
                    power.prerequisites.powers[0]
                  }}</a>
              </div>
              <div v-else-if="power.prerequisites.powers.length == power.prerequisites.requiredAmount">
                All of the following powers :
                <span v-for="(power, index) in power.prerequisites.powers">
                  <a :href="'#' + makeIdSafe(power)" @click.prevent="scrollToSection(power)">{{ power }}</a> 
                  <span v-if="index != power.prerequisites.powers.length -1"> and </span>
                </span>
              </div>
              <div v-else>
                Any of
                <span v-if="power.prerequisites.requiredAmount != 1">{{
                    power.prerequisites.requiredAmount
                  }}</span>
                the following powers :
                <span v-for="(power, index) in power.prerequisites.powers">
                  <a :href="'#' + makeIdSafe(power)" @click.prevent="scrollToSection(power)">{{ power }}</a>
                  <span v-if="index != power.prerequisites.powers.length -1"> or </span>
                </span>
              </div>
            </div>
      
            <h2 v-if="!isNullOrWhiteSpace(power.other)">
              Additional Information
            </h2>
            <div v-if="!isNullOrWhiteSpace(power.other)" v-html="power.other" />
            

      
          </template>
        </Card>
      </AccordionContent>
    </AccordionPanel>
  </Accordion>
</template>

<style>
@media(max-width: 768px){
  .card-body-fix .p-card-body{
    padding: 0 !important;
  }
}
</style>
