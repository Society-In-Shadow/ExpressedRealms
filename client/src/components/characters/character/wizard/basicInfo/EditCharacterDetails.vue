<script setup lang="ts">

import axios from "axios";
import {useForm} from 'vee-validate';
import {object, string} from 'yup';
import Card from "primevue/card";
import InputTextWrapper from "@/FormWrappers/InputTextWrapper.vue";
import TextAreaWrapper from "@/FormWrappers/TextAreaWrapper.vue";
import {computed, onBeforeMount, ref} from "vue";
import {useRoute} from 'vue-router'
import toaster from "@/services/Toasters";
import DropdownInfoWrapper from "@/FormWrappers/DropdownInfoWrapper.vue";
import {makeIdSafe} from "@/utilities/stringUtilities";
import type {Faction} from "@/components/characters/character/interfaces/Faction";
import {characterStore} from "@/components/characters/character/stores/characterStore";
import {FeatureFlags, userStore} from "@/stores/userStore.ts";
import HighLevelExpressionInfo
  from "@/components/characters/character/wizard/basicInfo/supporting/HighLevelExpressionInfo.vue";
import {wizardContentStore} from "@/components/characters/character/wizard/stores/wizardContentStore.ts";
import type {WizardContent} from "@/components/characters/character/wizard/types.ts";
import Button from "primevue/button";
import {breakpointsBootstrapV5, useBreakpoints} from "@vueuse/core";

const route = useRoute()

const characterInfo = characterStore();
const userInfo = userStore();
const showFactionInfo = ref(false);

const activeBreakpoint = useBreakpoints(breakpointsBootstrapV5);
const isMobile = activeBreakpoint.smaller('md');

onBeforeMount(async () =>{
  await characterInfo.getCharacterDetails(Number(route.params.id))
      .then(() => {
        factions.value = characterInfo.factions;
        name.value = characterInfo.name;
        background.value = characterInfo.background;
        expression.value = characterInfo.expression;
        faction.value = characterInfo.faction;

      });
  showFactionInfo.value = await userInfo.hasFeatureFlag(FeatureFlags.ShowFactionDropdown);
  if(!isMobile.value){
    updateWizardContent();
  }
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

const wizardContentInfo = wizardContentStore();
const updateWizardContent = () => {
  console.log('updating wizard content');
  wizardContentInfo.updateContent(
      {
        headerName: 'Expression Info',
        component: HighLevelExpressionInfo,
        props: { expressionId: characterInfo.expressionId }
      } as WizardContent
  )
}

</script>

<template>
  <Card class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" style="max-width: 30em">
    <template #content>
      <form @submit="onSubmit">
        <InputTextWrapper v-model="name" field-name="Name" :error-text="errors.name" :show-skeleton="characterInfo.isLoading" @change="onSubmit" />
        <InputTextWrapper v-model="expression" field-name="Expression" disabled :show-skeleton="characterInfo.isLoading" @change="onSubmit" />
        <DropdownInfoWrapper v-if="showFactionInfo"
          v-model="faction" option-label="name" :options="factions" field-name="Faction" :error-text="errors.factionId"
          :show-skeleton="characterInfo.isLoading" :redirect-url="expressionRedirectURL" @change="onSubmit"
        />
        <TextAreaWrapper v-model="background" field-name="Background" :error-text="errors.background" :show-skeleton="characterInfo.isLoading" @change="onSubmit" />
      </form>
      <Button label="Show High Level Expression Info" class="w-100 mb-2 d-block d-md-none " :disabled="characterInfo.isLoading && characterInfo.expressionId !== 0" @click="updateWizardContent" />
    </template>
  </Card>
</template>
