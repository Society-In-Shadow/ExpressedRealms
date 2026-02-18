<script setup lang="ts">
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'
import InputText from 'primevue/inputtext'
import Button from 'primevue/button'
import RadioButtonGroup from 'primevue/radiobuttongroup'
import RadioButton from 'primevue/radiobutton'
import { ref, watch } from 'vue'
import type { Question } from '@/components/conCheckin/types.ts'

const eventCheckinInfo = EventCheckinStore()
const playerName = ref('')

watch(() => eventCheckinInfo.questions, () => {
  const newPlayerQuestion = eventCheckinInfo.questions.find(q => q.typeId === 6)
  if (newPlayerQuestion && newPlayerQuestion.response && newPlayerQuestion?.response.startsWith('Yes - ')) {
    eventCheckinInfo.broughtNewPlayer = true
    playerName.value = newPlayerQuestion?.response.slice(6)
  }
}, { immediate: true })

const updateNewPlayerQuestion = (question: Question) => {
  question.response = 'No'
  if (eventCheckinInfo.broughtNewPlayer) {
    question.response = `Yes - ${playerName.value}`
  }
  eventCheckinInfo.updateQuestion(question)
}

</script>

<template>
  <div v-for="question in eventCheckinInfo.questions" :key="question.id">
    <div v-if="question.typeId === 1">
      <h3>{{ question.question }}</h3>
      <p>{{ question.response }}</p>
    </div>

    <div v-else-if="question.typeId === 6">
      <h3>Have you brought in a new player? If so what is their name?</h3>
      <div>
        <RadioButtonGroup v-model="eventCheckinInfo.broughtNewPlayer" class="d-flex flex-column gap-2 mb-3">
          <div class="d-flex align-items-center gap-2">
            <RadioButton id="yes" :value="true" />
            <label for="yes">Yes</label>
            <InputText v-if="eventCheckinInfo.broughtNewPlayer" v-model="playerName" placeholder="Who?" />
          </div>
          <div class="d-flex align-items-center gap-2">
            <RadioButton id="no" :value="false" />
            <label for="no">No</label>
          </div>
        </RadioButtonGroup>

        <Button label="Save" @click="updateNewPlayerQuestion(question)" />
      </div>
    </div>
    <div v-else>
      <h3>{{ question.question }}</h3>
      <InputText v-model="question.response" />
      <Button v-if="question.typeId !== 1" label="Save" @click="eventCheckinInfo.updateQuestion(question)" />
    </div>
  </div>
</template>
