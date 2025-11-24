<script setup lang="ts">

import FormTextAreaWrapper from '@/FormWrappers/FormTextAreaWrapper.vue'
import Button from 'primevue/button'
import { characterKnowledgeStore } from '@/components/characters/character/knowledges/stores/characterKnowledgeStore'
import { useRoute } from 'vue-router'
import { inject, onBeforeMount, ref } from 'vue'
import Message from 'primevue/message'

import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import {
  getValidationInstance,
} from '@/components/characters/character/knowledges/validations/specializationValidations'
import type { CharacterKnowledge } from '@/components/characters/character/knowledges/types'
import ShowXPCosts from '@/components/characters/character/wizard/ShowXPCosts.vue'
import { experienceStore, XpSectionTypes } from '@/components/characters/character/stores/experienceBreakdownStore.ts'
import type { CalculatedExperience } from '@/components/characters/character/types.ts'

const store = characterKnowledgeStore()
const form = getValidationInstance()
const xpInfo = experienceStore()
const route = useRoute()
const sectionInfo = ref<CalculatedExperience>({})

const dialogRef = inject('dialogRef')

const knowledge = ref<CharacterKnowledge>(dialogRef.value.data.knowledge)

onBeforeMount(async () => {
  await store.getKnowledgeLevels(route.params.id)
  sectionInfo.value = xpInfo.getExperienceInfoForSection(XpSectionTypes.knowledges)
})

const closeDialog = () => {
  dialogRef.value.close()
}

const onSubmit = form.handleSubmit(async (values) => {
  await store.addSpecialization(values, route.params.id, knowledge.value.mappingId)
  closeDialog()
})

</script>

<template>
  <h1 class="pt-0 mt-0">
    {{ knowledge.knowledge.name }}
  </h1>
  <h3>{{ knowledge.knowledge.type }}</h3>
  <p>{{ knowledge.knowledge.description }}</p>

  <ShowXPCosts :section-type="XpSectionTypes.knowledges" />

  <Message v-if="sectionInfo.availableXp < 2" severity="warn" class="my-3">
    You do not have enough experience to add a specialization (2xp)
  </Message>

  <form @submit="onSubmit">
    <FormInputTextWrapper v-model="form.name" :disabled="sectionInfo.availableXp < 2" />

    <FormTextAreaWrapper v-model="form.description" :disabled="sectionInfo.availableXp < 2" />

    <FormTextAreaWrapper v-model="form.notes" :disabled="sectionInfo.availableXp < 2" />

    <div class="m-3 text-right">
      <Button label="Cancel" class="m-2" type="reset" @click="closeDialog()" />
      <Button label="Add" class="m-2" type="submit" :disabled="sectionInfo.availableXp < 2" />
    </div>
  </form>
</template>
