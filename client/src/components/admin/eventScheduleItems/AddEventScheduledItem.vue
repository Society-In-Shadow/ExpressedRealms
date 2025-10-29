<script setup lang="ts">

import { EventScheduleItemStore } from '@/components/admin/eventScheduleItems/stores/eventScheduleItemStore'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import { getValidationInstance } from '@/components/admin/eventScheduleItems/validations/eventScheduleItemValidations'
import Button from 'primevue/button'
import Card from 'primevue/card'
import FormInputDateOnlyWrapper from '@/FormWrappers/FormInputDateOnlyWrapper.vue'
import FormInputTimeOnlyWrapper from '@/FormWrappers/FormInputTimeOnlyWrapper.vue'
import type { PropType } from 'vue'
import type { Event } from '@/components/admin/events/types.ts'

const store = EventScheduleItemStore()

const form = getValidationInstance()
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
})

const onSubmit = form.handleSubmit(async (values) => {
  await store.addEventScheduleItem(props.eventId, values)
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
        <FormInputTextWrapper v-model="form.fields.description" />
        <FormInputDateOnlyWrapper v-model="form.fields.date" :max-date="props.event.endDate" :min-date="props.event.startDate" />
        <div class="d-flex flex-row gap-2 w-100">
          <FormInputTimeOnlyWrapper v-model="form.fields.startTime" />
          <FormInputTimeOnlyWrapper v-model="form.fields.endTime" />
        </div>
        <div class="m-3 text-right">
          <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
          <Button label="Add" class="m-2" type="submit" />
        </div>
      </form>
    </template>
  </Card>
</template>
