<script setup lang="ts">

import FormDropdownWrapper from '@/FormWrappers/FormDropdownWrapper.vue'
import FormEditorWrapper from '@/FormWrappers/FormEditorWrapper.vue'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import Button from 'primevue/button'
import { onBeforeMount, type PropType, ref } from 'vue'
import axios from 'axios'
import toaster from '@/services/Toasters'
import { getValidationInstance } from '@/components/expressions/powers/Validations/PowerValidations'
import FormCheckboxWrapper from '@/FormWrappers/FormCheckboxWrapper.vue'
import FormMultiSelectWrapper from '@/FormWrappers/FormMultiSelectWrapper.vue'
import { powersStore } from '@/components/expressions/powers/stores/powersStore'
import PowerPrerequisites from '@/components/expressions/powers/PowerPrerequisites.vue'
import { TargetPowerType } from '@/components/expressions/powers/types.ts'

const form = getValidationInstance()

const emit = defineEmits<{
  cancelled: []
  updated: []
}>()

const props = defineProps({
  target: {
    type: Object as PropType<TargetPowerType>,
    required: true,
  },
  targetId: {
    type: Number,
    required: true,
  },
})

const powers = powersStore()
const prerequisiteChild = ref()

onBeforeMount(async () => {
  await powers.getPowerOptions()
})

const onSubmit = form.handleSubmit(async (values) => {
  await axios.post(`/powers`, {
    target: props.target,
    targetId: props.targetId,
    name: values.name,
    description: values.description,
    gameMechanicEffect: values.gameMechanicEffect,
    limitation: values.limitation,
    powerDuration: values.powerDuration.id,
    areaOfEffect: values.areaOfEffect.id,
    powerLevel: values.powerLevel.id,
    powerActivationType: values.powerActivationType.id,
    categoryIds: values.category?.map((item: { id: string | number }) => item.id),
    other: values.other,
    isPowerUse: values.isPowerUse,
    cost: values.cost,
  })
    .then(async (response) => {
      const newPowerId = response.data
      if (props.target === TargetPowerType.PowerPath) {
        await prerequisiteChild.value.addUpdatePrerequisite(newPowerId)
      }
      emit('updated')
      toaster.success('Successfully Added Power!')
      reset()
    })
})

const reset = () => {
  form.customResetForm()
  emit('cancelled')
}

</script>

<template>
  <div class="m-2">
    <form @submit="onSubmit">
      <FormInputTextWrapper v-model="form.name" />

      <FormDropdownWrapper
        v-model="form.powerLevel"
        :options="powers.powerLevels"
        option-label="name"
      />

      <FormMultiSelectWrapper
        v-model="form.category"
        :options="powers.categories"
        option-label="name"
      />

      <FormDropdownWrapper
        v-model="form.powerActivationType"
        :options="powers.powerActivationTypes"
        option-label="name"
      />

      <FormCheckboxWrapper v-model="form.isPowerUse" />

      <FormEditorWrapper v-model="form.description" />

      <FormEditorWrapper v-model="form.gameMechanicEffect" />

      <FormEditorWrapper v-model="form.limitation" />

      <FormDropdownWrapper
        v-model="form.areaOfEffect"
        :options="powers.areaOfEffects"
        option-label="name"
      />

      <FormDropdownWrapper
        v-model="form.powerDuration"
        :options="powers.powerDurations"
        option-label="name"
      />

      <FormInputTextWrapper v-model="form.cost" />

      <FormEditorWrapper v-model="form.other" />

      <PowerPrerequisites v-if="props.target === TargetPowerType.PowerPath" ref="prerequisiteChild" :power-path-id="props.targetId" />

      <div class="float-end">
        <Button label="Cancel" class="m-2" type="reset" @click="reset" />
        <Button label="Submit" class="m-2" type="submit" />
      </div>
    </form>
  </div>
</template>
