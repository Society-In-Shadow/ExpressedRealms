<script setup lang="ts">

import axios from "axios";
import Card from "primevue/card";
import {computed, onBeforeMount, ref} from "vue";
import {useRoute} from 'vue-router'
import toaster from "@/services/Toasters";
import {makeIdSafe} from "@/utilities/stringUtilities";
import {characterStore} from "@/components/characters/character/stores/characterStore";
import {FeatureFlags, userStore} from "@/stores/userStore.ts";
import HighLevelExpressionInfo
  from "@/components/characters/character/wizard/basicInfo/supporting/HighLevelExpressionInfo.vue";
import {wizardContentStore} from "@/components/characters/character/wizard/stores/wizardContentStore.ts";
import type {WizardContent} from "@/components/characters/character/wizard/types.ts";
import Button from "primevue/button";
import {breakpointsBootstrapV5, useBreakpoints} from "@vueuse/core";
import {
  getValidationInstance
} from "@/components/characters/character/wizard/basicInfo/validators/characterValidation.ts";
import FormTextAreaWrapper from "@/FormWrappers/FormTextAreaWrapper.vue";
import FormTextWrapper from "@/FormWrappers/FormInputTextWrapper.vue";
import FormCheckboxWrapper from "@/FormWrappers/FormCheckboxWrapper.vue";

const route = useRoute()

const characterInfo = characterStore();
const userInfo = userStore();
const showFactionInfo = ref(false);
const form = getValidationInstance();

const activeBreakpoint = useBreakpoints(breakpointsBootstrapV5);
const isMobile = activeBreakpoint.smaller('md');

onBeforeMount(async () =>{
  await characterInfo.getCharacterDetails(Number(route.params.id))
      .then(() => {
        form.fields.name.field.value = characterInfo.name;
        form.fields.background.field.value = characterInfo.background;
        form.fields.expression.field.value = characterInfo.expression;
        form.fields.faction.field.value = characterInfo.faction;
        form.fields.isPrimaryCharacter.field.value = characterInfo.isPrimaryCharacter;
      });
  showFactionInfo.value = await userInfo.hasFeatureFlag(FeatureFlags.ShowFactionDropdown);
  if(!isMobile.value){
    updateWizardContent();
  }
});



const expression = ref("");
const isLoading = ref(true);
const factions = ref([]);

const onSubmit = form.handleSubmit((values) => {
  axios.put('/characters/', {
    name: values.name,
    background: values.background,
    id: route.params.id,
    factionId: values.faction?.id,
    isPrimaryCharacter: values.isPrimaryCharacter,
  }).then(() => {
    characterInfo.name = values.name;
    characterInfo.background = values.background;
    characterInfo.faction = values.faction;
    toaster.success("Successfully Updated Character Info!");
  });
});

let expressionRedirectURL = computed(() => {
  if(!isLoading.value){
    return `/expressions/${expression.value.toLowerCase()}#${makeIdSafe(form.field.faction.value.name)}`;
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
        <FormTextWrapper v-model="form.fields.name" @change="onSubmit" :show-skeleton="characterInfo.isLoading"/>
        <FormTextWrapper v-model="form.fields.expression" disabled @change="onSubmit" :show-skeleton="characterInfo.isLoading"/>
<!--        <FormDropdownWrapper v-if="showFactionInfo" 
          v-model="form.fields.faction" option-label="name" :options="factions" field-name="Faction" 
          :show-skeleton="characterInfo.isLoading" :redirect-url="expressionRedirectURL" @change="onSubmit"
        />-->
        <FormTextAreaWrapper v-model="form.fields.background" :show-skeleton="characterInfo.isLoading" @change="onSubmit" />
        <FormCheckboxWrapper v-model="form.fields.isPrimaryCharacter" :show-skeleton="characterInfo.isLoading" @change="onSubmit" />
      </form>
      <Button label="Show High Level Expression Info" class="w-100 mb-2 d-block d-md-none " :disabled="characterInfo.isLoading && characterInfo.expressionId !== 0" @click="updateWizardContent" />
    </template>
  </Card>
</template>
