<script setup lang="ts">

import { onMounted, ref } from 'vue'
import { EventStore } from '@/components/admin/events/stores/eventStore'
import FormDropdownWrapper from '@/FormWrappers/FormDropdownWrapper.vue'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import { getValidationInstance } from '@/components/admin/events/validations/eventValidations'
import type { EditEvent } from '@/components/admin/events/types'
import Button from 'primevue/button'
import FormInputDateOnlyWrapper from '@/FormWrappers/FormInputDateOnlyWrapper.vue'
import FormInputNumberWrapper from '@/FormWrappers/FormInputNumberWrapper.vue'
import FormWrapper from '@/FormWrappers/FormWrapper.vue'

const store = EventStore()

const form = getValidationInstance()
const event = ref<EditEvent>()

const props = defineProps({
  eventId: {
    type: Number,
    required: true,
  },
})

onMounted(async () => {
  event.value = await store.getEvent(props.eventId)
  form.setValues(event.value)
})

const onSubmit = form.handleSubmit(async (values) => {
  await store.updateEvent(values, props.eventId)
})

</script>

<template>
  <FormWrapper :show-skeleton="!event" @submit="onSubmit">
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
      <Button label="Update" class="m-2" type="submit" />
    </div>
  </FormWrapper>
</template>
