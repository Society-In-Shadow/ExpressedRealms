<script setup lang="ts">
import FormTextAreaWrapper from '@/FormWrappers/FormTextAreaWrapper.vue'
import Button from 'primevue/button'
import {getValidationInstance} from '@/components/characters/character/knowledges/validations/knowledgeValidations'
import {characterKnowledgeStore} from '@/components/characters/character/knowledges/stores/characterKnowledgeStore'
import {useRoute} from 'vue-router'
import {ref, watch} from 'vue'
import type {CharacterKnowledge, KnowledgeOptions} from '@/components/characters/character/knowledges/types'
import Message from 'primevue/message'
import {addKnowledgeDialog} from '@/components/characters/character/knowledges/services/dialogs.ts'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import {confirmationPopup} from '@/components/characters/character/knowledges/services/confirmationService.ts'
import {experienceStore, XpSectionTypes} from '@/components/characters/character/stores/experienceBreakdownStore.ts'
import ShowXPCosts from '@/components/characters/character/wizard/ShowXPCosts.vue'
import type {CalculatedExperience} from '@/components/characters/character/types.ts'

const dialogService = addKnowledgeDialog()
const store = characterKnowledgeStore()
const xpInfo = experienceStore()
const form = getValidationInstance()
const route = useRoute()
const popupService = confirmationPopup(route.params.id)
const sectionInfo = ref<CalculatedExperience>({})

const selectedKnowledgeLevel = ref<KnowledgeOptions>()

const props = defineProps({
  knowledgeMappingId: {
    type: Number,
    required: true,
  },
})
const knowledge = ref<CharacterKnowledge>({})
const isUnknownKnowledge = ref(false)

watch(props, async () => {
  await loadInfo()
}, { immediate: true })

watch(() => store.knowledges, async () => {
  await loadInfo()
}, { immediate: true, deep: true })

async function loadInfo() {
  knowledge.value = store.knowledges.find(function (characterKnowledge: CharacterKnowledge) {
    return characterKnowledge.mappingId == props.knowledgeMappingId
  })

  isUnknownKnowledge.value = knowledge.value.knowledge.type === 'Unknown'

  await store.getKnowledgeLevels(route.params.id)

  sectionInfo.value = xpInfo.getExperienceInfoForSection(XpSectionTypes.knowledges)

  store.knowledgeLevels.map(function (level: KnowledgeOptions) {
    const xpCost = isUnknownKnowledge.value ? level.totalUnknownXpCost : level.totalGeneralXpCost
    const isLowerLevel = level.id > knowledge.value.levelId
    level.disabled = xpCost > sectionInfo.value.availableXp && isLowerLevel
    return level
  })

  selectedKnowledgeLevel.value = store.knowledgeLevels.find(function (level: KnowledgeOptions) {
    return level.id === knowledge.value.levelId
  })

  form.setValues(knowledge.value)
}

const onSubmit = form.handleSubmit(async (values) => {
  await store.editKnowledge(values, selectedKnowledgeLevel.value.id, route.params.id, knowledge.value.mappingId)
})

function getCurrentXpLevel(levelId: number) {
  let level = store.knowledgeLevels.filter(function (level: KnowledgeOptions) {
    return level.id === levelId
  })[0]

  if (isUnknownKnowledge.value) {
    return level.totalUnknownXpCost
  }
  return level.totalGeneralXpCost
}

const onRowUnselect = (event) => {
  let level = store.knowledgeLevels.find(function (level: KnowledgeOptions) {
    return level.id === knowledge.value.levelId
  })
  selectedKnowledgeLevel.value = level
}

</script>

<template>
  <form @submit="onSubmit">
    <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
      <div>
        <h2 class="p-0 m-0">
          {{ knowledge.knowledge.name }}
        </h2>
        <div>{{ knowledge.knowledge.type }}</div>
      </div>
      <div class="p-0 m-2 d-inline-flex align-items-start align-items-center gap-2">
        <Button label="Delete" size="small" severity="danger" @click="popupService.deleteConfirmation($event, knowledge.mappingId )" />
        <Button label="Update" size="small" type="submit" />
      </div>
    </div>

    <p>{{ knowledge.knowledge.description }}</p>
    <div>
      <ShowXPCosts :section-type="XpSectionTypes.knowledges" />
    </div>
    <div v-if="sectionInfo.availableXp == 0">
      <Message severity="warn" class="my-4">
        You are out of experience to spend on Knowledges.
      </Message>
    </div>
    <DataTable
      v-model:selection="selectedKnowledgeLevel" selection-mode="single" :value="store.knowledgeLevels" data-key="id" :row-class="row => (row.disabled ? 'non-selectable' : '')"
      @row-unselect="onRowUnselect"
    >
      <Column selection-mode="single" header-style="width: 3rem" />
      <Column field="name" header="Name" />
      <Column field="totalGeneralXpCost" header="XP" header-class="text-center" body-class="text-center">
        <template #body="slotProps">
          {{ slotProps.data.totalGeneralXpCost > getCurrentXpLevel(knowledge.levelId) ? "-" : "+" }}{{ Math.abs(slotProps.data.totalGeneralXpCost - getCurrentXpLevel(knowledge.levelId)) }}
        </template>
      </Column>
      <Column field="stoneModifier" header="Stones" header-class="text-center" body-class="text-center" />
      <Column field="specializationCount" header="Specials" header-class="text-center" body-class="text-center" />
    </DataTable>

    <Message v-if="form.knowledgeLevel2.field.value && form.knowledgeLevel2.field.value.id == 8" severity="warn" class="mt-4">
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
        <Button label="Delete" severity="danger" @click="popupService.deleteSpecializationConfirmation($event, knowledge.mappingId, special.id)" />
        <Button label="Edit" @click="dialogService.showEditSpecialization(knowledge, special)" />
      </div>
    </div>
  </div>
  <div class="text-right mt-2">
    <Button v-if="knowledge.specializationCount > knowledge.specializations.length" class="btn btn-primary text-right" label="Add Specialization" @click="dialogService.showAddSpecialization(knowledge)" />
  </div>
</template>

<style>
:deep(th.text-center .p-datatable-column-header-content) {
  justify-content: center;
}
.non-selectable { opacity:.6; pointer-events:none; }

</style>
