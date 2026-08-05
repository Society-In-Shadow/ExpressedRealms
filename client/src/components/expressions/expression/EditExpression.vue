<script setup lang="ts">

import { inject, ref } from 'vue'
import toaster from '@/services/Toasters'
import Button from 'primevue/button'
import { getValidationInstance } from '@/components/expressions/expression/validators/editExpressionValidation.ts'
import { cmsItemQuery, edit } from '@/components/expressions/expression/stores/cmsStore.ts'
import FormWrapper from '@/FormWrappers/FormWrapper.vue'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import FormTextAreaWrapper from '@/FormWrappers/FormTextAreaWrapper.vue'
import FormInputNumberWrapper from '@/FormWrappers/FormInputNumberWrapper.vue'
import { useQuery } from '@pinia/colada'
import { useHydrateFormOnce } from '@/utilities/piniaColadaUtilities.ts'
import FormDropdownWrapper from '@/FormWrappers/FormDropdownWrapper.vue'

const dialogRef = inject('dialogRef')
const expressionId = ref(dialogRef.value.data.expressionId)

const form = getValidationInstance()

const { data, isPending } = useQuery(cmsItemQuery(expressionId.value))
useHydrateFormOnce(data, form.setValues)

const editItem = edit((errors) => {
  form.setErrors(errors)
})

const onSubmit = form.handleSubmit(async (values) => {
  await editItem.mutateAsync({ id: expressionId.value, data: {
    name: values.name,
    shortDescription: values.shortDescription,
    publishStatus: values.publishStatus.id,
    navMenuImage: values.navMenuImage,
    sortOrder: Number(values.sortOrder),
  } })
  toaster.success('Successfully Updated Expression Info!')
})

</script>

<template>
  <FormWrapper :show-skeleton="isPending" @submit="onSubmit">
    <FormInputTextWrapper v-model="form.fields.name" />
    <FormTextAreaWrapper v-model="form.fields.shortDescription" />

    <div class="d-flex align-items-center gap-3 ">
      <div v-if="!isPending" class="flex-shrink-1">
        <span class="inline-flex flex-none align-items-center justify-content-center border-circle bg-primary w-3rem h-3rem">
          <i :class="['material-symbols-outlined', 'text-white']"> {{ form.fields.navMenuImage.field.value }}</i>
        </span>
      </div>
      <div class="flex-grow-1">
        <FormInputTextWrapper v-model="form.fields.navMenuImage" />
      </div>
    </div>
    <p>List of icons can be found here : <a href="https://fonts.google.com/icons?icon.size=24&icon.color=%23e3e3e3">Google Material Design Fonts</a></p>
    <p>You only need to add the name of the icon, with spaces being replaced with underlines.</p>
    <FormDropdownWrapper v-model="form.fields.publishStatus" :options="data?.publishTypes ?? []" option-label="name" />
    <FormInputNumberWrapper v-model="form.fields.sortOrder" />
    <p>Keep in mind, for 6 items, sort order is first column starts at one, and ends at 3, and 2nd column starts at 4 and ends at 6</p>
    <Button label="Save" class="w-100 mb-2" type="submit" />
  </FormWrapper>
</template>
