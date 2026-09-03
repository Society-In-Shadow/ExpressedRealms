<script setup lang="ts">
import FormTextAreaWrapper from '@/FormWrappers/FormTextAreaWrapper.vue'
import Button from 'primevue/button'
import { getValidationInstance } from '@/components/characters/character/knowledges/validations/knowledgeValidations'
import { characterKnowledgeStore } from '@/components/characters/character/knowledges/stores/characterKnowledgeStore'
import { useRoute } from 'vue-router'
import { ref, watch } from 'vue'
import type { EditKnowledge, KnowledgeLevel, KnowledgeOptions } from '@/components/characters/character/knowledges/types'
import Message from 'primevue/message'
import { addKnowledgeDialog } from '@/components/characters/character/knowledges/services/dialogs.ts'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import { confirmationPopup } from '@/components/characters/character/knowledges/services/confirmationService.ts'
import { experienceStore, XpSectionTypes } from '@/components/characters/character/stores/experienceBreakdownStore.ts'
import ShowXPCosts from '@/components/characters/wizard/ShowXPCosts.vue'
import type { CalculatedExperience } from '@/components/characters/character/types.ts'
import axios from 'axios'

const dialogService = addKnowledgeDialog()
const store = characterKnowledgeStore()
const xpInfo = experienceStore()
const form = getValidationInstance()
const route = useRoute()
const popupService = confirmationPopup(route.params.id)
const sectionInfo = ref<CalculatedExperience>({})

const selectedKnowledgeLevel = ref<KnowledgeLevel>()

const props = defineProps({
  knowledgeMappingId: {
    type: Number,
    required: true,
  },
})
const knowledge = ref<EditKnowledge>({})

watch(props, async () => {
  await loadInfo()
}, { immediate: true })

async function loadInfo() {
  console.log('loaded')
  knowledge.value = await axios.get<EditKnowledge>(`characters/${route.params.id}/knowledges/${props.knowledgeMappingId}`)
    .then(async (response) => { return response.data })

  sectionInfo.value = xpInfo.getExperienceInfoForSection(XpSectionTypes.knowledges)

  knowledge.value.knowledgeLevels.map(function (level: KnowledgeLevel) {
    let xpCost = getCurrentXpLevel(level.id) - getCurrentXpLevel(knowledge.value.selectedLevelId)

    const factionRequirementMinimum = knowledge.value.minimumKnowledgeId > level.id
    level.disabled = xpCost > sectionInfo.value.availableXp || factionRequirementMinimum
    return level
  })

  selectedKnowledgeLevel.value = knowledge.value.knowledgeLevels.find(function (level: KnowledgeOptions) {
    return level.id === knowledge.value.selectedLevelId
  })

  form.setValues(knowledge.value)
}

const onSubmit = form.handleSubmit(async (values) => {
  await store.editKnowledge(values, selectedKnowledgeLevel.value.id, route.params.id, props.knowledgeMappingId)
  await loadInfo()
})

function getCurrentXpLevel(levelId: number) {
  return knowledge.value.knowledgeLevels.filter(function (level: KnowledgeLevel) {
    return level.id === levelId
  })[0].totalXpCost ?? 0
}

function getFactionLevel() {
  const level = knowledge.value.knowledgeLevels.filter(function (level: KnowledgeLevel) {
    return level.id === knowledge.value.minimumKnowledgeId
  })[0]

  return `${level.name} (${level.level})`
}

const onRowUnselect = (event) => {
  selectedKnowledgeLevel.value = event.data
}

const showEditSpecialization = async (special) => {
  const result = await dialogService.showEditSpecialization({ knowledge: knowledge.value, specialization: special, mappingId: props.knowledgeMappingId })

  if (result?.action == 'edited') {
    await loadInfo()
  }
}

const showAddSpecialization = async () => {
  const result = await dialogService.showAddSpecialization({ knowledge: knowledge.value, mappingId: props.knowledgeMappingId })

  if (result?.action == 'added') {
    await loadInfo()
  }
}

const showDeleteConfirmation = async (event, id: number) => {
  const result = await popupService.deleteSpecializationConfirmation(event, props.knowledgeMappingId, id)

  if (result == 'deleted')
    await loadInfo()
}
</script>

