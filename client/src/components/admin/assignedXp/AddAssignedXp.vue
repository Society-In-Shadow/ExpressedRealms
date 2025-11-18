<script setup lang="ts">

import { AssignedXpStore } from './stores/assignedXpStore.ts'
import FormDropdownWrapper from '@/FormWrappers/FormDropdownWrapper.vue'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import { getValidationInstance } from './validations/assignedXpValidations.ts'
import Button from 'primevue/button'
import Card from 'primevue/card'
import FormInputNumberWrapper from '@/FormWrappers/FormInputNumberWrapper.vue'

const store = AssignedXpStore()

const form = getValidationInstance()
const emit = defineEmits<{
  canceled: []
}>()

const props = defineProps({
  characterId: {
    type: Number,
    required: true,
  },
})

const onSubmit = form.handleSubmit(async (values) => {
  await store.addEvent(values, props.characterId)
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

        <FormInputNumberWrapper v-model="form.fields.amount" />

        <FormInputTextWrapper v-model="form.fields.notes" />

        <div class="m-3 text-right">
          <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
          <Button label="Add" class="m-2" type="submit" />
        </div>
      </form>
    </template>
  </Card>
</template>
