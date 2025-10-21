<script setup lang="ts">

import FormInputNumberWrapper from '@/FormWrappers/FormInputNumberWrapper.vue'
import FormTextAreaWrapper from '@/FormWrappers/FormTextAreaWrapper.vue'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import Button from 'primevue/button'
import {getValidationInstance} from '@/components/blessings/validations/blessingLevelForm.ts'
import {blessingsStore} from '@/components/blessings/stores/blessingsStore.ts'
import {inject, onBeforeMount, type Ref, ref} from 'vue'
import ModifierGroup from '@/components/modifiergroups/ModifierGroup.vue'
import {SourceTableEnum} from '@/components/modifiergroups/types.ts'

const store = blessingsStore()
const form = getValidationInstance()

const dialogRef = inject('dialogRef') as Ref
const blessingId = ref(dialogRef.value.data.blessingId)
const levelId = ref(dialogRef.value.data.levelId)
const groupId = ref(dialogRef.value.data.groupId)

onBeforeMount(async () => {
  const values = await store.getBlessingLevel(blessingId.value, levelId.value)
  groupId.value = values.modifierGroupId
  form.setValues(values)
})

const onSubmit = form.handleSubmit(async (_) => {
  if (await store.editBlessingLevel(blessingId.value, levelId.value, form)) {
    cancel()
  }
})

const cancel = () => {
  dialogRef.value.close()
}

</script>

<template>
  <form @submit="onSubmit">
    <FormInputTextWrapper v-model="form.fields.level" />

    <FormTextAreaWrapper v-model="form.fields.description" />

    <FormInputNumberWrapper v-model="form.fields.xpGain" />

    <FormInputNumberWrapper v-model="form.fields.xpCost" />

    <div class="m-3 text-right">
      <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
      <Button label="Update" class="m-2" type="submit" />
    </div>
  </form>

  <ModifierGroup :group-id="groupId" :source="SourceTableEnum.Blessings" :source-id="levelId" />
</template>
