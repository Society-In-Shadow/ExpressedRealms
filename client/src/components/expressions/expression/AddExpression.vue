<script setup lang="ts">

import toaster from '@/services/Toasters'
import Button from 'primevue/button'
import { useRouter } from 'vue-router'

import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import FormTextAreaWrapper from '@/FormWrappers/FormTextAreaWrapper.vue'
import { inject, onMounted, ref } from 'vue'
import { getValidationInstance } from '@/components/expressions/expression/validators/addExpressionValidation.ts'
import { cmsItemsQuery, create } from '@/components/expressions/expression/stores/cmsStore.ts'
import { useQueryWithLoading } from '@/utilities/queryOverride.ts'
import { useQueryCache } from '@pinia/colada'
import { characterListQuery } from '@/components/navbar/stores/navMenuStore.ts'

const router = useRouter()

const dialogRef = inject('dialogRef')
const expressionTypeId = ref(dialogRef.value.data.expressionTypeId)

const form = getValidationInstance()

const createItem = create((errors) => {
  form.setErrors(errors)
})

const { data } = useQueryWithLoading(cmsItemsQuery())
const queryCache = useQueryCache()

onMounted(() => {
  form.setValues(null)
})

const onSubmit = form.handleSubmit(async (values) => {
  const addedItemId = await createItem.mutateAsync({
    data: {
      name: values.name,
      navMenuImage: values.navMenuImage,
      shortDescription: values.shortDescription,
      expressionTypeId: expressionTypeId.value,
    } },
  )

  await queryCache.refresh(queryCache.ensure(characterListQuery))

  let slug = ''
  switch (expressionTypeId.value) {
    case 1:
      toaster.success(`Successfully added ${values.name} Expression as a Draft!`)
      slug = data.value!.expressionItems.find(x => x.id == addedItemId)!.slug
      router.push('/expressions/' + slug)
      break
    case 13:
      toaster.success(`Successfully added ${values.name} Rule Book Section as a Draft!`)
      slug = data.value!.rulebookItems.find(x => x.id == addedItemId)!.slug
      await router.push('/rulebook/' + slug)
      break
    case 14:
      toaster.success(`Successfully added ${values.name} World Background Section as a Draft!`)
      slug = data.value!.worldBackgroundItems.find(x => x.id == addedItemId)!.slug
      router.push('/worldbackground/' + slug)
      break
  }
  dialogRef.value.close()
})

</script>

<template>
  <form @submit="onSubmit">
    <FormInputTextWrapper v-model="form.fields.name" />
    <FormTextAreaWrapper v-model="form.fields.shortDescription" />
    <div class="d-flex align-items-center gap-3 ">
      <div class="flex-shrink-1">
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

    <Button data-cy="add-expression-button" label="Add" class="w-100 mb-2" type="submit" />
  </form>
</template>
