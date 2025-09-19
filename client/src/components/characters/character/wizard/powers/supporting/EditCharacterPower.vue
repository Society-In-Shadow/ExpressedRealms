<script setup lang="ts">

import FormTextAreaWrapper from "@/FormWrappers/FormTextAreaWrapper.vue";
import Button from "primevue/button";
import {useRoute} from "vue-router";
import {type PropType, watch} from "vue";
import {characterPowersStore} from "@/components/characters/character/powers/stores/characterPowerStore.ts";
import type {Power} from "@/components/characters/character/powers/types.ts";
import {getValidationInstance} from "@/components/characters/character/powers/validations/powerValidations.ts";
import PowerDetails from "@/components/characters/character/wizard/powers/supporting/PowerDetails.vue";
import {confirmationPopup} from "@/components/characters/character/powers/services/confirmationService.ts";

const store = characterPowersStore();
const form = getValidationInstance();
const route = useRoute();
const popups = confirmationPopup(route.params.id);

const props = defineProps({
  power: {
    type: Object as PropType<Power>,
    required: true,
  }
});

watch(() => props.power, async (newValue, oldValue) => {
  form.setValues(props.power);
}, {immediate: true, deep: true})

const onSubmit = form.handleSubmit(async (values) => {
  await store.editPower(values, route.params.id, props.power.id);
});


</script>

<template>

  <form @submit="onSubmit">
    <PowerDetails :power="props.power">
      <template #buttons>
        <Button label="Delete" size="small" severity="danger" @click="popups.deleteConfirmation($event, props.power.id )" />
        <Button label="Update" size="small" type="submit" />
      </template>
    </PowerDetails>
    <FormTextAreaWrapper v-model="form.notes" />
  </form>
</template>
