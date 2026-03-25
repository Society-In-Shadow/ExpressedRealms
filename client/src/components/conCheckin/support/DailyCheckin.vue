<script setup lang="ts">

import { onBeforeMount, ref } from 'vue'
import {
  handleAdeptSidheSorcerers,
  handleAeternari,
  handleShammas,
  handleVampyre,
  type TrackedStripInfo,
} from '@/components/conCheckin/support/BreakOfDawnService.ts'
import { getValidationInstance } from '@/components/conCheckin/validations/breakOfDawnValidations.ts'
import FormInputNumberWrapper from '@/FormWrappers/FormInputNumberWrapper.vue'
import Button from 'primevue/button'
import type { ListItem } from '@/types/ListItem.ts'
import { formatSign } from '@/utilities/stringUtilities.ts'
import type { GetBreakOfDawnInfoResponse } from '@/components/conCheckin/types.ts'
import axios from 'axios'

const form = getValidationInstance()
const changes = ref<TrackedStripInfo | null>(null)
const availableExpressions = ref<ListItem[] | null>(null)
const characterInfo = ref<GetBreakOfDawnInfoResponse>({})

const props = defineProps({
  characterId: {
    type: Number,
    required: false,
    default: 0,
  },
  lookupId: {
    type: String,
    required: false,
    default: '',
  },
})

onBeforeMount(async () => {
  let data: GetBreakOfDawnInfoResponse

  if (props.characterId != 0) {
    const response = await axios.get<GetBreakOfDawnInfoResponse>(`/characters/${props.characterId}/dailyCheckinInfo`)
    data = response.data
  }
  else {
    const response = await axios.get<GetBreakOfDawnInfoResponse>(`/events/checkin/lookup/${props.lookupId}/breakOfDawnInfo`)
    data = response.data
  }

  characterInfo.value = data
  availableExpressions.value = [{
    id: '1',
    name: 'Adept',
    description: '',
  }, {
    id: '2',
    name: 'Aeternari',
    description: '',
  }, {
    id: '3',
    name: 'Shammas',
    description: '',
  }, {
    id: '4',
    name: 'Sidhe',
    description: '',
  }, {
    id: '5',
    name: 'Sorcerers',
    description: '',
  }, {
    id: '6',
    name: 'Vampyres',
    description: '',
  }]
})

const onSubmit = form.handleSubmit(async (values) => {
  const diffValues = {
    rwp: values.rwp,
    blood: values.blood,
    health: values.health,
    vitality: values.vitality,
    psyche: values.psyche,
    mortis: values.mortis,
  }
  const maxValues = {
    rwp: characterInfo.value.rwp,
    blood: characterInfo.value.blood,
    health: characterInfo.value.health,
    vitality: characterInfo.value.vitality,
    psyche: characterInfo.value.psyche,
    mortis: characterInfo.value.mortis,
  }

  const simpleNonFeeders = [1, 4, 5]
  if (simpleNonFeeders.includes(characterInfo.value.expressionId)) {
    changes.value = handleAdeptSidheSorcerers(diffValues, maxValues)
  }
  if (characterInfo.value.expressionId == 3) { // Shammas
    changes.value = handleShammas(diffValues, maxValues, characterInfo.value.characterLevel)
  }
  if (characterInfo.value.expressionId == 2) {
    changes.value = handleAeternari(diffValues, maxValues, characterInfo.value.characterLevel)
  }
  if (characterInfo.value.expressionId == 6) {
    changes.value = handleVampyre(diffValues, maxValues, characterInfo.value.characterLevel)
  }
})

</script>

<template>
  <div class="flex flex-md-row flex-column gap-2">
    <div>
      <h1>Are you down anything?</h1>
      <form @submit="onSubmit">
        <FormInputNumberWrapper v-model="form.fields.vitality" />
        <FormInputNumberWrapper v-model="form.fields.health" />
        <FormInputNumberWrapper v-model="form.fields.blood" />
        <FormInputNumberWrapper v-model="form.fields.rwp" />
        <FormInputNumberWrapper v-model="form.fields.psyche" />
        <FormInputNumberWrapper v-model="form.fields.mortis" />
        <div class="m-3 text-right">
          <Button label="Calculate" class="m-2" type="submit" />
        </div>
      </form>
    </div>
    <div v-if="changes" class="ml-3">
      <h1>Results and Reasons</h1>
      <div v-if="changes.vitality.reasons.length > 0">
        <h3>{{ formatSign(changes.vitality.gainedAmount) }} Vitality</h3>
        <p v-for="reason in changes.vitality.reasons">
          {{ reason }}
        </p>
      </div>
      <div v-if="changes.health.reasons.length > 0">
        <h3>{{ formatSign(changes.health.gainedAmount) }} Health</h3>
        <p v-for="reason in changes.health.reasons">
          {{ reason }}
        </p>
      </div>
      <div v-if="changes.blood.reasons.length > 0">
        <h3>{{ formatSign(changes.blood.gainedAmount) }} Blood</h3>
        <p v-for="reason in changes.blood.reasons">
          {{ reason }}
        </p>
      </div>
      <div v-if="changes.rwp.reasons.length > 0">
        <h3>{{ formatSign(changes.rwp.gainedAmount) }} RWP</h3>
        <p v-for="reason in changes.rwp.reasons">
          {{ reason }}
        </p>
      </div>
      <div v-if="changes.psyche.reasons.length > 0">
        <h3>{{ formatSign(changes.psyche.gainedAmount) }} Psyche</h3>
        <p v-for="reason in changes.psyche.reasons">
          {{ reason }}
        </p>
      </div>
      <div v-if="changes.mortis.reasons.length > 0">
        <h3>{{ formatSign(changes.mortis.gainedAmount) }} Mortis</h3>
        <p v-for="reason in changes.mortis.reasons">
          {{ reason }}
        </p>
      </div>
    </div>
  </div>
</template>
