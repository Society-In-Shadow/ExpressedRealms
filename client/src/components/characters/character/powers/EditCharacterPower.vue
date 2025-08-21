<script setup lang="ts">

import FormTextAreaWrapper from "@/FormWrappers/FormTextAreaWrapper.vue";
import Button from "primevue/button";
import {useRoute} from "vue-router";
import {inject, onBeforeMount, ref} from "vue";
import {characterPowersStore} from "@/components/characters/character/powers/stores/characterPowerStore.ts";
import type {Power} from "@/components/characters/character/powers/types.ts";
import PickPowerCard from "@/components/characters/character/powers/PickPowerCard.vue";
import {getValidationInstance} from "@/components/characters/character/powers/validations/powerValidations.ts";

const store = characterPowersStore();
const form = getValidationInstance();
const route = useRoute();

const dialogRef = inject('dialogRef');

const power = ref<Power>(dialogRef.value.data.power);

const closeDialog = () => {
  dialogRef.value.close();
}

onBeforeMount(async () => {
  form.setValues(power.value);
})

const onSubmit = form.handleSubmit(async (values) => {
  await store.editPower(values, route.params.id, power.value.id);
  closeDialog()
});

</script>

<template>
  
  <PickPowerCard :power="power" />
  
  <form @submit="onSubmit">

    <FormTextAreaWrapper v-model="form.notes" />

    <div class="m-3 text-right">
      <Button label="Cancel" class="m-2" type="reset" @click="closeDialog()" />
      <Button label="Update" class="m-2" type="submit" />
    </div>
  </form>
</template>