<template>
  <form v-if="knowledge.name" @submit="onSubmit">
    <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
      <div>
        <h2 class="p-0 m-0">
          {{ knowledge.name }}
        </h2>
        <div>{{ knowledge.knowledgeType }}</div>
      </div>
      <div class="p-0 m-2 d-inline-flex align-items-start align-items-center gap-2">
        <Button v-if="knowledge.blockFactionChanges" label="Delete" size="small" severity="danger" @click="popupService.deleteConfirmation($event, props.knowledgeMappingId )" />
        <Button label="Update" size="small" type="submit" />
      </div>
    </div>

    <p>{{ knowledge.description }}</p>
    <div>
      <ShowXPCosts :section-type="XpSectionTypes.knowledges" />
    </div>
    <div v-if="sectionInfo.availableXp == 0">
      <Message severity="warn" class="my-4">
        You are out of experience to spend on Knowledges.
      </Message>
    </div>
    <div v-if="!knowledge.blockFactionChanges">
      <Message severity="warn" class="my-4">
        Your faction level requires you to be at least {{ getFactionLevel(selectedKnowledgeLevel.level) }}
      </Message>
    </div>
    <DataTable
      v-model:selection="selectedKnowledgeLevel" selection-mode="single" :value="knowledge.knowledgeLevels" data-key="id" :row-class="row => (row.disabled ? 'non-selectable' : '')"
      @row-unselect="onRowUnselect"
    >
      <Column selection-mode="single" header-style="width: 3rem" />
      <Column field="name" header="Name">
        <template #body="slotProps">
          {{ slotProps.data.name }} ({{ slotProps.data.level }})
        </template>
      </Column>
      <Column field="totalGeneralXpCost" header="XP" header-class="text-center" body-class="text-center">
        <template #body="slotProps">
          {{
            getCurrentXpLevel(slotProps.data.id) > getCurrentXpLevel(knowledge.selectedLevelId) ? "-" : "+"
          }}{{ Math.abs(getCurrentXpLevel(slotProps.data.id) - getCurrentXpLevel(knowledge.selectedLevelId)) }}
        </template>
      </Column>
      <Column field="stones" header="Stones" header-class="text-center" body-class="text-center" />
      <Column field="specializationCount" header="Specials" header-class="text-center" body-class="text-center" />
    </DataTable>

    <Message v-if="selectedKnowledgeLevel && selectedKnowledgeLevel.level == 7" severity="warn" class="mt-4">
      <p>
        Gaining the seventh level of knowledge also requires the completion of a quest of some kind. The quest can be as
        straightforward as finding lost or unknown relics that relate to the subject or as complicated as a life-long
        journey to discover new theories and paradigms of the knowledge. In either case, the quest should have some
        bearing on the field of the knowledge.
      </p>
      <p>
        Selecting this will require interaction with a GO to get the quest approved.  Use the notes field below to
        keep track of your ideas, and anything you may have discussed with your GO.
      </p>
    </Message>

    <div class="pt-4">
      <FormTextAreaWrapper v-model="form.notes" />
    </div>
  </form>

  <div v-if="knowledge.name">
    <hr v-if="knowledge.specializations.length > 0" class="mt-2 mb-2">
    <h1 v-if="knowledge.specializations.length > 0" class="mt-3">
      Specializations
    </h1>
    <div v-if="knowledge.specializations.length > 0">
      <div v-for="special in knowledge.specializations" :key="special.id">
        <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
          <div>
            <h2 class="m-0 p-0">
              {{ special.name }}
            </h2>
          </div>
        </div>

        <p>{{ special.description }}</p>
        <h4 v-if="special.notes">
          Notes
        </h4>
        <p v-if="special.notes">
          {{ special.notes }}
        </p>

        <div class="p-0 m-0 d-flex justify-content-between">
          <Button v-if="!special.blockFactionChanges" label="Delete" severity="danger" @click="showDeleteConfirmation($event, special.id)" />
          <Button label="Edit" @click="showEditSpecialization( special )" />
        </div>
      </div>
    </div>
    <div class="text-right mt-2">
      <Button v-if="selectedKnowledgeLevel!.specializationCount > knowledge.specializations.length" class="btn btn-primary text-right" label="Add Specialization" @click="showAddSpecialization()" />
    </div>
  </div>
</template>

<style>
:deep(th.text-center .p-datatable-column-header-content) {
  justify-content: center;
}
.non-selectable { opacity:.6; pointer-events:none; }

</style>
