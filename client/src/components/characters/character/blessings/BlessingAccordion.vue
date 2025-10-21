<script setup lang="ts">

import AccordionHeader from 'primevue/accordionheader'
import Accordion from 'primevue/accordion'
import AccordionPanel from 'primevue/accordionpanel'
import AccordionContent from 'primevue/accordioncontent'
import type {CharacterBlessing} from '@/components/characters/character/wizard/blessings/types.ts'

const props = defineProps({
  blessings: {
    type: Object as PropType<Array<CharacterBlessing>>,
    required: true,
  },
})

</script>

<template>
  <Accordion :value="openKnowledgeItems" multiple expand-icon="pi pi-info-circle" collapse-icon="pi pi-times-circle">
    <AccordionPanel v-for="item in props.blessings" :key="item.blessingId" :value="item.blessingId">
      <AccordionHeader>
        <div class="d-flex flex-column flex-grow-1 pr-3">
          <div class="d-flex flex-fill align-content-between d-block">
            <div class="flex-grow-1 font-bold text-900">
              {{ item.name }}
            </div>
            <div>
              {{ item.subCategory }}
            </div>
          </div>
        </div>
      </AccordionHeader>
      <AccordionContent>
        <div class="pt-0 mt-0" v-html="item.description" />
        <h3>Effect</h3>
        <div v-html="item.levelDescription" />
        <h3 v-if="item.notes">
          Notes
        </h3>
        <div>{{ item.notes }}</div>
      </AccordionContent>
    </AccordionPanel>
  </Accordion>
</template>

<style>
@media(max-width: 768px){
  .card-body-fix .p-card-body{
    padding: 0 !important;
  }
}
</style>
