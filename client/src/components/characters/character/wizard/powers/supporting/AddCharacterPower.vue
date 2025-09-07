<script setup lang="ts">

import Message from "primevue/message";
import FormTextAreaWrapper from "@/FormWrappers/FormTextAreaWrapper.vue";
import Button from "primevue/button";
import {getValidationInstance} from "@/components/characters/character/powers/validations/powerValidations.ts";
import {useRoute} from "vue-router";
import {computed, onMounted, type PropType, ref} from "vue";
import {characterPowersStore} from "@/components/characters/character/powers/stores/characterPowerStore.ts";
import type {Power} from "@/components/characters/character/powers/types.ts";
import PowerDetails from "@/components/characters/character/wizard/powers/supporting/PowerDetails.vue";

const store = characterPowersStore();
const form = getValidationInstance();
const route = useRoute();

const props = defineProps({
  power: {
    type: Object as PropType<Power>,
    required: true,
  }
});

const availableXp = ref(0);
const powerXp = ref(0);

const disabled = computed(() => {
  return availableXp.value < powerXp.value || availableXp.value == 0 && powerXp.value == 0;
})

onMounted(async () => {
  const values = await store.getPowerOptions(route.params.id, props.power.id);
  availableXp.value = values.availableXp;
  powerXp.value = values.powerXp;
})


const onSubmit = form.handleSubmit(async (values) => {
  await store.addPower(values, route.params.id, props.power.id);
});

</script>

<template>
  
  <PowerDetails :power="props.power" />
  
  <h3 class="d-flex justify-content-between">
    <span>Experience Cost: {{ powerXp }}</span>
    <span>Available Experience: {{ availableXp }}</span>
  </h3>
  <Message severity="warn" v-if="disabled">
    You do not have enough experience to add this power
  </Message>
  <form @submit="onSubmit">

    <FormTextAreaWrapper v-model="form.notes" :disabled="disabled"  />

    <div class="m-3 text-right">
      <Button label="Add" class="m-2" type="submit" :disabled="disabled" />
    </div>
  </form>
</template>
