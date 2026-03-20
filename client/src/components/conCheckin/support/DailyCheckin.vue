<script setup lang="ts">

import { onBeforeMount, onMounted, ref } from 'vue'
import {
  type ChangedStripInfo,
  handleAdeptSidheSorcerers,
  handleShammas,
} from '@/components/conCheckin/support/BreakOfDawnService.ts'
import { getValidationInstance } from '@/components/conCheckin/validations/breakOfDawnValidations.ts'
import FormInputNumberWrapper from '@/FormWrappers/FormInputNumberWrapper.vue'
import Button from 'primevue/button'
import { proficiencyStore } from '@/components/characters/character/proficiency/stores/proficiencyStore.ts'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'
import type { ListItem } from '@/types/ListItem.ts'
import FormDropdownWrapper from '@/FormWrappers/FormDropdownWrapper.vue'

const form = getValidationInstance()
const changes = ref<ChangedStripInfo | null>(null)
const proficiencyData = proficiencyStore()
const checkinInfo = EventCheckinStore()
const availableExpressions = ref<ListItem[] | null>(null)

onBeforeMount(() => {
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

onMounted(async () => {
  await proficiencyData.getUpdateProficiencies(checkinInfo.primaryCharacter!.characterId)
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
    rwp: proficiencyData.secondary.find(x => x.id == 22).value,
    blood: proficiencyData.secondary.find(x => x.id == 15).value,
    health: proficiencyData.secondary.find(x => x.id == 14).value,
    vitality: proficiencyData.secondary.find(x => x.id == 13).value,
    psyche: proficiencyData.secondary.find(x => x.id == 17).value,
    mortis: proficiencyData.secondary.find(x => x.id == 23).value,
  })
  }
  const simpleNonFeeders = [1, 4, 5]
  if (simpleNonFeeders.includes(form.fields.expression.field.value.id)) {
    changes.value = handleAdeptSidheSorcerers(diffValues, maxValues)
  }
})

</script>

<template>
  <div class="flex flex-md-row flex-column gap-2">
    <div>
      <h1>Are you down anything?</h1>
      <form @submit="onSubmit">
        <FormDropdownWrapper v-model="form.fields.expression" option-label="name" :options="availableExpressions!" />
        <FormInputNumberWrapper v-model="form.fields.xpLevel" />
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
      <h1>What they get and why.</h1>
      <div v-if="changes.vitality.amount > 0 || changes.vitality.reasons.length > 0">
        <h3>+{{ changes.vitality.amount }} Vitality</h3>
        <p v-for="reason in changes.vitality.reasons">
          {{ reason }}
        </p>
      </div>
      <div v-if="changes.health.amount > 0 || changes.health.reasons.length > 0">
        <h3>+{{ changes.health.amount }} Health</h3>
        <p v-for="reason in changes.health.reasons">
          {{ reason }}
        </p>
      </div>
      <div v-if="changes.blood.amount > 0 || changes.blood.reasons.length > 0">
        <h3>+{{ changes.blood.amount }} Blood</h3>
        <p v-for="reason in changes.blood.reasons">
          {{ reason }}
        </p>
      </div>
      <div v-if="changes.rwp.amount > 0 || changes.rwp.reasons.length > 0">
        <h3>+{{ changes.rwp.amount }} RWP</h3>
        <p v-for="reason in changes.rwp.reasons">
          {{ reason }}
        </p>
      </div>
      <div v-if="changes.psyche.amount > 0 || changes.psyche.reasons.length > 0">
        <h3>+{{ changes.psyche.amount }} Psyche</h3>
        <p v-for="reason in changes.psyche.reasons">
          {{ reason }}
        </p>
      </div>
      <div v-if="changes.mortis.amount > 0 || changes.mortis.reasons.length > 0">
        <h3>+{{ changes.mortis.amount }} Mortis</h3>
        <p v-for="reason in changes.mortis.reasons">
          {{ reason }}
        </p>
      </div>
    </div>
  </div>
</template>
