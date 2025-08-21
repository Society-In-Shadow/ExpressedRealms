<script setup lang="ts">

import Card from "primevue/card";
import {type PropType, ref} from "vue";
import type {Power} from "@/components/expressions/powers/types";
import EditPower from "@/components/expressions/powers/EditPower.vue";
import {powerConfirmationPopups} from "@/components/expressions/powers/services/powerConfirmationPopupService";
import {UserRoles, userStore} from "@/stores/userStore";
import {isNullOrWhiteSpace, makeIdSafe} from "@/utilities/stringUtilities";
import {scrollToSection} from "@/components/expressions/expressionUtilities";

let userInfo = userStore();
const props = defineProps({
  power: {
    type: Object as PropType<Power>,
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

const popups = powerConfirmationPopups(props.power.id, props.power.name, props.powerPathId);

const showEdit = ref(false);

const toggleEdit = () =>{
  showEdit.value = !showEdit.value;
}

</script>

<template>
  <EditPower
    v-if="showEdit && userInfo.hasUserRole(UserRoles.PowerManagementRole) && !props.isReadOnly" :power-id="props.power.id"
    :power-path-id="props.powerPathId" @canceled="toggleEdit"
  />
  <Card v-else :id="makeIdSafe(props.power.name)" class="card-body-fix">
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
      <div class="pt-0 mt-0" v-html="props.power.description" />
    </template>
    <template #content>

      <h2>Game Mechanic Effect</h2>
      <div v-html="props.power.gameMechanicEffect" />

      <h2 v-if="!isNullOrWhiteSpace(props.power.limitation)">
        Limitations
      </h2>
      <div v-if="!isNullOrWhiteSpace(props.power.limitation)" v-html="props.power.limitation" />




      <h2 v-if="props.power.prerequisites">
        Prerequisites
      </h2>
      <div v-if="props.power.prerequisites">
        <div v-if="props.power.prerequisites.powers.length == 1">
          <a :href="'#' + makeIdSafe(props.power.prerequisites.powers[0])" @click.prevent="scrollToSection(props.power.prerequisites.powers[0])">{{ props.power.prerequisites.powers[0] }}</a>
        </div>
        <div v-else-if="props.power.prerequisites.powers.length == props.power.prerequisites.requiredAmount">
          All of the following powers :
          <span v-for="(power, index) in props.power.prerequisites.powers">
            <a :href="'#' + makeIdSafe(power)" @click.prevent="scrollToSection(power)">{{ power }}</a> 
            <span v-if="index != props.power.prerequisites.powers.length -1"> and </span>
          </span>
        </div>
        <div v-else>
          Any of
          <span v-if="props.power.prerequisites.requiredAmount != 1">{{ props.power.prerequisites.requiredAmount }}</span>
          the following powers :
          <span v-for="(power, index) in props.power.prerequisites.powers">
            <a :href="'#' + makeIdSafe(power)" @click.prevent="scrollToSection(power)">{{ power }}</a>
            <span v-if="index != props.power.prerequisites.powers.length -1"> or </span>
          </span>
        </div>
      </div>

      <h2 v-if="!isNullOrWhiteSpace(props.power.other)">
        Additional Information
      </h2>
      <div v-if="!isNullOrWhiteSpace(props.power.other)" v-html="props.power.other" />
      
      <h2>Power Use And Cost</h2>
      <p>{{ props.power.isPowerUse ? "Yes" : "No" }} -                 
        <span v-if="!isNullOrWhiteSpace(props.power.cost)">{{ props.power.cost }}</span>
        <span v-else>N/A</span>
      </p>

    </template>
  </Card>
</template>

<style>
@media(max-width: 768px){
  .card-body-fix .p-card-body{
    padding: 0 !important;
  }
}
</style>
