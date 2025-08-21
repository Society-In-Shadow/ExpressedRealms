<script setup lang="ts">

import FormTextAreaWrapper from "@/FormWrappers/FormTextAreaWrapper.vue";
import Button from "primevue/button";
import {getValidationInstance} from "@/components/characters/character/knowledges/validations/knowledgeValidations";
import {useRoute} from "vue-router";
import {inject, onBeforeMount, ref} from "vue";
import {characterPowersStore} from "@/components/characters/character/powers/stores/characterPowerStore.ts";
import PickPowerCard from "@/components/characters/character/powers/PickPowerCard.vue";

const store = characterPowersStore();
const form = getValidationInstance();
const route = useRoute();

const dialogRef = inject('dialogRef');

const power = ref(dialogRef.value.data.power);

const closeDialog = () => {
  dialogRef.value.close();
}

onBeforeMount(async () => {
/*  await store.getKnowledgeLevels(route.params.id);
  store.knowledgeLevels.forEach(function(level:KnowledgeOptions) {
    const xpCost = isUnknownKnowledge.value ? level.totalUnknownXpCost : level.totalGeneralXpCost;
    level.disabled = xpCost > store.currentExperience;
  });*/
})

const onSubmit = form.handleSubmit(async (values) => {
  await store.addPower(values, route.params.id, power.value.id);
  closeDialog()
});

</script>

<template>
  
  <PickPowerCard :power="power" />
  
  <h3 class="text-right">
    Available Experience: {{ store.currentExperience }}
  </h3>
  <form @submit="onSubmit">

    <FormTextAreaWrapper v-model="form.notes" />

    <div class="m-3 text-right">
      <Button label="Cancel" class="m-2" type="reset" @click="closeDialog()" />
      <Button label="Pick" class="m-2" type="submit" />
    </div>
  </form>
</template>
