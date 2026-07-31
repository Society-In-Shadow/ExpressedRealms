<script setup lang="ts">
import toaster from '@/services/Toasters'

import { inject } from 'vue'
import Button from 'primevue/button'
import FormWrapper from '@/FormWrappers/FormWrapper.vue'
import FormTextAreaWrapper from '@/FormWrappers/FormTextAreaWrapper.vue'
import type { DialogRef } from '@/utilities/dialogUtilities.ts'
import {
  getValidationInstance,
} from '@/components/characters/character/factions/validators/approvePromotionValidator.ts'
import type { ApprovePromotionInfo } from '@/components/characters/character/factions/types.ts'
import { approvePromotion } from '@/components/characters/wizard/factions/stores/factionStore.ts'

const form = getValidationInstance()

const dialogRef = inject('dialogRef') as DialogRef<ApprovePromotionInfo>

const dialogValues = {
  characterId: dialogRef.value.data.characterId,
  factionLevelId: dialogRef.value.data.factionLevelId,
}

const updateFactionFields = approvePromotion((errors) => {
  form.setErrors(errors)
})

const onSubmit = form.handleSubmit(async (values) => {
  await updateFactionFields.mutateAsync({
    data: {
      characterId: dialogValues.characterId,
      factionLevelId: dialogValues.factionLevelId,
      approvalReason: values.approvalReason,
    } as ApprovePromotionInfo,
  })
  toaster.success('Successfully approved the promotion!')
  cancel()
})

const cancel = () => {
  dialogRef.value.close()
}

</script>

<template>
  <p>Please denote what the character / player did to earn this promotion.</p>
  <p>There's a minimum of 20 characters</p>
  <p>This will be visible to the user</p>
  <FormWrapper @submit="onSubmit">
    <FormTextAreaWrapper v-model="form.fields.approvalReason" />
    <Button label="Approve Promotion" class="m-2" type="submit" />
  </FormWrapper>
</template>
