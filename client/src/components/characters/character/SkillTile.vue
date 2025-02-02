<script setup lang="ts">


import axios from "axios";
import { useForm } from 'vee-validate';
import { object, string }  from 'yup';
import Card from "primevue/card";
import InputTextWrapper from "@/FormWrappers/InputTextWrapper.vue";
import TextAreaWrapper from "@/FormWrappers/TextAreaWrapper.vue";
import {onMounted, ref, computed, type Ref} from "vue";
import { useRoute } from 'vue-router'
import toaster from "@/services/Toasters";
import SmallStatDisplay from "@/components/characters/character/SmallStatDisplay.vue";
const route = useRoute()
import Breadcrumb from 'primevue/breadcrumb';
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";
import DropdownInfoWrapper from "@/FormWrappers/DropdownInfoWrapper.vue";
import {makeIdSafe} from "@/utilities/stringUtilities";
import SkillTile from "@/components/characters/character/SkillTile.vue";

import Accordion from 'primevue/accordion';
import AccordionPanel from 'primevue/accordionpanel';
import AccordionHeader from 'primevue/accordionheader';
import AccordionContent from 'primevue/accordioncontent';

import type {CharacterSkillsResponse} from "@/components/characters/character/interfaces/CharacterSkillsResponse";

const offensiveSkills:Ref<Array<CharacterSkillsResponse>> = ref([]);
const defensiveSkills:Ref<Array<CharacterSkillsResponse>> = ref([]);

const skillTypes = ref([
  { name: "Offensive Skills",  skills: offensiveSkills },
  { name: "Defensive Skills", skills: defensiveSkills }
]);

onMounted(() =>{
  getEditOptions();
});

function getEditOptions() {
  axios.get(`characters/${route.params.id}/skills`)
      .then((response) => {
        offensiveSkills.value = response.data.filter((x: CharacterSkillsResponse) => x.skillSubTypeId === 1);
        defensiveSkills.value = response.data.filter((x: CharacterSkillsResponse) => x.skillSubTypeId === 2);
      })
}

</script>

<template>
  
  <Card v-for="skillType in skillTypes" class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" style="width: 25em">
    <template #title>{{skillType.name}}</template>
    <template #content>
      <Accordion :value="[]" multiple expandIcon="pi pi-info-circle" collapseIcon="pi pi-times-circle">
        <AccordionPanel v-for="skill in skillType.skills" :key="skill.name" :value="skill.skillTypeId">
          <AccordionHeader>  
            <div class="d-flex justify-content-between w-100 pr-3">
              <div>{{skill.name}}</div>
              <div class="text-right">{{skill.levelName}}</div>
            </div>
          </AccordionHeader>
          <AccordionContent>
            <p class="m-0">{{ skill.description}}</p>
            <h3>Level - {{ skill.levelName}}</h3>
            <p class="m-0">{{ skill.levelDescription}}</p>
            <div v-if="skill.benefits && skill.benefits.length > 0">
              <h2>Benefits</h2>
              <div v-if="skill.benefits" v-for="benefit in skill.benefits">
                <h3 class="d-flex justify-content-between w-100">
                  <div>{{benefit.name}}</div>
                  <div class="text-right">+{{benefit.modifier}}</div>
                </h3>
                <p class="m-0">{{ benefit.description}}</p>
              </div>
            </div>
          </AccordionContent>
        </AccordionPanel>
      </Accordion>
    </template>
  </Card>

</template>

<style scoped>

</style>