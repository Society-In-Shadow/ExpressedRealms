<script setup lang="ts">

import { type PropType, ref } from 'vue'
import { userStore } from '@/stores/userStore'
import Button from 'primevue/button'
import type { Question } from '@/components/admin/eventQuestions/types.ts'
import FormCheckboxWrapper from '@/FormWrappers/FormCheckboxWrapper.vue'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import { getValidationInstance } from '@/components/admin/eventQuestions/validations/eventQuestionValidation.ts'
import { confirmationPopups } from '@/components/admin/eventQuestions/services/popupService.ts'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'
import EditEventQuestion from '@/components/admin/eventQuestions/EditEventQuestion.vue'

let userInfo = userStore()
const userPermissionData = userPermissionStore()
const checkPermission = userPermissionData.permissionCheck
const props = defineProps({
  eventId: {
    type: Number,
    required: true,
  },
  eventQuestion: {
    type: Object as PropType<Question>,
    required: true,
  },
  isReadOnly: {
    type: Boolean,
    required: true,
  },
})
const dummyForm = getValidationInstance()
let popups = confirmationPopups(props.eventQuestion.id, props.eventQuestion?.question)

const showEdit = ref(false)

function toggleEdit() {
  showEdit.value = !showEdit.value
}

</script>

<template>
  <div v-if="showEdit && checkPermission.EventQuestion.Edit" class="mb-2">
    <EditEventQuestion :question="props.eventQuestion" :event-id="props.eventId" @canceled="toggleEdit" />
  </div>
  <div v-else class="d-flex flex-column flex-md-row align-self-center justify-content-between mt-3">
    <div class="flex-fill mr-3">
      <div v-if="props.eventQuestion.questionTypeId == 3">
        <FormInputTextWrapper v-model="dummyForm.fields.question" :is-disabled="true" :label-override="props.eventQuestion.question" />
      </div>
      <div v-if="props.eventQuestion.questionTypeId == 4">
        <FormCheckboxWrapper v-model="dummyForm.fields.question" :is-disabled="true" :label-override="props.eventQuestion.question" />
      </div>
    </div>
    <div
      v-if="!showEdit && !props.isReadOnly"
      class="p-0 m-0 d-inline-flex align-items-start align-self-center"
    >
      <Button
        v-if="checkPermission.EventQuestion.Delete" class="mr-2" severity="danger" label="Delete"
        @click="popups.deleteConfirmation(props.eventId, $event)"
      />
      <Button v-if="checkPermission.EventQuestion.Edit" class="float-end" label="Edit" @click="toggleEdit" />
    </div>
  </div>
</template>
