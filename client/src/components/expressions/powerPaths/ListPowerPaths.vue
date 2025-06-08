<script setup lang="ts">

import {powerPathStore} from "@/components/expressions/powerPaths/powerPathStore";
import {onBeforeMount, ref} from "vue";
import Accordion from 'primevue/accordion';
import AccordionPanel from 'primevue/accordionpanel';
import AccordionHeader from 'primevue/accordionheader';
import AccordionContent from 'primevue/accordioncontent';
import Button from 'primevue/button';
import ListPowers from "@/components/expressions/powers/ListPowers.vue";
import AddPowerPath from "@/components/expressions/powerPaths/AddPowerPath.vue";
import Divider from 'primevue/divider';
import EditPowerPath from "@/components/expressions/powerPaths/EditPowerPath.vue";

var powerPaths = powerPathStore();

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

const showEdit = ref(false);
const toggleEdit = () => {
  showEdit.value = !showEdit.value;
}

</script>

<template>
  
  <div v-for="path in powerPaths.powerPaths" :key="path.id">
    <Divider></Divider>
    <div v-if="!showEdit">
      <div class="flex">
        <div class="col-flex flex-grow-1 align-self-center">
          <h1 class="m-0">
            {{path.name}}
          </h1>
        </div>
        <div class="col-flex align-self-center">
          <Button label="Edit" class="float-end m-2" @click="toggleEdit()" />
        </div>
      </div>
      <div class="mb-0 pb-0" v-html="path.description"></div>
    </div>
    <div v-else>
      <EditPowerPath :power-path-id="path.id" :expression-id="props.expressionId" @cancelled="toggleEdit()"></EditPowerPath>
    </div>
   
    <Divider></Divider>
    <h2>Powers</h2>
    <ListPowers :power-path-id="path.id"></ListPowers>
  </div>
  <AddPowerPath :expression-id="props.expressionId" @canceled="toggleAddPower" />

</template>