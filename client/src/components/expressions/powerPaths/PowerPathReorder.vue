<script setup lang="ts">

import {powerPathStore} from "@/components/expressions/powerPaths/stores/powerPathStore";
import {UserRoles, userStore} from "@/stores/userStore";
let userInfo = userStore();
let powerPaths = powerPathStore();
import { DropList, Drag } from "vue-easy-dnd";
import Button from 'primevue/button';
import {ref, toRaw} from "vue";
import axios from "axios";
import type {PowerPath} from "@/components/expressions/powerPaths/types";
import {getSortAndIdsForPowerPaths} from "@/components/expressions/powerPaths/utilities/powerPathUtilities";
import {expressionStore} from "@/stores/expressionStore";
import toaster from "@/services/Toasters";

const expressionInfo = expressionStore();
const emit = defineEmits<{
  togglePreview: []
}>();

let originalModel:PowerPath[];

function saveChanges(){

  axios.put(`/expression/${expressionInfo.currentExpressionId}/updateSorting`, {
    expressionId: expressionInfo.currentExpressionId,
    items: getSortAndIdsForPowerPaths(powerPaths.powerPaths)
  }).then(() => {
    emit("togglePreview");
    showPowerPathReorder.value = !showPowerPathReorder.value;
    toaster.success("Successfully Updated Power Sorting!");
  });
}

const showPowerPathReorder = ref(false);
function toggleEdit(){
  if(!showPowerPathReorder.value)
    originalModel = JSON.parse(JSON.stringify(toRaw(powerPaths.powerPaths)));

  if(showPowerPathReorder.value)
    powerPaths.powerPaths = originalModel;

  emit("togglePreview");
  showPowerPathReorder.value = !showPowerPathReorder.value;
}

</script>

<template>
  <div class="row">
    <Button
      v-if="userInfo.hasUserRole(UserRoles.PowerManagementRole)" class="col m-2"
      :label="showPowerPathReorder ? 'Cancel' : 'Reorder Power Paths'" @click="toggleEdit"
    />
    <Button v-if="showPowerPathReorder" label="Save" class="col m-2" @click="saveChanges" />
  </div>

  <drop-list v-if="showPowerPathReorder" :items="powerPaths.powerPaths" @reorder="$event.apply(powerPaths.powerPaths)">
    <template #item="{item}">
      <drag :key="item.id" :data="item">
        <h1><i class="pi pi-bars mr-2" />{{ item.name }}</h1>
      </drag>
    </template>
    <template #feedback="{data}">
      <h1><i class="pi pi-bars mr-2" />{{ data.name }}</h1>
    </template>
  </drop-list>
</template>
