<script setup lang="ts">

import { onBeforeMount, type PropType, ref } from 'vue'
import { EventScheduleItemStore } from '@/components/admin/eventScheduleItems/stores/eventScheduleItemStore'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import { getValidationInstance } from '@/components/admin/eventScheduleItems/validations/eventScheduleItemValidations'
import type { EditEventScheduleItem } from '@/components/admin/eventScheduleItems/types'
import Button from 'primevue/button'
import FormInputDateOnlyWrapper from '@/FormWrappers/FormInputDateOnlyWrapper.vue'
import FormInputTimeOnlyWrapper from '@/FormWrappers/FormInputTimeOnlyWrapper.vue'
import type { Event } from '@/components/admin/events/types.ts'

const store = EventScheduleItemStore()

const form = getValidationInstance()
const EventScheduleItem = ref<EditEventScheduleItem>()
const emit = defineEmits<{
  canceled: []
}>()

const props = defineProps({
  eventId: {
    type: Number,
    required: true,
  },
  event: {
    type: Object as PropType<Event>,
    required: true,
  },
  eventScheduleItemId: {
    type: Number,
    required: true,
  },
})

onBeforeMount(async () => {
  EventScheduleItem.value = await store.getEventScheduleItem(props.eventId, props.eventScheduleItemId)
  form.setValues(EventScheduleItem.value)
})

const onSubmit = form.handleSubmit(async (values) => {
  await store.updateEventScheduleItem(props.eventId, values, props.eventScheduleItemId)
  cancel()
})

const cancel = () => {
  emit('canceled')
}

</script>

<template>
  <form @submit="onSubmit">
    <FormInputTextWrapper v-model="form.fields.description" />
    <FormInputDateOnlyWrapper v-model="form.fields.date" :max-date="props.event.endDate" :min-date="props.event.startDate" />
    <div class="d-flex flex-row gap-2 w-100">
      <FormInputTimeOnlyWrapper v-model="form.fields.startTime" />
      <FormInputTimeOnlyWrapper v-model="form.fields.endTime" />
    </div>
    <div class="m-3 text-right">
      <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
      <Button label="Update" class="m-2" type="submit" />
    </div>
  </form>
</template>
