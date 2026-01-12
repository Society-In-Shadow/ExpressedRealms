<script setup lang="ts">

import { watch } from 'vue'
import { RoleStore } from './stores/roleStore'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import { getValidationInstance } from './validations/roleValidations'
import Button from 'primevue/button'
import FormWrapper from '@/FormWrappers/FormWrapper.vue'

const store = RoleStore()

const form = getValidationInstance()

watch(() => store.role, (role) => {
  if (!role) return

  form.setValues(store.role)
},
{ immediate: true },
)

const onSubmit = form.handleSubmit(async (values) => {
  await store.updateEvent()
  store.role.name = values.name
  store.role.description = values.description
})

</script>

<template>
  <FormWrapper :show-skeleton="!store.haveRole" @submit="onSubmit">
    <FormInputTextWrapper v-model="form.fields.name" />
    <FormInputTextWrapper v-model="form.fields.description" />

    <div class="m-3 text-right">
      <Button label="Update" class="m-2" type="submit" />
    </div>
  </FormWrapper>
</template>
