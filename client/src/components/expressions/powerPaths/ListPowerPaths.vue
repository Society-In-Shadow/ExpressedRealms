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

</script>

<template>
  <div class="card">
    <div v-if="powerPaths.powerPaths.length == 0">
      <Button label="Add Power Path" @click="toggleAddPower"></Button>
    </div>
    <AddPowerPath v-if="showAddPower" :expression-id="props.expressionId" @canceled="toggleAddPower" />
    <Accordion v-else :value="['0']" multiple>
      <AccordionPanel v-for="path in powerPaths.powerPaths" :key="path.id" :value="path.id">
        <AccordionHeader>{{path.name}}</AccordionHeader>
        <AccordionContent>
          <div v-html="path.description"></div>
          <Divider/>
          <ListPowers :power-path-id="path.id"></ListPowers>
        </AccordionContent>
      </AccordionPanel>
    </Accordion>
  </div>

</template>