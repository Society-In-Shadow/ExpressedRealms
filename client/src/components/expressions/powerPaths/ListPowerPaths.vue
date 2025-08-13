<script setup lang="ts">

import {powerPathStore} from "@/components/expressions/powerPaths/stores/powerPathStore";
import {onBeforeMount, ref} from "vue";
import Button from 'primevue/button';
import ListPowers from "@/components/expressions/powers/ListPowers.vue";
import AddPowerPath from "@/components/expressions/powerPaths/AddPowerPath.vue";
import Divider from 'primevue/divider';
import ShowPowerPath from "@/components/expressions/powerPaths/ShowPowerPath.vue";
import {UserRoles, userStore} from "@/stores/userStore";
let userInfo = userStore();
let powerPaths = powerPathStore();
import PowerPathReorder from "@/components/expressions/powerPaths/PowerPathReorder.vue";
import axios from "axios";
import {expressionStore} from "@/stores/expressionStore.ts";
import {useRoute} from "vue-router";
import SplitButton from "primevue/splitbutton";

let expressionInfo = expressionStore();
let route = useRoute();

const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  }
});

onBeforeMount(async () => {
  await powerPaths.getPowerPaths(props.expressionId);
})

const showAddPower = ref(false);

const toggleAddPower = () => {
  showAddPower.value = !showAddPower.value;
}

const readOnly = ref(false);
const toggleReadOnly = () => {
  readOnly.value = !readOnly.value;
}

async function downloadPowerCards(isFiveByThree: boolean = false) {
  const res = await axios.get(`/expression/${expressionInfo.currentExpressionId}/getPowerCards`, {
    responseType: 'blob',
    params: {
      isFiveByThree: isFiveByThree,
    }
  });
  var expression = route.params.name
  const url = URL.createObjectURL(res.data);
  const a = document.createElement('a');
  a.href = url;
  a.download = `${expression}PowerCards.pdf`;
  document.body.appendChild(a);
  a.click();
  a.remove();
  URL.revokeObjectURL(url);
}

const items = [
  {
    label: '5x3 Index Card Size',
    command: () => {
      downloadPowerCards(true);
    }
  }
];

</script>

<template>
  <div class="d-flex flex-row justify-content-end align-items-center">
    <SplitButton label="Download Power Cards" @click="downloadPowerCards(false)" :model="items" />
  </div>
  
  <PowerPathReorder v-if="userInfo.hasUserRole(UserRoles.PowerManagementRole)" @toggle-preview="toggleReadOnly" />
  <div v-for="path in powerPaths.powerPaths" :key="path.id">
    <Divider />
    <ShowPowerPath :path="path" :expression-id="props.expressionId" :is-read-only="readOnly" />
   
    <Divider />
    <h2>Powers</h2>
    <ListPowers :power-path-id="path.id" :powers="path.powers" :is-read-only="readOnly" />
  </div>

  <Button
    v-if="!showAddPower && userInfo.hasUserRole(UserRoles.PowerManagementRole) && !readOnly" class="w-100 m-2"
    label="Add Power Path" @click="toggleAddPower"
  />
  <AddPowerPath
    v-if="showAddPower && userInfo.hasUserRole(UserRoles.PowerManagementRole) && !readOnly"
    :expression-id="props.expressionId" @canceled="toggleAddPower"
  />
</template>
