<script setup lang="ts">

import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import Button from 'primevue/button'
import Card from 'primevue/card'
import { getValidationInstance } from '@/components/admin/eventQuestions/validations/eventQuestionValidation.ts'
import FormDropdownWrapper from '@/FormWrappers/FormDropdownWrapper.vue'
import { EventQuestionStore } from '@/components/admin/eventQuestions/stores/eventQuestionStore.ts'

const store = EventQuestionStore()

const form = getValidationInstance()
const emit = defineEmits<{
  canceled: []
}>()

const props = defineProps({
  eventId: {
    type: Number,
    required: true,
  },
})

const onSubmit = form.handleSubmit(async (values) => {
  await store.addItem(props.eventId, values)
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
        <FormInputTextWrapper v-model="form.fields.question" />
        <FormDropdownWrapper v-model="form.fields.questionType" :options="store.questionTypes" option-label="name" />
        <div class="m-3 text-right">
          <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
          <Button label="Add" class="m-2" type="submit" />
        </div>
      </form>
    </template>
  </Card>
</template>
