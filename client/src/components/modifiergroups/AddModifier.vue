<script setup lang="ts">

import FormDropdownWrapper from '@/FormWrappers/FormDropdownWrapper.vue'
import { getValidationInstance } from '@/components/modifiergroups/validations/modifierValidations'
import Button from 'primevue/button'
import { onBeforeMount, type PropType } from 'vue'
import ModifierGroupStore from '@/components/modifiergroups/stores/modifierGroupStore.ts'
import FormInputNumberWrapper from '@/FormWrappers/FormInputNumberWrapper.vue'
import FormCheckboxWrapper from '@/FormWrappers/FormCheckboxWrapper.vue'
import { SourceTableEnum } from '@/components/modifiergroups/types.ts'

const store = ModifierGroupStore()

const form = getValidationInstance()
const emit = defineEmits<{
  canceled: []
  updateGroupId: [groupId: number]
}>()

const props = defineProps({
  source: {
    type: Number as PropType<SourceTableEnum>,
    required: true,
    validator: (value: number): value is SourceTableEnum =>
      Object.values(SourceTableEnum).includes(value as SourceTableEnum),
  },
  sourceId: {
    type: Number,
    required: true,
  },
  groupId: {
    type: Number,
    required: false,
    default: null,
  },
})

onBeforeMount(async () => {
  await store.getOptions()
})

const onSubmit = form.handleSubmit(async (values) => {
  const groupId = await store.addModifier(values, props.groupId, props.sourceId, props.source)
  emit('updateGroupId', groupId)
  cancel()
})

const cancel = () => {
  emit('canceled')
}

</script>

<template>
  <form @submit="onSubmit">
    <FormInputNumberWrapper v-model="form.fields.modifier" />

    <FormDropdownWrapper
      v-model="form.fields.modifierType"
      :options="store.modifierTypes"
      option-label="name"
    />

    <FormCheckboxWrapper v-model="form.fields.scaleWithLevel" />

    <FormCheckboxWrapper v-model="form.fields.creationSpecificBonus" />

    <FormDropdownWrapper
      v-model="form.fields.targetExpression"
      :options="store.expressions"
      option-label="name"
    />

    <div class="m-3 text-right">
      <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
      <Button label="Add" class="m-2" type="submit" />
    </div>
  </form>
</template>
