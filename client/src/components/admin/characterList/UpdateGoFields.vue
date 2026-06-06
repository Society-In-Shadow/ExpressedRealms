<script setup lang="ts">

import { inject, ref, type Ref, watch } from 'vue'
import FormInputNumberWrapper from '@/FormWrappers/FormInputNumberWrapper.vue'
import Button from 'primevue/button'
import { getValidationInstance } from '@/components/admin/characterList/validators/characterGoFieldsForm.ts'
import FormWrapper from '@/FormWrappers/FormWrapper.vue'
import { useQuery } from '@pinia/colada'
import { goFieldQuery, useUpdateGoFields } from '@/components/admin/characterList/stores/goFieldColada.ts'
import type { GoFields } from '@/components/admin/characterList/types.ts'

const form = getValidationInstance()
const dialogRef = inject('dialogRef') as Ref
const characterId = ref(dialogRef.value.data.characterId as number)

const { data, isPending } = useQuery(goFieldQuery(characterId.value))
const updateGoFields = useUpdateGoFields()

useHydrateFormOnce(data, form.setValues)

const onSubmit = form.handleSubmit(async (values) => {
  await updateGoFields.mutateAsync({
    id: characterId.value,
    data: {
      wealthLevel: values.wealthLevel,
      motes: values.motes,
      primaFragments: values.primaFragments,
      voidFragments: values.voidFragments,
    } as GoFields })
  cancel()
})

const cancel = () => {
  dialogRef.value.close()
}

function useHydrateFormOnce<T>(
  source: Ref<T | undefined>,
  hydrate: (value: T) => void,
) {
  const hydrated = ref(false)

  watch(
    source,
    (val) => {
      if (!val || hydrated.value) return

      hydrate(val)
      hydrated.value = true
    },
    { immediate: true },
  )

  return {
    resetHydration: () => {
      hydrated.value = false
    },
  }
}

</script>

<template>
  <FormWrapper :show-skeleton="isPending" @submit="onSubmit">
    <FormInputNumberWrapper v-model="form.fields.wealthLevel" />
    <FormInputNumberWrapper v-model="form.fields.motes" />
    <FormInputNumberWrapper v-model="form.fields.primaFragments" />
    <FormInputNumberWrapper v-model="form.fields.voidFragments" />
    <Button label="Update" class="m-2" type="submit" />
  </FormWrapper>
</template>
