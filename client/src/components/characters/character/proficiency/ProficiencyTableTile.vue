<script setup lang="ts">

import axios from "axios";
import Card from "primevue/card";
import {computed, onMounted, ref, type Ref} from "vue";
import { useRoute } from 'vue-router'
const route = useRoute()

import Accordion from 'primevue/accordion';
import AccordionPanel from 'primevue/accordionpanel';
import AccordionHeader from 'primevue/accordionheader';
import AccordionContent from 'primevue/accordioncontent';

import type {CharacterSkillsResponse} from "@/components/characters/character/skills/interfaces/CharacterSkillsResponse";

const offensiveSkills:Ref<Array<CharacterSkillsResponse>> = ref([]);
const defensiveSkills:Ref<Array<CharacterSkillsResponse>> = ref([]);
const maxXP = 28;
const appliedXp = ref(0);
const openItems = ref([]);

const skillTypes = ref([
  { name: "Offensive Skills",  skills: offensiveSkills },
  { name: "Defensive Skills", skills: defensiveSkills }
]);

onMounted(() =>{
  getEditOptions();
});

var offensive = ref([]);
var defensive = ref([]);

function getEditOptions() {
  axios.get(`proficiencies/${route.params.id}`)
      .then((response) => {
        offensive.value = response.data.proficiencies.filter(x => x.type === "Offensive");
        defensive.value = response.data.proficiencies.filter(x => x.type === "Defensive");
      })
}

</script>

<template>
  <Card class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" style="width: 75em">
    <template #title>
      Proficiencies
    </template>
    <template #content>
      <div class="row">
        <div class="col">
          <Accordion :value="openItems" multiple :lazy="true" expand-icon="pi pi-info-circle" collapse-icon="pi pi-times-circle">
            <AccordionPanel v-for="proficiency in offensive" :key="proficiency.id" :value="proficiency.id" >
              <AccordionHeader>
                <div class="d-flex justify-content-between w-100 pr-3">
                  <div>{{proficiency.name}}</div>
                  <div class="text-right">
                    {{proficiency.value}}
                  </div>
                </div>
              </AccordionHeader>
              <AccordionContent>
                <div v-for="modifier in proficiency.appliedModifiers">
                  <div class="row">
                    <div class="col">{{modifier.name}}</div>
                    <div class="col">{{modifier.message}}</div>
                    <div class="col">{{modifier.value}}</div>
                  </div>
                </div>
              </AccordionContent>
            </AccordionPanel>
          </Accordion>
        </div>
        <div class="col">
          <Accordion :value="openItems" multiple :lazy="true" expand-icon="pi pi-info-circle" collapse-icon="pi pi-times-circle">
            <AccordionPanel v-for="proficiency in defensive" :key="proficiency.id" :value="proficiency.id" >
              <AccordionHeader>
                <div class="d-flex justify-content-between w-100 pr-3">
                  <div>{{proficiency.name}}</div>
                  <div class="text-right">
                    {{proficiency.value}}
                  </div>
                </div>
              </AccordionHeader>
              <AccordionContent>
                <div v-for="modifier in proficiency.appliedModifiers">
                  <div class="row">
                    <div class="col">{{modifier.name}}</div>
                    <div class="col">{{modifier.message}}</div>
                    <div class="col">{{modifier.value}}</div>
                  </div>
                </div>
              </AccordionContent>
            </AccordionPanel>
          </Accordion>
        </div>
      </div>
      
    </template>
  </Card>
</template>
