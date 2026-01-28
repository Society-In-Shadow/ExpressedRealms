<script setup lang="ts">

import Button from 'primevue/button'
import { onBeforeMount } from 'vue'
import { useRoute } from 'vue-router'
import type { WizardContent } from '@/components/characters/character/wizard/types.ts'
import { wizardContentStore } from '@/components/characters/character/wizard/stores/wizardContentStore.ts'
import { contactStore } from '@/components/characters/character/wizard/contacts/stores/contactStore.ts'
import EditContact from '@/components/characters/character/wizard/contacts/EditContact.vue'
import AddContact from '@/components/characters/character/wizard/contacts/AddContact.vue'
import { XpSectionTypes } from '@/components/characters/character/stores/experienceBreakdownStore.ts'
import ShowXPCosts from '@/components/characters/character/wizard/ShowXPCosts.vue'
import type { Contact } from '@/components/characters/character/wizard/contacts/types.ts'

const wizardContentInfo = wizardContentStore()
const contactData = contactStore()
const route = useRoute()

onBeforeMount(async () => {
  await contactData.getContacts(route.params.id)
})

const toggleEdit = (contact: Contact) => {
  wizardContentInfo.updateContent(
    {
      headerName: 'Contact',
      component: EditContact,
      props: { contactId: contact.id },
    } as WizardContent,
  )
}

const toggleAdd = () => {
  wizardContentInfo.updateContent(
    {
      headerName: 'Contact',
      component: AddContact,
    } as WizardContent)
}

</script>

<template>
  <div style="max-width: 650px; margin: 0 auto;">
    <div>
      <ShowXPCosts :section-type="XpSectionTypes.contacts" />
    </div>
    <div class="d-flex justify-content-between align-items-center mb-2">
      <h1>Contacts</h1>
      <Button class="float-end" size="small" label="Add" @click="toggleAdd" />
    </div>
    <div v-if="contactData.contacts.length === 0">
      <p>No Contacts detected, please add one below.</p>
    </div>

    <div v-for="contact in contactData.contacts" :key="contact.id" class="pl-md-3">
      <div class="d-flex flex-column flex-md-row align-self-center justify-content-between pt-2 pb-2">
        <div class="d-flex flex-column flex-grow-1 pr-3">
          <div class="d-flex flex-fill align-content-between d-block">
            <div class="flex-grow-1 font-bold text-900">
              {{ contact.name }}
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
        <div class="text-end">
          <Button label="View" size="small" @click="toggleEdit(contact)" />
        </div>
      </div>
    </div>
  </div>
</template>
