<script setup lang="ts">
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'
import InputText from 'primevue/inputtext'
import Button from 'primevue/button'

const eventCheckinInfo = EventCheckinStore()

</script>

<template>
  <div v-for="question in eventCheckinInfo.questions" :key="question.id">
    <div v-if="question.typeId === 1">
      <h3>{{ question.question }}</h3>
      <p>{{ question.response }}</p>
    </div>
    <div v-else>
      <h3>{{ question.question }}</h3>
      <InputText v-model="question.response" />
      <Button v-if="question.typeId !== 1" label="Save" @click="eventCheckinInfo.updateQuestion(question)" />
    </div>
  </div>
</template>
