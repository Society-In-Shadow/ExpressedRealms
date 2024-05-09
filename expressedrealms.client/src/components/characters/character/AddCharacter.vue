<script setup lang="ts">

import Button from 'primevue/button';
import axios from "axios";
import { useForm } from 'vee-validate';
import {object, string} from 'yup';
import Card from "primevue/card";
import InputTextWrapper from "@/FormWrappers/InputTextWrapper.vue";
import Router from "@/router";
import TextAreaWrapper from "@/FormWrappers/TextAreaWrapper.vue";
import {onMounted, ref} from "vue";
import DropdownWrapper from "@/FormWrappers/DropdownWrapper.vue";

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    name: string().required()
        .max(150)
        .label("Name"),
    background: string()
        .label('Background'),
    expressionId: object().required()
        .label("Expression"),
    factionId: object().required()
        .label("Faction")
  })
});

const [name] = defineField('name');
const [background] = defineField('background');
const [expression] = defineField('expressionId');
const [faction] = defineField('factionId');
const expressions = ref([]);
const factions = ref([]);

onMounted(() =>{
  axios.get(`/api/characters/options`)
      .then((response) => {
        expressions.value = response.data.expressions;
      })
});

const onSubmit = handleSubmit((values) => {
  axios.post('/api/characters', {
    name: values.name,
    expressionId: values.expressionId.id,
    background: values.background,
    factionId: values.factionId.id
  })
      .then(() => {
        Router.push("/characters");
      });
});

function loadFactions(){
  axios.get(`/api/characters/factionOptions/${expression.value.id}`)
      .then((response) => {
        factions.value = response.data;
      })
}

</script>

<template>
  <div class="flex flex-xs-column flex-sm-column flex-lg-row flex-md-row gap-3 m-3 ">
    <Card class="mb-3">
      <template #title>
        Add Character
      </template>
      <template #content>
        <form @submit="onSubmit">
          <InputTextWrapper v-model="name" field-name="Name" :error-text="errors.name" />
          <DropdownWrapper v-model="expression" option-label="name" :options="expressions" field-name="Expression" :error-text="errors.expressionId" @change="loadFactions()">
            <div data-cy="expression-short-description" class="m-2">
              {{ expression?.shortDescription ?? "" }}
            </div>
          </DropdownWrapper>
          <DropdownWrapper v-model="faction" option-label="name" :options="factions" field-name="Faction" :error-text="errors.factionId" :disabled="!expression">
            <div data-cy="faction-description" class="m-2" v-html="faction?.description ?? ''" ></div>
          </DropdownWrapper>
          <TextAreaWrapper v-model="background" field-name="Background" :error-text="errors.background" />
          <Button data-cy="add-character-button" label="Add Character" class="w-100 mb-2" type="submit" />
        </form>
      </template>
    </Card>
  </div>
</template>
