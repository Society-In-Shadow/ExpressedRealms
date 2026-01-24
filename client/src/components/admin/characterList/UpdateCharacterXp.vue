<script setup lang="ts">

import { inject, onBeforeMount, ref, type Ref } from 'vue'
import { getValidationInstance } from '@/components/admin/characterList/validators/characterXpForm.ts'
import FormInputNumberWrapper from '@/FormWrappers/FormInputNumberWrapper.vue'
import Button from 'primevue/button'
import { adminCharacterListStore } from '@/components/admin/characterList/stores/characterListStore.ts'
import Message from 'primevue/message'

const form = getValidationInstance()
const store = adminCharacterListStore()
const dialogRef = inject('dialogRef') as Ref
const characterId = ref(dialogRef.value.data.characterId)
const playerNumber = ref(dialogRef.value.data.playerNumber)

onBeforeMount(async () => {
  form.setValues(playerNumber.value)
})

const onSubmit = form.handleSubmit(async (values) => {
  await store.updateCharacterXp(values, characterId.value)
  cancel()
})

const cancel = () => {
  dialogRef.value.close()
}

</script>

<template>
  <form @submit="onSubmit">
    <FormInputNumberWrapper v-model="form.fields.playerNumber" />
    <Message class="mb-3">
      This will not do duplicate checking for player number, as multiple characters can exist for one player.
    </Message>
    <Button label="Update" class="m-2" type="submit" />
  </form>
</template>
