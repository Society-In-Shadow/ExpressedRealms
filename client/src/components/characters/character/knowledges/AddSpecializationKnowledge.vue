<script setup lang="ts">

import FormTextAreaWrapper from "@/FormWrappers/FormTextAreaWrapper.vue";
import Button from "primevue/button";
import {characterKnowledgeStore} from "@/components/characters/character/knowledges/stores/characterKnowledgeStore";
import {useRoute} from "vue-router";
import {inject, onBeforeMount, ref} from "vue";
import Message from "primevue/message";

import FormInputTextWrapper from "@/FormWrappers/FormInputTextWrapper.vue";
import {
  getValidationInstance
} from "@/components/characters/character/knowledges/validations/specializationValidations";
import type {CharacterKnowledge} from "@/components/characters/character/knowledges/types";

const store = characterKnowledgeStore();
const form = getValidationInstance();
const route = useRoute();

const dialogRef = inject('dialogRef');

const knowledge = ref<CharacterKnowledge>(dialogRef.value.data.knowledge);

onBeforeMount(async () => {
  await store.getKnowledgeLevels(route.params.id);
})

const closeDialog = () => {
  dialogRef.value.close();
}

const onSubmit = form.handleSubmit(async (values) => {
  await store.addSpecialization(values, route.params.id, knowledge.value.mappingId);
  closeDialog()
});

</script>

<template>

  <h1 class="pt-0 mt-0">{{ knowledge.knowledge.name }}</h1>
  <h3>{{ knowledge.knowledge.type }}</h3>
  <p>{{ knowledge.knowledge.description }}</p>
  <h3 class="text-right">Available Experience: {{store.currentExperience }}</h3>
  
  <Message severity="warn" class="mb-2" v-if="store.currentExperience < 2">
    You do not have enough experience to add a specialization (2xp)
  </Message>
  
  <form @submit="onSubmit">

    <FormInputTextWrapper v-model="form.name" :disabled="store.currentExperience < 2" />
    
    <FormTextAreaWrapper v-model="form.description" :disabled="store.currentExperience < 2"/>
    
    <FormTextAreaWrapper v-model="form.notes" :disabled="store.currentExperience < 2"/>

    <div class="m-3 text-right">
      <Button label="Cancel" class="m-2" type="reset" @click="closeDialog()" />
      <Button label="Add" class="m-2" type="submit" :disabled="store.currentExperience < 2"/>
    </div>
    
  </form>
  
</template>
