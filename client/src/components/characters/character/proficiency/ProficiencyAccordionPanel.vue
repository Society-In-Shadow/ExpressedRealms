<script setup lang="ts">

import AccordionContent from 'primevue/accordioncontent'
import AccordionHeader from 'primevue/accordionheader'
import AccordionPanel from 'primevue/accordionpanel'

const props = defineProps({
  proficiency: {
    type: Object,
    required: true,
  },
})

</script>

<template>
  <AccordionPanel v-bind="$attrs">
    <AccordionHeader>
      <div class="d-flex justify-content-between w-100 pr-3">
        <div>{{ props.proficiency.name }}</div>
        <div class="text-right">
          {{ props.proficiency.value }}
        </div>
      </div>
    </AccordionHeader>
    <AccordionContent>
      <div class="flex flex-column w-100">
        <div class="flex flex-row w-100 align-content-between">
          <div class="flex-fill">
            <strong>Source</strong>
          </div>
          <div>
            <strong>Bonus</strong>
          </div>
        </div>
        <div
          v-for="(modifier, index) in props.proficiency.appliedModifiers" :key="index"
          class="flex flex-row w-100"
        >
          <div class="flex-fill">
            {{ modifier.name }}
          </div>
          <div class="text-right">
            {{ modifier.value >= 0 ? '+' : '' }}{{ modifier.value }}
          </div>
        </div>
      </div>
    </AccordionContent>
  </AccordionPanel>
</template>
