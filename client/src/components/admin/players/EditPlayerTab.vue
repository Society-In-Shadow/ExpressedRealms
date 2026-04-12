<script setup lang="ts">
import { useQuery } from '@pinia/colada'
import FormInputTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import { getValidationInstance } from '@/components/admin/players/validations/eventValidations.ts'
import Button from 'primevue/button'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'
import toaster from '@/services/Toasters'
import { watch } from 'vue'
import FormWrapper from '@/FormWrappers/FormWrapper.vue'
import { basicUserInfoById } from '@/components/admin/players/stores/userStore.ts'
import { userService } from '@/components/admin/players/services/userService.ts'
import { PlayerStore } from '@/components/admin/players/stores/playerStore.ts'

const props = defineProps({
  userId: {
    type: String,
    required: true,
  },
})

const playerData = PlayerStore()
const userPermissions = userPermissionStore()
const permissionCheck = userPermissions.permissionCheck
const { data, isLoading } = useQuery(basicUserInfoById(props.userId))
const form = getValidationInstance()

watch(() => isLoading.value, () => {
  if (!data) return
  form.setValues(data!.value)
})

const onSubmit = form.handleSubmit(async (values) => {
  try {
    await userService.edit(props.userId, values)
    toaster.success('User updated successfully')
    await playerData.updatePlayer(props.userId)
  }
  catch (error) {
    const errors = error?.response.data?.errors as Record<string, string[] | string> | undefined
    if (errors) {
      form.setErrors(errors)
    }
  }
})

</script>

<template>
  <FormWrapper :show-skeleton="isLoading" :is-disabled="!permissionCheck.Player.Edit" @submit="onSubmit">
    <FormInputTextWrapper v-model="form.fields.playerNumber" />
    <div class="m-3 text-right">
      <Button v-if="permissionCheck.Player.Edit" label="Update" class="m-2" type="submit" />
    </div>
  </FormWrapper>
</template>
