<script setup lang="ts">

import Button from 'primevue/button';
import axios from "axios";
import { useForm } from 'vee-validate';
import {object, string} from 'yup';
import Card from "primevue/card";
import InputTextWrapper from "@/FormWrappers/InputTextWrapper.vue";
import TextAreaWrapper from "@/FormWrappers/TextAreaWrapper.vue";
import {onMounted, ref, computed } from "vue";
import { useRouter } from "vue-router";
import DropdownWrapper from "@/FormWrappers/DropdownWrapper.vue";
import {makeIdSafe} from "@/utilities/stringUtilities";
import DropdownInfoWrapper from "@/FormWrappers/DropdownInfoWrapper.vue";

const Router = useRouter();

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    name: string().required()
        .max(150)
        .label("Name"),
    background: string().nullable()
        .label('Background'),
    expressionId: object().required()
        .label("Expression"),
    factionId: object().nullable()
        .label("Faction")
  })
});

const [name] = defineField('name');
const [background] = defineField('background');
const [expression] = defineField('expressionId');
const [faction] = defineField('factionId');
const expressions = ref([]);
const factions = ref([]);
const isLoadingFactions = ref(true);
const isLoadingExpressions = ref(true);

onMounted(() =>{
  axios.get(`/characters/options`)
      .then((response) => {
        expressions.value = response.data.expressions;
        isLoadingExpressions.value = false;
      })
});

const onSubmit = handleSubmit((values) => {
  axios.post('/characters', {
    name: values.name,
    expressionId: values.expressionId.id,
    background: values.background,
    factionId: values.factionId?.id
  })
      .then(() => {
        Router.push("/characters");
      });
});

function loadFactions(){
  isLoadingFactions.value = true;
  axios.get(`/characters/factionOptions/${expression.value.id}`)
      .then((response) => {
        faction.value = null;
        factions.value = response.data;
        isLoadingFactions.value = false;
      })
}

const expressionRedirectURL = computed(() => {
  if(!isLoadingFactions.value && faction.value){
    return `/expressions/${expression.value.name.toLowerCase()}#${makeIdSafe(faction.value.name)}`;
  }
  return '';
})

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
          <DropdownWrapper
            v-model="expression" option-label="name" :options="expressions" field-name="Expression" :error-text="errors.expressionId"
            :show-skeleton="isLoadingExpressions" @change="loadFactions()"
          >
            <div data-cy="expression-short-description" class="m-2">
              {{ expression?.shortDescription ?? "" }}
            </div>
          </DropdownWrapper>
          <DropdownInfoWrapper
            v-if="expression" v-model="faction" option-label="name" :options="factions" field-name="Faction"
            :error-text="errors.factionId" :disabled="!expression" :redirect-url="expressionRedirectURL" :show-skeleton="isLoadingFactions" :redirect-to-different-page="true"
          />
          <TextAreaWrapper v-model="background" field-name="Background" :error-text="errors.background" />
          <Button data-cy="add-character-button" label="Add Character" class="w-100 mb-2" type="submit" />
        </form>
      </template>
    </Card>
  </div>
</template>
