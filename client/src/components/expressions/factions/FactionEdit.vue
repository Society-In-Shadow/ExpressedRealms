<script setup lang="ts">
import toaster from '@/services/Toasters'

import { inject, ref, type Ref } from 'vue'
import Button from 'primevue/button'
import FormWrapper from '@/FormWrappers/FormWrapper.vue'
import { useQuery } from '@pinia/colada'
import { useHydrateFormOnce } from '@/utilities/piniaColadaUtilities.ts'
import type { EditSingleFactionInfo } from '@/components/expressions/factions/types.ts'
import { factionQuery, factionUpdate } from '@/components/expressions/factions/stores/factionStore.ts'
import { getValidationInstance } from '@/components/expressions/factions/validators/factionValidator.ts'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'

const form = getValidationInstance()
const dialogRef = inject('dialogRef') as Ref
const factionId = ref(dialogRef.value.data.factionId as number)

const { data, isPending } = useQuery(factionQuery(factionId.value))
useHydrateFormOnce(data, form.setValues)

const updateFactionFields = factionUpdate((errors) => {
  form.setErrors(errors)
})

const onSubmit = form.handleSubmit(async (values) => {
  await updateFactionFields.mutateAsync({
    id: factionId.value,
    data: values as EditSingleFactionInfo,
  })
  toaster.success('Faction fields updated successfully')
  cancel()
})

const cancel = () => {
  dialogRef.value.close()
}

</script>

<template>
  <FormWrapper :show-skeleton="isPending" @submit="onSubmit">
    <FormInputTextWrapper v-model="form.fields.name" />
    <FormInputTextWrapper v-model="form.fields.background" />
    <Button label="Update" class="m-2" type="submit" />
  </FormWrapper>
</template>
