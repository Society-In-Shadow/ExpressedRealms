<script setup lang="ts">

import Button from 'primevue/button'
import axios from 'axios'
import { useRouter } from 'vue-router'
import { useForm } from 'vee-validate'
import { object, string } from 'yup'
import { logOff } from '@/services/Authentication'
import InputTextWrapper from '../../FormWrappers/InputTextWrapper.vue'
import { userInfoQuery } from '@/auth/authStore.ts'
import { useQueryWithLoading } from '@/utilities/queryOverride.ts'
import Message from 'primevue/message'

const Router = useRouter()
const { refetch } = useQueryWithLoading(userInfoQuery)

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    name: string().required()
      .max(100)
      .label('Name'),
  }),
})

const [name] = defineField('name')
const onSubmit = handleSubmit((values) => {
  axios.post('/player', values).then(async () => {
    await refetch()
    await Router.push({ name: 'characters' })
  })
})

</script>

<template>
  <form style="max-width: 30em" @submit="onSubmit">
    <div class="mb-3">
      <h1 class="mt-md-0 pt-md-0">
        User Profile Setup
      </h1>
    </div>
    <InputTextWrapper v-model="name" field-name="Preferred Name" :error-text="errors.name" />
    <Message class="mb-3">
      Your preferred name is a nick name or actual name, something that we can reliably get your attention within a crowd.
    </Message>
    <Button data-cy="update-profile-button" label="Update Profile" class="w-100 mb-2" type="submit" />
  </form>
  <Button data-cy="logoff-button" label="Logoff" class="w-100 mb-2" @click="logOff(Router)" />
</template>
