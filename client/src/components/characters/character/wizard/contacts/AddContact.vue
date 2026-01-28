<script setup lang="ts">

import FormTextAreaWrapper from '@/FormWrappers/FormTextAreaWrapper.vue'
import Button from 'primevue/button'
import { computed, onBeforeMount, ref } from 'vue'
import Message from 'primevue/message'
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
import FormWrapper from '@/FormWrappers/FormWrapper.vue'

const store = contactStore()
const form = getValidationInstance()
const xpInfo = experienceStore()
const route = useRoute()
const sectionInfo = ref<CalculatedExperience>({})

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
  <FormWrapper :is-disabled="!canAdd" @submit="onSubmit">
    <FormInputTextWrapper v-model="form.fields.name" />

    <FormDropdownWrapper v-model="form.fields.knowledge" option-label="name" :options="store.knowledges" :show-description="true" filter />
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
      <Column field="cost" header="XP" class="col-number">
        <template #body="slotProps">
          -{{ slotProps.data.cost }}
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
      <Column field="cost" header="XP" class="col-number">
        <template #body="slotProps">
          <span v-if="slotProps.data.cost == 0">0</span>
          <span v-else>-{{ slotProps.data.cost }}</span>
        </template>
      </Column>
    </DataTable>

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

<style>
.p-datatable th.col-number .p-datatable-column-header-content {
  justify-content: flex-end;
}

.p-datatable td.col-number{
  text-align: right !important;
}
.non-selectable { opacity:.6; pointer-events:none; }
</style>
