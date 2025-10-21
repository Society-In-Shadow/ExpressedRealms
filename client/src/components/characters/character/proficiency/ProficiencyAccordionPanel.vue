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
      <div class="p-datatable p-component p-datatable-striped">
        <div class="p-datatable-table-container">
          <table class="w-100 p-datatable-table">
            <!-- Table header -->
            <thead class="p-datatable-thead">
              <tr>
                <th class="p-datatable-header-cell">
                  Source
                </th>
                <th class="p-datatable-header-cell text-right">
                  Bonus
                </th>
              </tr>
            </thead>
            <tbody class="p-datatable-tbody">
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
        </div>
      </div>
    </AccordionContent>
  </AccordionPanel>
</template>
