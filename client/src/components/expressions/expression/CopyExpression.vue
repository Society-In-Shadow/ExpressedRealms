<script setup lang="ts">

import { inject, ref } from 'vue'
import toaster from '@/services/Toasters'
import Button from 'primevue/button'
import { useRouter } from 'vue-router'
import { getValidationInstance } from '@/components/expressions/expression/validators/copyExpressionValidation.ts'
import FormWrapper from '@/FormWrappers/FormWrapper.vue'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import { cmsItemsQuery, copy } from '@/components/expressions/expression/stores/cmsStore.ts'
import { characterListQuery } from '@/components/navbar/stores/navMenuStore.ts'
import { useQueryWithLoading } from '@/utilities/queryOverride.ts'
import { useQueryCache } from '@pinia/colada'

const router = useRouter()

const dialogRef = inject('dialogRef')
const expressionId = ref(dialogRef.value.data.expressionId)

const form = getValidationInstance()

const copyItem = copy((errors) => {
  form.setErrors(errors)
})

const { data } = useQueryWithLoading(cmsItemsQuery())
const queryCache = useQueryCache()

const onSubmit = form.handleSubmit(async (values) => {
  const copiedItemId = await copyItem.mutateAsync({ id: expressionId.value, data: {
    name: values.name,
  } })

  await queryCache.refresh(queryCache.ensure(characterListQuery))

  toaster.success(`Successfully copied ${values.name} Expression as a Draft!`)
  let slug = data.value!.expressionItems.find(x => x.id == copiedItemId)!.slug
  router.push('/expressions/' + slug)

  dialogRef.value.close()
})

</script>

<template>
  <FormWrapper @submit="onSubmit">
    <FormInputTextWrapper v-model="form.fields.name" />
    <Button label="Save" class="w-100 mb-2" type="submit" />
  </FormWrapper>
</template>
