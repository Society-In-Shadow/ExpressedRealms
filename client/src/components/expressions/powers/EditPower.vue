<script setup lang="ts">

import FormDropdownWrapper from '@/FormWrappers/FormDropdownWrapper.vue'
import FormEditorWrapper from '@/FormWrappers/FormEditorWrapper.vue'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import Button from 'primevue/button'
import { inject, onBeforeMount, ref } from 'vue'
import { getValidationInstance } from '@/components/expressions/powers/Validations/PowerValidations'
import FormCheckboxWrapper from '@/FormWrappers/FormCheckboxWrapper.vue'
import FormMultiSelectWrapper from '@/FormWrappers/FormMultiSelectWrapper.vue'
import { powersStore } from '@/components/expressions/powers/stores/powersStore'
import { type EditPower, type EditPowerPopup, TargetPowerType } from '@/components/expressions/powers/types'
import { SourceTableEnum } from '@/components/modifiergroups/types.ts'
import ModifierGroup from '@/components/modifiergroups/ModifierGroup.vue'
import type { DialogRef } from '@/utilities/dialogUtilities.ts'
import PowerPrerequisites from '@/components/expressions/powers/PowerPrerequisites.vue'

const form = getValidationInstance()
const power = ref<EditPower>()
const emit = defineEmits<{
  canceled: []
}>()

const dialogRef = inject('dialogRef') as DialogRef<EditPowerPopup>

const powers = powersStore()
const prerequisiteChild = ref()
const groupId = ref(0)

onBeforeMount(async () => {
  power.value = await powers.getPower(dialogRef.value.data.powerId)
  groupId.value = power.value.modifierGroupId
  form.setValues(power.value)
})

const onSubmit = form.handleSubmit(async (values) => {
  await powers.updatePower(values, dialogRef.value.data.powerId)
  dialogRef.value.close({
    action: 'modified',
  })

  if (dialogRef.value.data.target == TargetPowerType.PowerPath) {
    await prerequisiteChild.value.addUpdatePrerequisite(dialogRef.value.data.powerId)
  }
  cancel()
})

const cancel = () => {
  emit('canceled')
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

      <PowerPrerequisites v-if="dialogRef.data.target == TargetPowerType.PowerPath" ref="prerequisiteChild" :power-id="dialogRef.data.powerId" :power-path-id="dialogRef.data.powerPathId" />

      <div class="m-3 text-right">
        <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
        <Button label="Update" class="m-2" type="submit" />
      </div>
    </form>
    <ModifierGroup :group-id="groupId" :source="SourceTableEnum.Powers" :source-id="dialogRef.data.powerId" />
  </div>
</template>
