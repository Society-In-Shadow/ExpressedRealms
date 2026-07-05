<script setup lang="ts">

import { useForm } from 'vee-validate'
import { object, string } from 'yup'
import InputTextWrapper from '@/FormWrappers/InputTextWrapper.vue'
import { inject, ref } from 'vue'
import axios from 'axios'
import toaster from '@/services/Toasters'
import { cmsStore } from '@/stores/cmsStore.ts'
import Button from 'primevue/button'
import { useRouter } from 'vue-router'

const cmsData = cmsStore()
const router = useRouter()

const dialogRef = inject('dialogRef')
const expressionId = ref(dialogRef.value.data.expressionId)

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    name: string().required()
      .max(50)
      .label('Name'),
  }),
})

const [name] = defineField('name')

const onSubmit = handleSubmit((values) => {
  axios.post(`/expression/${expressionId.value}/copy`, {
    name: values.name,
  }).then(async (response) => {
    await cmsData.refreshCmsInformation()
    const slug = cmsData.expressionItems.filter(x => x.id == response.data)[0].slug
    router.push('/expressions/' + slug)
    toaster.success('Successfully Copied Expression!')
    dialogRef.value.close()
  })
})

</script>

<template>
  <form @submit="onSubmit">
    <InputTextWrapper v-model="name" field-name="New Expression Name" :error-text="errors.name" />
    <Button label="Save" class="w-100 mb-2" type="submit" />
  </form>
</template>
