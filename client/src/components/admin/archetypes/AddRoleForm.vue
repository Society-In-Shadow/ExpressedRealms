<script setup lang="ts">

import { RoleStore } from '@/components/admin/roles/stores/roleStore'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import { getValidationInstance } from '@/components/admin/roles/validations/roleValidations'
import Button from 'primevue/button'
import Card from 'primevue/card'
import FormTextAreaWrapper from '@/FormWrappers/FormTextAreaWrapper.vue'

const store = RoleStore()

const form = getValidationInstance()
const emit = defineEmits<{
  canceled: []
}>()

const onSubmit = form.handleSubmit(async (values) => {
  await store.addEvent(values)
  cancel()
})

const cancel = () => {
  emit('canceled')
}

</script>

<template>
  <Card>
    <template #content>
      <form @submit="onSubmit">
        <FormInputTextWrapper v-model="form.fields.name" />
        <FormTextAreaWrapper v-model="form.fields.description" />

        <div class="m-3 text-right">
          <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
          <Button label="Add" class="m-2" type="submit" />
        </div>
      </form>
    </template>
  </Card>
</template>
