<script setup lang="ts">
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'
import InputText from 'primevue/inputtext'
import Button from 'primevue/button'
import RadioButtonGroup from 'primevue/radiobuttongroup'
import RadioButton from 'primevue/radiobutton'
import { onBeforeMount, ref } from 'vue'
import { CheckinStage, type Question } from '@/components/conCheckin/types.ts'

const eventCheckinInfo = EventCheckinStore()
const playerName = ref('')
const questions = ref <Array<Question>>([])
const isReadOnly = ref(true)
const broughtNewPlayer = ref(false)

onBeforeMount(async () => {
  const response = await eventCheckinInfo.getQuestions()
  questions.value = response.questions
  isReadOnly.value = response.hasCompletedStage

  const newPlayerQuestion = questions.value.find(q => q.typeId === 6)
  if (newPlayerQuestion && newPlayerQuestion.response && newPlayerQuestion?.response.startsWith('Yes')) {
    broughtNewPlayer.value = true
    playerName.value = newPlayerQuestion?.response.slice(6)
  }
})

const updateNewPlayerQuestion = (question: Question) => {
  question.response = 'No'
  if (broughtNewPlayer.value) {
    question.response = `Yes`
  }
  eventCheckinInfo.updateQuestion(question)
}

const submitCheckin = async () => {
  // Stub for now
  await eventCheckinInfo.approveStage(CheckinStage.EventQuestionsCheck)
  eventCheckinInfo.activeStepperStep = '4'
}

</script>

<template>
  <div v-for="question in questions" :key="question.id">
    <div v-if="question.typeId === 1">
      <h3>{{ question.question }}</h3>
      <p>{{ question.response }}</p>
    </div>

    <div v-else-if="question.typeId === 6">
      <h3>Have you brought in a new player?</h3>
      <div>
        <RadioButtonGroup v-model="broughtNewPlayer" class="d-flex flex-column gap-2 mb-3">
          <div class="d-flex align-items-center gap-2">
            <RadioButton id="yes" :value="true" :disabled="isReadOnly" />
            <label for="yes">Yes</label>
          </div>
          <div class="d-flex align-items-center gap-2">
            <RadioButton id="no" :value="false" :disabled="isReadOnly" />
            <label for="no">No</label>
          </div>
        </RadioButtonGroup>

        <Button label="Save" :disabled="isReadOnly" @click="updateNewPlayerQuestion(question)" />
      </div>
    </div>
    <div v-else>
      <h3>{{ question.question }}</h3>
      <InputText v-model="question.response" :disabled="isReadOnly" />
      <Button v-if="question.typeId !== 1" label="Save" :disabled="isReadOnly" @click="eventCheckinInfo.updateQuestion(question)" />
    </div>
  </div>
  <Button label="Submit Checkin" :disabled="isReadOnly" @click="submitCheckin" />
</template>
