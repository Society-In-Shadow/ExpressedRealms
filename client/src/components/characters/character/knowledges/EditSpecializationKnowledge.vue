<script setup lang="ts">

import FormTextAreaWrapper from "@/FormWrappers/FormTextAreaWrapper.vue";
import Button from "primevue/button";
import {characterKnowledgeStore} from "@/components/characters/character/knowledges/stores/characterKnowledgeStore";
import {useRoute} from "vue-router";
import {inject, onBeforeMount, ref} from "vue";

import type {
  CharacterKnowledge,
  Specialization
} from "@/components/characters/character/knowledges/types";
import FormInputTextWrapper from "@/FormWrappers/FormInputTextWrapper.vue";
import {
  getValidationInstance
} from "@/components/characters/character/knowledges/validations/specializationValidations";

const store = characterKnowledgeStore();
const form = getValidationInstance();
const route = useRoute();

const dialogRef = inject('dialogRef');

const knowledge = ref<CharacterKnowledge>(dialogRef.value.data.knowledge);
const specialization = ref<Specialization>(dialogRef.value.data.specialization);

const closeDialog = () => {
  dialogRef.value.close();
}

onBeforeMount(async () => {
  form.setValues(specialization.value);
})

const onSubmit = form.handleSubmit(async (values) => {
  await store.editSpecialization(values, route.params.id, knowledge.value.mappingId, specialization.value.id);
  closeDialog()
});


</script>


<template>

  <h1 class="pt-0 mt-0">{{ knowledge.knowledge.name }}</h1>
  <h3>{{ knowledge.knowledge.type }}</h3>
  <p>{{ knowledge.knowledge.description }}</p>
  
  <h2>Specialization</h2>
  <form @submit="onSubmit">

    <FormInputTextWrapper v-model="form.name" />
    
    <FormTextAreaWrapper v-model="form.description" />
    
    <FormTextAreaWrapper v-model="form.notes" />

    <div class="m-3 text-right">
      <Button label="Cancel" class="m-2" type="reset" @click="closeDialog()"/>
      <Button label="Update" class="m-2" type="submit" />
    </div>
  </form>
</template>
