<script setup lang="ts">

import axios from "axios";
import { useForm } from 'vee-validate';
import { object, string }  from 'yup';
import Card from "primevue/card";
import InputTextWrapper from "@/FormWrappers/InputTextWrapper.vue";
import TextAreaWrapper from "@/FormWrappers/TextAreaWrapper.vue";
import {onMounted, ref, computed} from "vue";
import { useRoute } from 'vue-router'
import toaster from "@/services/Toasters";
const route = useRoute()
import DropdownInfoWrapper from "@/FormWrappers/DropdownInfoWrapper.vue";
import {makeIdSafe} from "@/utilities/stringUtilities";
import type {Faction} from "@/components/characters/character/interfaces/Faction";
import {characterStore} from "@/components/characters/character/stores/characterStore";
const characterInfo = characterStore();

onMounted(async () =>{
  await characterInfo.getCharacterDetails(Number(route.params.id))
      .then(() => {
        factions.value = characterInfo.factions;
        name.value = characterInfo.name;
        background.value = characterInfo.background;
        expression.value = characterInfo.expression;
        faction.value = characterInfo.faction;
      });
});

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    name: string().required()
        .max(150)
        .label("Name"),
    faction: object<Faction>().nullable()
        .label('Faction'),
    background: string().nullable()
        .label('Background'),
  })
});

const [name] = defineField('name');
const [background] = defineField('background');
const [faction] = defineField('faction');
const expression = ref("");
const isLoading = ref(true);
const factions = ref([]);

const onSubmit = handleSubmit((values) => {
  axios.put('/characters/', {
    name: values.name,
    background: values.background,
    id: route.params.id,
    factionId: values.faction?.id
  }).then(() => {
    characterInfo.name = values.name;
    characterInfo.background = values.background;
    characterInfo.faction = values.faction;
    toaster.success("Successfully Updated Character Info!");
  });
});

let expressionRedirectURL = computed(() => {
  if(!isLoading.value){
    return `/expressions/${expression.value.toLowerCase()}#${makeIdSafe(faction.value.name)}`;
  }
  return '';
})
</script>

<template>
  <Card class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" style="width: 390px">
    <template #content>
      <form @submit="onSubmit">
        <InputTextWrapper v-model="name" field-name="Name" :error-text="errors.name" :show-skeleton="characterInfo.isLoading" @change="onSubmit" />
        <InputTextWrapper v-model="expression" field-name="Expression" disabled :show-skeleton="characterInfo.isLoading" @change="onSubmit" />
        <DropdownInfoWrapper
          v-model="faction" option-label="name" :options="factions" field-name="Faction" :error-text="errors.factionId"
          :show-skeleton="characterInfo.isLoading" :redirect-url="expressionRedirectURL" @change="onSubmit"
        />
        <TextAreaWrapper v-model="background" field-name="Background" :error-text="errors.background" :show-skeleton="characterInfo.isLoading" @change="onSubmit" />
      </form>
    </template>
  </Card>
</template>
