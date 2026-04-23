<script setup lang="ts">

import Button from 'primevue/button'
import axios from 'axios'
import { useForm } from 'vee-validate'
import { object, string } from 'yup'
import InputTextWrapper from '../../FormWrappers/InputTextWrapper.vue'
import Card from 'primevue/card'
import { onMounted, ref } from 'vue'
import toasters from '@/services/Toasters'
import { userInfoQuery } from '@/auth/authStore.ts'
import { useQueryWithLoading } from '@/utilities/queryOverride.ts'
import Message from 'primevue/message'

const { refresh } = useQueryWithLoading(userInfoQuery)
const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    name: string().required()
      .max(100)
      .label('Name'),
  }),
})

const [name] = defineField('name')
const isLoading = ref(true)

onMounted(() => {
  axios.get('/player')
    .then((response) => {
      name.value = response.data.name
      isLoading.value = false
    })
})

const onSubmit = handleSubmit(async (values) => {
  await axios.put('/player', values).then(async () => {
    await refresh()
  })
    .then(() => {
      toasters.success('Successfully Updated User Name!')
    })
})

</script>

<template>
  <Card class="mb-3" style="width: 390px">
    <template #title>
      User Profile
    </template>
    <template #content>
      <form @submit="onSubmit">
        <InputTextWrapper v-model="name" field-name="Preferred Name" :error-text="errors.name" :show-skeleton="isLoading" />
        <Message class="mb-3">
          Your preferred name is a nick name or actual name, something that we can reliably get your attention within a crowd.
        </Message>
        <Button data-cy="update-profile-button" label="Update Profile" class="w-100 mb-2" type="submit" :disabled="isLoading" />
      </form>
    </template>
  </Card>
</template>
