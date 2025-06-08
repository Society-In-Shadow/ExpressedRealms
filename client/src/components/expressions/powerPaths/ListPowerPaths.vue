<script setup lang="ts">

import {powerPathStore} from "@/components/expressions/powerPaths/powerPathStore";
import {onBeforeMount} from "vue";
import Accordion from 'primevue/accordion';
import AccordionPanel from 'primevue/accordionpanel';
import AccordionHeader from 'primevue/accordionheader';
import AccordionContent from 'primevue/accordioncontent';
import Button from 'primevue/button';
import ListPowers from "@/components/expressions/powers/ListPowers.vue";

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

</script>

<template>
  <div class="card">
    <div v-if="powerPaths.powerPaths.length == 0">
      <Button label="Add Power Path"></Button>
    </div>
    <Accordion v-else :value="['0']" multiple>
      <AccordionPanel v-for="path in powerPaths.powerPaths" :key="path.id" :value="path.id">
        <AccordionHeader>{{path.name}}</AccordionHeader>
        <AccordionContent>
          {{path.description}}
          <ListPowers :expression-id="path.id"></ListPowers>
        </AccordionContent>
      </AccordionPanel>
    </Accordion>
  </div>

</template>