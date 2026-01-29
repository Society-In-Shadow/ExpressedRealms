<script setup lang="ts">

import { onBeforeMount } from 'vue'
import { useRoute } from 'vue-router'
import { contactStore } from '@/components/characters/character/wizard/contacts/stores/contactStore.ts'
import AccordionPanel from 'primevue/accordionpanel'
import Accordion from 'primevue/accordion'
import AccordionContent from 'primevue/accordioncontent'
import AccordionHeader from 'primevue/accordionheader'

const contactData = contactStore()
const route = useRoute()

onBeforeMount(async () => {
  await contactData.getContactsForSheet(route.params.id)
})

</script>

<template>
  <div style="max-width: 650px; margin: 0 auto;">
    <div v-if="contactData.contactsForCharacterSheet.length === 0">
      <p>No Contacts detected, please add one in the wizard!</p>
    </div>

    <Accordion multiple expand-icon="pi pi-info-circle" collapse-icon="pi pi-times-circle">
      <AccordionPanel v-for="contact in contactData.contactsForCharacterSheet" :key="contact.id" :value="contact.id">
        <AccordionHeader>
          <div class="d-flex flex-column flex-grow-1 pr-3">
            <div class="d-flex flex-fill align-content-between d-block">
              <div class="d-flex flex-grow-1 font-bold text-900">
                <div>{{ contact.name }}</div>
                <span v-if="contact.isApproved" class="material-symbols-outlined pl-2" title="Contact was Approved, will show up in CRB">
                  check_circle
                </span>
              </div>
              <div>
                {{ contact.knowledgeLevel }}
              </div>
            </div>
            <div class="d-flex d-block mt-1">
              <div class="flex-grow-1">
                {{ contact.knowledge }}
              </div>
              <div>{{ contact.usesPerWeek }} uses / week</div>
            </div>
          </div>
        </AccordionHeader>
        <AccordionContent>
          <h3>Knowledge Information</h3>
          <div>{{ contact.knowledgeDescription }}</div>
          <h3 v-if="contact.notes">
            Notes
          </h3>
          <div v-if="contact.notes">
            {{ contact.notes }}
          </div>
        </AccordionContent>
      </AccordionPanel>
    </Accordion>
  </div>
</template>
