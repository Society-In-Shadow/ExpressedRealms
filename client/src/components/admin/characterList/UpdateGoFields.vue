<script setup lang="ts">
import toaster from '@/services/Toasters'

import { inject, ref, type Ref } from 'vue'
import FormInputNumberWrapper from '@/FormWrappers/FormInputNumberWrapper.vue'
import Button from 'primevue/button'
import { getValidationInstance } from '@/components/admin/characterList/validators/characterGoFieldsForm.ts'
import FormWrapper from '@/FormWrappers/FormWrapper.vue'
import { useQuery } from '@pinia/colada'
import { goFieldQuery, useUpdateGoFields } from '@/components/admin/characterList/stores/goFieldColada.ts'
import type { GoFields } from '@/components/admin/characterList/types.ts'
import { useHydrateFormOnce } from '@/utilities/piniaColadaUtilities.ts'
import { SourceTableEnum } from '@/components/modifiergroups/types.ts'
import ModifierGroup from '@/components/modifiergroups/ModifierGroup.vue'
import { can } from '@/stores/userPermissionStore.ts'

const form = getValidationInstance()
const dialogRef = inject('dialogRef') as Ref
const characterId = ref(dialogRef.value.data.characterId as number)

const { data, isPending } = useQuery(goFieldQuery(characterId.value))
useHydrateFormOnce(data, form.setValues)

const updateGoFields = useUpdateGoFields((errors) => {
  form.setErrors(errors)
})

const onSubmit = form.handleSubmit(async (values) => {
  await updateGoFields.mutateAsync({
    id: characterId.value,
    data: values as GoFields,
  })
  toaster.success('Go fields updated successfully')
  cancel()
})

const cancel = () => {
  dialogRef.value.close()
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

  <div v-if="can.CharacterManagement.EditModifiers">
    <p>For the modifiers below, these are permanent additions to the character sheet recieved via role play.</p>
    <p>This is not meant to be used to get around bugs in the app</p>
    <ModifierGroup v-if="!isPending" :group-id="data!.statModifierGroupId" :source="SourceTableEnum.Characters" :source-id="characterId" />
  </div>
</template>
