<script setup lang="ts">
import toaster from '@/services/Toasters'

import { inject, type Ref } from 'vue'
import Button from 'primevue/button'
import FormWrapper from '@/FormWrappers/FormWrapper.vue'
import type { CreateSingleFactionInfo } from '@/components/expressions/factions/types.ts'
import { factionCreate } from '@/components/expressions/factions/stores/factionStore.ts'
import { getValidationInstance } from '@/components/expressions/factions/validators/factionValidator.ts'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import { expressionStore } from '@/stores/expressionStore.ts'
import FormEditorWrapper from '@/FormWrappers/FormEditorWrapper.vue'

const expressionInfo = expressionStore()
const form = getValidationInstance()
const dialogRef = inject('dialogRef') as Ref

const updateFactionFields = factionCreate((errors) => {
  form.setErrors(errors)
})

const onSubmit = form.handleSubmit(async (values) => {
  await updateFactionFields.mutateAsync({
    data: {
      ...values,
      expressionId: expressionInfo.currentExpressionId,
    } as CreateSingleFactionInfo,
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
    <Button label="Update" class="m-2" type="submit" />
  </FormWrapper>
</template>
