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
      <table class="w-100">
        <thead>
          <tr>
            <th class="text-left">
              Source
            </th>
            <th class="text-right">
              Bonus
            </th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="(modifier, index) in props.proficiency.appliedModifiers" :key="index"
            :class="index % 2 === 0 ? 'p-row-even' : 'p-row-odd'"
          >
            <td>
              {{ modifier.name }}
            </td>
            <td class="text-right">
              {{ modifier.value >= 0 ? '+' : '' }}{{ modifier.value }}
            </td>
          </tr>
        </tbody>
      </table>
    </AccordionContent>
  </AccordionPanel>
</template>
