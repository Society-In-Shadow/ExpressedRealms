<script setup lang="ts">
import toaster from '@/services/Toasters'

import { inject } from 'vue'
import Button from 'primevue/button'
import FormWrapper from '@/FormWrappers/FormWrapper.vue'
import { getValidationInstance } from '@/components/characters/wizard/factions/validators/factionCreateValidator.ts'
import FormTextAreaWrapper from '@/FormWrappers/FormTextAreaWrapper.vue'
import { requestPromotion } from '@/components/characters/wizard/factions/stores/factionStore.ts'
import type { RequestPromotionInfo } from '@/components/characters/wizard/factions/types.ts'
import type { DialogRef } from '@/utilities/dialogUtilities.ts'

const form = getValidationInstance()

const dialogRef = inject('dialogRef') as DialogRef<RequestPromotionInfo>

const dialogValues = {
  characterId: dialogRef.value.data.characterId,
  factionLevelId: dialogRef.value.data.factionLevelId,
}

const updateFactionFields = requestPromotion((errors) => {
  form.setErrors(errors)
})

const onSubmit = form.handleSubmit(async (values) => {
  await updateFactionFields.mutateAsync({
    data: {
      characterId: dialogValues.characterId,
      factionLevelId: dialogValues.factionLevelId,
      requestReason: values.requestReason,
    } as RequestPromotionInfo,
  })
  toaster.success('Successfully requested the promotion!')
  cancel()
})

const cancel = () => {
  dialogRef.value.close()
}

</script>

<template>
  <p>A promotion consists of one or more tasks assigned out by Faction Leaders (GO's) along with the specified requirements.</p>
  <p>Key things regarding promotions:</p>
  <ul>
    <li>Promotions will only happen during conventions</li>
    <li>Promotions are role play and story driven</li>
    <li>It's possible that you will fail the promotion</li>
    <li>A promotion might take a couple of cons to complete</li>
  </ul>
  <p>While not required, it is appreciated if you could request a promotion before an event, so we can incorporate it into the overall plot.</p>
  <p>If you have an idea of what you would like to do for a promotion, please write it down below.</p>
  <p>Upon successful completion of the task(s), a GO will approve your promotion.</p>
  <FormWrapper @submit="onSubmit">
    <FormTextAreaWrapper v-model="form.fields.requestReason" />
    <Button label="Request Promotion" class="m-2" type="submit" />
  </FormWrapper>
</template>
