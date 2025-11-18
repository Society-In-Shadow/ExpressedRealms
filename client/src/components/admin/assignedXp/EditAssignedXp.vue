<script setup lang="ts">

import { onBeforeMount, ref } from 'vue'
import { AssignedXpStore } from './stores/assignedXpStore.ts'
import FormDropdownWrapper from '@/FormWrappers/FormDropdownWrapper.vue'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import { getValidationInstance } from '@/components/admin/assignedXp/validations/assignedXpValidations'
import type { EditEvent } from '@/components/admin/events/types'
import Button from 'primevue/button'
import FormInputNumberWrapper from '@/FormWrappers/FormInputNumberWrapper.vue'

const store = AssignedXpStore()

const form = getValidationInstance()
const event = ref<EditEvent>()
const emit = defineEmits<{
  canceled: []
}>()

const props = defineProps({
  characterId: {
    type: Number,
    required: true,
  },
  eventId: {
    type: Number,
    required: true,
  },
})

onBeforeMount(async () => {
  event.value = await store.getEvent(props.eventId)
  form.setValues(event.value)
})

const onSubmit = form.handleSubmit(async (values) => {
  await store.updateEvent(values, props.eventId)
  cancel()
})

const cancel = () => {
  emit('canceled')
}

</script>

<template>
  <form @submit="onSubmit">
    <FormInputTextWrapper v-model="form.fields.notes" />
    <FormInputNumberWrapper v-model="form.fields.amount" />

    <FormDropdownWrapper
      v-model="form.fields.event"
      :options="store.events"
      option-label="name"
    />

    <!--        <FormDropdownWrapper
      v-model="form.fields.xpType"
      :options="store.timeZones"
      option-label="name"
    />-->

    <div class="m-3 text-right">
      <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
      <Button label="Update" class="m-2" type="submit" />
    </div>
  </form>
</template>
