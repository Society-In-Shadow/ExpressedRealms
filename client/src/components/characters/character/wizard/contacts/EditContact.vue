<script setup lang="ts">

import FormTextAreaWrapper from '@/FormWrappers/FormTextAreaWrapper.vue'
import Button from 'primevue/button'
import { computed, ref, watch } from 'vue'
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
import type {
  ContactFrequency,
  ContactKnowledgeLevels,
  EditContact,
} from '@/components/characters/character/wizard/contacts/types.ts'
import { number } from 'yup'
import {
  confirmationPopup,
} from '@/components/characters/character/wizard/contacts/services/confirmationPopupService.ts'
import FormWrapper from '@/FormWrappers/FormWrapper.vue'
import FormRadioTableWrapper from '@/FormWrappers/FormRadioTableWrapper.vue'

const store = contactStore()
const form = getValidationInstance()
const xpInfo = experienceStore()
const route = useRoute()
const sectionInfo = ref<CalculatedExperience>({})
const originalXp = ref<number>(0)
const initialLoad = ref<boolean>(true)
const contact = ref<EditContact>({} as EditContact)

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
  contact.value = await store.getContact(route.params.id, props.contactId)
  form.setValues(contact.value)
  sectionInfo.value = xpInfo.getExperienceInfoForSection(XpSectionTypes.contacts)

  if (initialLoad.value) {
    originalXp.value = totalCost.value
    initialLoad.value = false
  }

  // Hide options that are too expensive
  updateKnowledgeLevels()
  updateFrequencyLevels()
}

const updateKnowledgeLevels = () => {
  store.knowledgeLevels.forEach(function (level: ContactKnowledgeLevels) {
    const xpCost = Math.abs(level.cost - knowledgeLevelCost.value)
    const isLowerLevelOrSame = level.levelId <= form.fields.knowledgeLevel.field.value.levelId
    level.isDisabled = ((xpCost > sectionInfo.value.availableXp + newlyAvailableXp.value) && !isLowerLevelOrSame) || contact.value.isApproved
  })
}

const updateFrequencyLevels = () => {
  store.contactFrequency.forEach(function (frequency: ContactFrequency) {
    const xpCost = Math.abs(frequency.cost - frequencyCost.value)
    const isLowerLevelOrSame = frequency.frequency <= form.fields.frequency.field.value.frequency
    frequency.isDisabled = ((xpCost > sectionInfo.value.availableXp + newlyAvailableXp.value) && !isLowerLevelOrSame) || contact.value.isApproved
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

const newlyAvailableXp = computed(() => originalXp.value - totalCost.value)

</script>

<template>
  <div>
    <ShowXPCosts :section-type="XpSectionTypes.contacts" />
  </div>

  <div class="mt-3 text-right">
    <strong>Cost:</strong> {{ totalCost }}
  </div>
  <FormWrapper :is-disabled="contact.isApproved" :show-skeleton="initialLoad" @submit="onSubmit">
    <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
      <h1>Edit Contact</h1>
      <div v-if="!contact.isApproved" class="p-0 m-2 d-inline-flex align-items-start align-items-center gap-2">
        <Button label="Delete" size="small" severity="danger" @click="popups.deleteConfirmation($event)" />
        <Button label="Update" size="small" type="submit" />
      </div>
    </div>

    <FormInputTextWrapper v-model="form.fields.name" />

    <FormDropdownWrapper
      v-model="form.fields.knowledge" option-label="name" :options="store.knowledges" :show-description="true" filter
      :is-disabled="true"
    />
    <div class="mt-4">
      <FormTextAreaWrapper v-model="form.fields.notes" :disabled="sectionInfo.availableXp == 0" />
    </div>

    <FormRadioTableWrapper v-model="form.fields.knowledgeLevel" row-key="levelId" :row-data="store.knowledgeLevels" @selected-item="updateFrequencyLevels">
      <template #header>
        <span>Name</span>
        <span class="xp">XP</span>
      </template>
      <template #row="{ data }">
        <span>{{ data.name }}</span>
        <span class="xp">{{ data.cost > knowledgeLevelCost ? "-" : "+" }}{{ Math.abs(data.cost - knowledgeLevelCost) }}</span>
      </template>
    </FormRadioTableWrapper>

    <FormRadioTableWrapper v-model="form.fields.frequency" row-key="frequency" :row-data="store.contactFrequency" @selected-item="updateKnowledgeLevels">
      <template #header>
        <span>Contacts</span>
        <span class="xp">XP</span>
      </template>
      <template #row="{ data }">
        <span>{{ data.frequency }}</span>
        <span class="xp">
          <span>{{ data.cost > frequencyCost ? "-" : "+" }}{{ Math.abs(data.cost - frequencyCost) }}</span>
        </span>
      </template>
    </FormRadioTableWrapper>
  </FormWrapper>
</template>
