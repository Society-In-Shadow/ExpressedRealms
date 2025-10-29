<script setup lang="ts">

import { EventStore } from '@/components/admin/events/stores/eventStore'
import FormDropdownWrapper from '@/FormWrappers/FormDropdownWrapper.vue'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import { getValidationInstance } from '@/components/admin/events/validations/eventValidations'
import Button from 'primevue/button'
import Card from 'primevue/card'
import FormInputDateOnlyWrapper from '@/FormWrappers/FormInputDateOnlyWrapper.vue'
import FormInputNumberWrapper from '@/FormWrappers/FormInputNumberWrapper.vue'

const store = EventStore()

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
        <FormInputTextWrapper v-model="form.fields.location" />
        <FormInputTextWrapper v-model="form.fields.websiteName" />
        <FormInputTextWrapper v-model="form.fields.websiteUrl" />
        <FormInputTextWrapper v-model="form.fields.additionalNotes" />
        <FormInputNumberWrapper v-model="form.fields.conExperience" />

        <div class="d-flex flex-row gap-2 w-100">
          <FormInputDateOnlyWrapper v-model="form.fields.startDate" />
          <FormInputDateOnlyWrapper v-model="form.fields.endDate" />
        </div>
        <FormDropdownWrapper
          v-model="form.fields.timeZone"
          :options="store.timeZones"
          option-label="name"
        />

        <div class="m-3 text-right">
          <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
          <Button label="Add" class="m-2" type="submit" />
        </div>
      </form>
    </template>
  </Card>
</template>
