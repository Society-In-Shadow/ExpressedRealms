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

</script>

<template>
  <div v-for="path in powerPaths.powerPaths" :key="path.id">
    <Divider />
    <ShowPowerPath :path="path" :expression-id="props.expressionId" />
   
    <Divider />
    <h2>Powers</h2>
    <ListPowers :power-path-id="path.id" :powers="path.powers" />
  </div>

  <Button
    v-if="!showAddPower && userInfo.hasUserRole(UserRoles.PowerManagementRole)" class="w-100 m-2"
    label="Add Power Path" @click="toggleAddPower"
  />
  <AddPowerPath
    v-if="showAddPower && userInfo.hasUserRole(UserRoles.PowerManagementRole)"
    :expression-id="props.expressionId" @canceled="toggleAddPower"
  />
</template>
