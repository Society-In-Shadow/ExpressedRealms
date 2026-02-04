<script setup lang="ts">

import { onBeforeMount, type PropType } from 'vue'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import Button from 'primevue/button'
import FormDropdownWrapper from '@/FormWrappers/FormDropdownWrapper.vue'
import { EventQuestionStore } from '@/components/admin/eventQuestions/stores/eventQuestionStore.ts'
import { getValidationInstance } from '@/components/admin/eventQuestions/validations/eventQuestionValidation.ts'
import type { Question } from '@/components/admin/eventQuestions/types.ts'

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
  question: {
    type: Object as PropType<Question>,
    required: true,
  },
})

onBeforeMount(async () => {
  form.setValues(props.question)
})

const onSubmit = form.handleSubmit(async (values) => {
  await store.updateItem(props.eventId, values, props.question.id)
  cancel()
})

const cancel = () => {
  emit('canceled')
}

</script>

<template>
  <form @submit="onSubmit">
    <FormInputTextWrapper v-model="form.fields.question" />
    <FormDropdownWrapper v-model="form.fields.questionType" :options="store.questionTypes" option-label="name" :is-disabled="true" />
    <div class="m-3 text-right">
      <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
      <Button label="Update" class="m-2" type="submit" />
    </div>
  </form>
</template>
