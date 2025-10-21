<script setup lang="ts">

import {useForm} from 'vee-validate'
import {number, object, string} from 'yup'
import InputTextWrapper from '@/FormWrappers/InputTextWrapper.vue'
import TextAreaWrapper from '@/FormWrappers/TextAreaWrapper.vue'
import DropdownWrapper from '@/FormWrappers/DropdownWrapper.vue'
import {inject, onMounted, ref} from 'vue'
import axios from 'axios'
import toaster from '@/services/Toasters'
import {cmsStore} from '@/stores/cmsStore.ts'
import Button from 'primevue/button'

const isLoading = ref(true)
const publishStatusOptions = ref([])
const cmsData = cmsStore()

const dialogRef = inject('dialogRef')
const expressionId = ref(dialogRef.value.data.expressionId)

onMounted(() => {
  axios.get(`/expression/${expressionId.value}`)
    .then((response) => {
      name.value = response.data.name
      shortDescription.value = response.data.shortDescription
      navMenuImage.value = response.data.navMenuImage
      publishStatus.value = response.data.publishTypes.find(x => x.id == response.data.publishStatus)
      publishStatusOptions.value = response.data.publishTypes
      sortOrder.value = response.data.sortOrder
      isLoading.value = false
    })
})

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    name: string().required()
      .max(50)
      .label('Name'),
    shortDescription: string()
      .required()
      .max(125)
      .label('Short Description'),
    navMenuImage: string().required()
      .label('Nav Menu Icon'),
    publishStatus: object().required()
      .label('Publish Status'),
    sortOrder: number().required()
      .label('Sort Order'),
  }),
})

const [name] = defineField('name')
const [shortDescription] = defineField('shortDescription')
const [navMenuImage] = defineField('navMenuImage')
const [publishStatus] = defineField('publishStatus')
const [sortOrder] = defineField('sortOrder')

const onSubmit = handleSubmit((values) => {
  axios.put(`/expression/${expressionId.value}`, {
    name: values.name,
    shortDescription: values.shortDescription,
    id: expressionId.value,
    publishStatus: values.publishStatus.id,
    navMenuImage: values.navMenuImage,
    sortOrder: Number(values.sortOrder),
  }).then(() => {
    cmsData.refreshCmsInformation()
    toaster.success('Successfully Updated Expression Info!')
  })
})

</script>

<template>
  <form @submit="onSubmit">
    <InputTextWrapper v-model="name" field-name="Name" :error-text="errors.name" :show-skeleton="isLoading" />
    <TextAreaWrapper v-model="shortDescription" field-name="Short Description" :error-text="errors.shortDescription" :show-skeleton="isLoading" />

    <div class="d-flex align-items-center gap-3 ">
      <div class="flex-shrink-1">
        <span class="inline-flex flex-none align-items-center justify-content-center border-circle bg-primary w-3rem h-3rem">
          <i :class="['material-symbols-outlined', 'text-white']"> {{ navMenuImage }}</i>
        </span>
      </div>
      <div class="flex-grow-1">
        <InputTextWrapper v-model="navMenuImage" field-name="Nav Menu Icon" :error-text="errors.navMenuImage" :show-skeleton="isLoading" />
      </div>
    </div>
    <p>List of icons can be found here : <a href="https://fonts.google.com/icons?icon.size=24&icon.color=%23e3e3e3">Google Material Design Fonts</a></p>
    <p>You only need to add the name of the icon, with spaces being replaced with underlines.</p>
    <DropdownWrapper
      v-model="publishStatus" option-label="name" :options="publishStatusOptions" field-name="Publish Status" :error-text="errors.publishStatus"
      :show-skeleton="isLoading"
    />
    <InputTextWrapper v-model="sortOrder" field-name="Sort Order" :error-text="errors.sortOrder" :show-skeleton="isLoading" />
    <p>Keep in mind, for 6 items, sort order is first column starts at one, and ends at 3, and 2nd column starts at 4 and ends at 6</p>
    <Button label="Save" class="w-100 mb-2" type="submit" />
  </form>
</template>
