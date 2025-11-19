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
  id: {
    type: Number,
    required: true,
  },
  characterId: {
    type: Number,
    required: true,
  },
})

onBeforeMount(async () => {
  event.value = await store.getEvent(props.id)
  form.setValues(event.value)
})

const onSubmit = form.handleSubmit(async (values) => {
  await store.update(values, props.id, props.characterId)
  cancel()
})

const cancel = () => {
  emit('canceled')
}

</script>

<template>
  <form @submit="onSubmit">
    <div class="d-flex flex-row gap-2 w-100">
      <FormDropdownWrapper
        v-model="form.fields.xpType"
        :options="store.xpTypes"
        option-label="name"
      />

      <FormInputNumberWrapper v-model="form.fields.amount" />
    </div>

    <FormDropdownWrapper
      v-model="form.fields.event"
      :options="store.events"
      option-label="name"
    />
    <FormInputTextWrapper v-model="form.fields.notes" />

    <div class="m-3 text-right">
      <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
      <Button label="Update" class="m-2" type="submit" />
    </div>
  </form>
</template>
