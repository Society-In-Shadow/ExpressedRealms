<script setup lang="ts">

import Card from "primevue/card";
import Button from "primevue/button";
import {type PropType, ref} from "vue";
import type {Power} from "@/components/expressions/powers/types";
import EditPower from "@/components/expressions/powers/EditPower.vue";
import {powerConfirmationPopups} from "@/components/expressions/powers/services/powerConfirmationPopupService";
import {UserRoles, userStore} from "@/stores/userStore";
import {isNullOrWhiteSpace, makeIdSafe} from "@/utilities/stringUtilities";

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
    <template #title>
      <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
        <div>
          <h1 class="p-0 m-0">
            {{ props.power.name }}
          </h1>
          <div class="p-0 m-0">
            {{ props.power.powerLevel.name }}
          </div>
        </div>
        <div
          v-if="!showEdit && userInfo.hasUserRole(UserRoles.PowerManagementRole) && !props.isReadOnly"
          class="p-0 m-0 d-inline-flex align-items-start"
        >
          <Button class="mr-2" severity="danger" label="Delete" @click="popups.deleteConfirmation($event)" />
          <Button class="float-end" label="Edit" @click="toggleEdit" />
        </div>
      </div>
    </template>
    <template #subtitle>
      <div v-html="props.power.description" />
    </template>
    <template #content>
      <div style="overflow: auto">
        <table class="p-datatable-table">
          <!-- Table header -->
          <thead class="p-datatable-thead">
            <tr>
              <th class="p-datatable-header-cell">
                Category
              </th>
              <th class="p-datatable-header-cell">
                Power Duration
              </th>
              <th class="p-datatable-header-cell">
                Area of Effect
              </th>
            </tr>
          </thead>
          <tbody class="p-datatable-tbody">
            <tr class="p-row-even">
              <td>
                <p v-for="category in props.power.category" v-if="props.power.category && props.power.category.length > 0" :key="category.id" class="pr-3">
                  {{ category.name }}
                </p>
                <p v-else>
                  N/A
                </p>
              </td>
              <td :title="props.power.powerDuration.description">
                {{ props.power.powerDuration.name }}
              </td>
              <td :title="props.power.areaOfEffect.description">
                {{ props.power.areaOfEffect.name }}
              </td>
            </tr>
            <tr>
              <td class="p-datatable-header-cell">
                Activation Type
              </td>
              <td class="p-datatable-header-cell">
                Power Used?
              </td>
              <td class="p-datatable-header-cell">
                Cost
              </td>
            </tr>
            <tr class="p-row-even">
              <td :title="props.power.powerActivationType.description">
                {{ props.power.powerActivationType.name }}
              </td>
              <td>{{ props.power.isPowerUse ? "Yes" : "No" }}</td>
              <td>
                <span v-if="!isNullOrWhiteSpace(props.power.cost)">{{ props.power.cost }}</span>
                <span v-else>N/A</span>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      
      <h2>Game Mechanic Effect</h2>
      <div v-html="props.power.gameMechanicEffect" />

      <h2 v-if="!isNullOrWhiteSpace(props.power.limitation)">
        Limitations
      </h2>
      <div v-if="!isNullOrWhiteSpace(props.power.limitation)" v-html="props.power.limitation" />

      <h2 v-if="!isNullOrWhiteSpace(props.power.other)">
        Additional Information
      </h2>
      <div v-if="!isNullOrWhiteSpace(props.power.other)" v-html="props.power.other" />
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
