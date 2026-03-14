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
import { userPermissionStore } from '@/stores/userPermissionStore.ts'
import FormCheckboxWrapper from '@/FormWrappers/FormCheckboxWrapper.vue'

const userPermissionInfo = userPermissionStore()
const permissionCheck = userPermissionInfo.permissionCheck
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
  if (!permissionCheck.Event.Edit) {
    return
  }
  await store.updateEvent(values, props.eventId)
})

</script>

<template>
  <FormWrapper :show-skeleton="!event" :is-disabled="!permissionCheck.Event.Edit || props.eventId == 1" @submit="onSubmit">
    <FormInputTextWrapper v-model="form.fields.name" />
    <FormInputTextWrapper v-model="form.fields.location" />
    <FormInputTextWrapper v-model="form.fields.websiteName" />
    <FormInputTextWrapper v-model="form.fields.websiteUrl" />
    <FormInputTextWrapper v-model="form.fields.additionalNotes" />
    <FormInputNumberWrapper v-model="form.fields.conExperience" />
    <div class="d-flex flex-column flex-md-row gap-2 w-100">
      <FormInputDateOnlyWrapper v-model="form.fields.startDate" />
      <FormInputDateOnlyWrapper v-model="form.fields.endDate" />
    </div>

    <FormDropdownWrapper
      v-model="form.fields.timeZone"
      :options="store.timeZones"
      option-label="name"
    />

    <FormCheckboxWrapper
      v-model="form.fields.collectAttendeeInformation"
    />

    <div class="m-3 text-right">
      <Button v-if="permissionCheck.Event.Edit && props.eventId != 1" label="Update" class="m-2" type="submit" />
    </div>
  </FormWrapper>
</template>
