<script setup lang="ts">

import FormTextAreaWrapper from '@/FormWrappers/FormTextAreaWrapper.vue'
import Button from 'primevue/button'
import { computed, ref, watch } from 'vue'
import Column from 'primevue/column'
import DataTable from 'primevue/datatable'
import { experienceStore, XpSectionTypes } from '@/components/characters/character/stores/experienceBreakdownStore.ts'
import ShowXPCosts from '@/components/characters/character/wizard/ShowXPCosts.vue'
import type { CalculatedExperience } from '@/components/characters/character/types.ts'
import {
  getValidationInstance,
} from '@/components/characters/character/wizard/contacts/validators/contactValidations.ts'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import { contactStore } from '@/components/characters/character/wizard/contacts/stores/contactStore.ts'
import FormDropdownWrapper from '@/FormWrappers/FormDropdownWrapper.vue'
import { useRoute } from 'vue-router'
import type { ContactFrequency, ContactKnowledgeLevels } from '@/components/characters/character/wizard/contacts/types.ts'
import { number } from 'yup'
import {
  confirmationPopup,
} from '@/components/characters/character/wizard/contacts/services/confirmationPopupService.ts'

const store = contactStore()
const form = getValidationInstance()
const xpInfo = experienceStore()
const route = useRoute()
const sectionInfo = ref<CalculatedExperience>({})

const props = defineProps({
  contactId: {
    type: number,
    required: true,
  },
})

const popups = confirmationPopup(route.params.id, props.contactId)

watch(props, async () => {
  await loadInfo()
}, { immediate: true })

async function loadInfo() {
  const contact = await store.getContact(route.params.id, props.contactId)
  form.setValues(contact)
  sectionInfo.value = xpInfo.getExperienceInfoForSection(XpSectionTypes.contacts)

  // Hide options that are too expensive
  updateKnowledgeLevels()
  updateFrequencyLevels()
}

const updateKnowledgeLevels = () => {
  store.knowledgeLevels.forEach(function (level: ContactKnowledgeLevels) {
    const xpCost = level.cost + form.fields.frequency?.field.value?.cost ?? 0
    level.isDisabled = xpCost > sectionInfo.value.availableXp
  })
}

const updateFrequencyLevels = () => {
  store.contactFrequency.forEach(function (frequency: ContactFrequency) {
    const xpCost = frequency.cost + form.fields.knowledgeLevel.field.value.cost
    frequency.isDisabled = xpCost > sectionInfo.value.availableXp
  })
}

const onSubmit = form.handleSubmit(async (values) => {
  await store.updateContact(values, route.params.id, props.contactId)
})

const totalCost = computed(() => {
  if (form.fields.frequency && form.fields.knowledgeLevel && form.fields.frequency.field.value && form.fields.knowledgeLevel.field.value)
    return form.fields.frequency.field.value.cost + form.fields.knowledgeLevel.field.value.cost
  return 0
})

const knowledgeLevelCost = computed(() => {
  return form.fields.knowledgeLevel?.field.value?.cost ?? 0
})

const frequencyCost = computed(() => {
  return form.fields.frequency?.field.value?.cost ?? 0
})

</script>

<template>
  <div>
    <ShowXPCosts :section-type="XpSectionTypes.contacts" />
  </div>

  <div class="mt-3 text-right">
    <strong>Cost:</strong> {{ totalCost }}
  </div>
  <form @submit="onSubmit">
    <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
      <h1>Add Contact</h1>
      <div class="p-0 m-2 d-inline-flex align-items-start align-items-center gap-2">
        <Button label="Delete" size="small" severity="danger" @click="popups.deleteConfirmation($event)" />
        <Button label="Update" size="small" type="submit" />
      </div>
    </div>

    <FormInputTextWrapper v-model="form.fields.name" />

    <FormDropdownWrapper
      v-model="form.fields.knowledge" option-label="name" :options="store.knowledges" :show-description="true" filter
      disabled
    />
    <div class="mt-4">
      <FormTextAreaWrapper v-model="form.fields.notes" :disabled="sectionInfo.availableXp == 0" />
    </div>

    <div class="mt-3">
      Knowledge Level
    </div>
    <DataTable
      v-model:selection="form.fields.knowledgeLevel.field.value" selection-mode="single" :value="store.knowledgeLevels" data-key="levelId" :row-class="row => (row.isDisabled ? 'non-selectable' : '')"
      @row-select="updateFrequencyLevels()"
    >
      <Column selection-mode="single" header-style="width: 3rem" />
      <Column field="name" header="Name" />
      <Column field="cost" header="XP">
        <template #body="slotProps">
          {{ slotProps.data.cost > knowledgeLevelCost ? "-" : "+" }}{{ Math.abs(slotProps.data.cost - knowledgeLevelCost) }}
        </template>
      </Column>
    </DataTable>

    <div class="mt-3">
      Contact Frequency Per Week
    </div>
    <DataTable
      v-model:selection="form.fields.frequency.field.value" selection-mode="single" :value="store.contactFrequency" data-key="frequency" :row-class="row => (row.isDisabled ? 'non-selectable' : '')"
      @row-select="updateKnowledgeLevels()"
    >
      <Column selection-mode="single" header-style="width: 3rem" />
      <Column field="frequency" header="Contacts" />
      <Column field="cost" header="XP" header-class="text-right" body-class="text-right">
        <template #body="slotProps">
          {{ slotProps.data.cost > frequencyCost ? "-" : "+" }}{{ Math.abs(slotProps.data.cost - frequencyCost) }}
        </template>
      </Column>
    </DataTable>
  </form>
</template>

<style>
:deep(th.text-center .p-datatable-column-header-content) {
  justify-content: center;
}
:deep(th.text-right .p-datatable-column-header-content) {
  justify-content: right;
}
.non-selectable { opacity:.6; pointer-events:none; }
</style>
