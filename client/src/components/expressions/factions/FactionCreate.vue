<script setup lang="ts">
import toaster from '@/services/Toasters'

import { computed, inject, onMounted, type Ref } from 'vue'
import Button from 'primevue/button'
import FormWrapper from '@/FormWrappers/FormWrapper.vue'
import type { CreateSingleFactionPost } from '@/components/expressions/factions/types.ts'
import { factionCreate } from '@/components/expressions/factions/stores/factionStore.ts'
import { getValidationInstance } from '@/components/expressions/factions/validators/factionCreateValidator.ts'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import { expressionStore } from '@/stores/expressionStore.ts'
import FormEditorWrapper from '@/FormWrappers/FormEditorWrapper.vue'
import { knowledgeStore } from '@/components/knowledges/stores/knowledgeStore.ts'
import FormDropdownWrapper from '@/FormWrappers/FormDropdownWrapper.vue'
import type { ListItem } from '@/types/ListItem.ts'

const expressionInfo = expressionStore()
const knowledgeInfo = knowledgeStore()

const form = getValidationInstance()
const dialogRef = inject('dialogRef') as Ref

onMounted(() => {
  knowledgeInfo.getKnowledges()
})

const knowledgeValues = computed(() => knowledgeInfo.knowledges.map<ListItem>(knowledge => ({
  id: knowledge.id,
  name: knowledge.name,
  description: knowledge.description,
} as ListItem)))

const updateFactionFields = factionCreate((errors) => {
  form.setErrors(errors)
})

const onSubmit = form.handleSubmit(async (values) => {
  await updateFactionFields.mutateAsync({
    data: {
      name: values.name,
      background: values.background,
      knowledgeId: values.knowledge.id,
      specialization: values.specialization,
      expressionId: expressionInfo.currentExpressionId,
    } as CreateSingleFactionPost,
  })
  toaster.success('Faction fields updated successfully')
  cancel()
})

const cancel = () => {
  dialogRef.value.close()
}

</script>

<template>
  <FormWrapper @submit="onSubmit">
    <FormInputTextWrapper v-model="form.fields.name" />
    <FormEditorWrapper v-model="form.fields.background" />
    <p>
      Creating the faction will automatically populate the Faction's levels.  Specify the knowledge and specialization below
      that are required for given levels
    </p>
    <FormDropdownWrapper
      v-model="form.fields.knowledge"
      :options="knowledgeValues"
      option-label="name"
    />
    <FormInputTextWrapper v-model="form.fields.specialization" />
    <Button label="Add" class="m-2" type="submit" />
  </FormWrapper>
</template>
