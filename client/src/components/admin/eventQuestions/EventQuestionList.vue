<script setup lang="ts">

import { computed, inject, onBeforeMount, type Ref, ref } from 'vue'
import Button from 'primevue/button'
import { EventQuestionStore } from '@/components/admin/eventQuestions/stores/eventQuestionStore.ts'
import AddEventQuestion from '@/components/admin/eventQuestions/AddEventQuestion.vue'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'
import EventQuestionItem from '@/components/admin/eventQuestions/EventQuestionItem.vue'

const store = EventQuestionStore()
const permissionStore = userPermissionStore()
const permissionCheck = permissionStore.permissionCheck

const dialogRef = inject<Ref<any> | null>('dialogRef', null) as Ref
const props = defineProps({
  eventId: {
    type: Number,
    default: undefined,
  },
  isReadOnly: {
    type: Boolean,
    default: undefined,
  },
})

const eventId = computed(() =>
  props.eventId ?? dialogRef?.value?.data?.eventId,
)

const isReadOnly = computed(() =>
  props.isReadOnly ?? dialogRef?.value?.data?.isReadOnly ?? false,
)

onBeforeMount(async () => {
  await store.getItems(eventId.value)
})

const showAdd = ref(false)

const toggleAdd = () => {
  showAdd.value = !showAdd.value
}

</script>

<template>
  <div v-for="question in store.questions" :key="question.id" class="w-100">
    <EventQuestionItem :event-question="question" :is-read-only="isReadOnly" :event-id="eventId" />
  </div>

  <AddEventQuestion v-if="showAdd && permissionCheck.EventQuestion.Create && !isReadOnly" :event-id="eventId" @canceled="toggleAdd" />
  <Button
    v-if="!showAdd && permissionCheck.EventQuestion.Create && !isReadOnly" class="w-100 m-2"
    label="Add Question" @click="toggleAdd"
  />
</template>
