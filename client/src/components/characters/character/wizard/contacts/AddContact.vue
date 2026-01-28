<script setup lang="ts">

import FormTextAreaWrapper from '@/FormWrappers/FormTextAreaWrapper.vue'
import Button from 'primevue/button'
import { computed, onBeforeMount, ref } from 'vue'
import Message from 'primevue/message'
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
import FormWrapper from '@/FormWrappers/FormWrapper.vue'
import FormRadioTableWrapper from '@/FormWrappers/FormRadioTableWrapper.vue'

const store = contactStore()
const form = getValidationInstance()
const xpInfo = experienceStore()
const route = useRoute()
const sectionInfo = ref<CalculatedExperience>({})
const isLoaded = ref(false)

onBeforeMount(async () => {
  await loadInfo()
})

async function loadInfo() {
  await store.getOptions()
  sectionInfo.value = xpInfo.getExperienceInfoForSection(XpSectionTypes.contacts)
  // Set minimimum defaults
  form.fields.frequency.field.value = store.contactFrequency.find(x => x.frequency == 1)
  form.fields.knowledgeLevel.field.value = store.knowledgeLevels.find(x => x.levelId == 4)

  // Hide options that are too expensive
  updateKnowledgeLevels()
  updateFrequencyLevels()
  isLoaded.value = true
}

const updateKnowledgeLevels = () => {
  store.knowledgeLevels.forEach(function (level: ContactKnowledgeLevels) {
    const xpCost = level.cost + form.fields.frequency.field.value.cost
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
  await store.addContact(route.params.id, values)
})

const totalCost = computed(() => {
  if (form.fields.frequency && form.fields.knowledgeLevel && form.fields.frequency.field.value && form.fields.knowledgeLevel.field.value)
    return form.fields.frequency.field.value.cost + form.fields.knowledgeLevel.field.value.cost
  return 0
})

const canAdd = computed(() => {
  return sectionInfo.value.availableXp >= totalCost.value
})

function selectKnowledgeLevel(level) {
  if (level.isDisabled) return

  form.fields.knowledgeLevel.field.value = level
  updateFrequencyLevels()
}

function selectFrequency(freq) {
  if (freq.isDisabled) return

  form.fields.frequency.field.value = freq
  updateKnowledgeLevels()
}

</script>

<template>
  <div>
    <ShowXPCosts :section-type="XpSectionTypes.contacts" />
  </div>
  <h1>Add Contact</h1>
  <Message v-if="!canAdd" severity="warn" class="my-3">
    You do not have enough experience to add this contact
  </Message>
  <div v-if="sectionInfo.availableXp == 0">
    <Message severity="warn" class="my-4">
      You are out of experience to spend on Contacts.
    </Message>
  </div>
  <FormWrapper :is-disabled="!canAdd" :show-skeleton="!isLoaded" @submit="onSubmit">
    <FormInputTextWrapper v-model="form.fields.name" />

    <FormDropdownWrapper v-model="form.fields.knowledge" option-label="name" :options="store.knowledges" :show-description="true" filter />
    <div class="mt-4">
      <FormTextAreaWrapper v-model="form.fields.notes" :disabled="sectionInfo.availableXp == 0" />
    </div>

    <FormRadioTableWrapper v-model="form.fields.knowledgeLevel" row-key="levelId" :row-data="store.knowledgeLevels" @selected-item="selectKnowledgeLevel">
      <template #header>
        <span>Name</span>
        <span class="xp">XP</span>
      </template>
      <template #row="{ data }">
        <span>{{ data.name }}</span>
        <span class="xp">-{{ data.cost }}</span>
      </template>
    </FormRadioTableWrapper>

    <FormRadioTableWrapper v-model="form.fields.frequency" row-key="frequency" :row-data="store.contactFrequency" @selected-item="selectFrequency">
      <template #header>
        <span>Contacts</span>
        <span class="xp">XP</span>
      </template>
      <template #row="{ data }">
        <span>{{ data.frequency }}</span>
        <span class="xp">
          <span v-if="data.cost === 0">0</span>
          <span v-else>-{{ data.cost }}</span>
        </span>
      </template>
    </FormRadioTableWrapper>

    <div class="mt-3">
      <strong>Cost:</strong> {{ totalCost }}
    </div>
    <Message v-if="!canAdd" severity="warn" class="my-3">
      You do not have enough experience to add this contact
    </Message>

    <div class="m-3 text-right">
      <Button label="Add" class="m-2" type="submit" :disabled="!canAdd" />
    </div>
  </FormWrapper>
</template>
