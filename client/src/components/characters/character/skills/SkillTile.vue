<script setup lang="ts">

import axios from "axios";
import Panel from "primevue/panel";
import {computed, onMounted, ref, type Ref} from "vue";
import { useRoute } from 'vue-router'
const route = useRoute()

import Accordion from 'primevue/accordion';
import AccordionPanel from 'primevue/accordionpanel';
import AccordionHeader from 'primevue/accordionheader';
import AccordionContent from 'primevue/accordioncontent';

import type {CharacterSkillsResponse} from "@/components/characters/character/skills/interfaces/CharacterSkillsResponse";
import EditSkillDetail from "@/components/characters/character/skills/EditSkillDetail.vue";
import {skillStore} from "@/components/characters/character/skills/Stores/skillStore";

const offensiveSkills:Ref<Array<CharacterSkillsResponse>> = ref([]);
const defensiveSkills:Ref<Array<CharacterSkillsResponse>> = ref([]);
const maxXP = 28;
const appliedXp = ref(0);
const skillInfo = skillStore();
const openItems = ref([]);

const remainingXP = computed(() => maxXP - appliedXp.value);

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
        appliedXp.value = response.data.reduce((sum: number, item: CharacterSkillsResponse) => sum + item.xp, 0);
      })
}

</script>

<template>
  <div class="d-inline-flex flex-wrap justify-content-center column-gap-3 row-gap-1 w-100">
    <Panel v-for="skillType in skillTypes" class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" style="width: 25em">
      <template #header>
        <div class="row">
          <h3 class="col pb-0 mb-0 mt-0 pt-0">
            {{ skillType.name }}
          </h3>
          <div v-if="skillInfo.showExperience" class="col text-right">
            {{ remainingXP }} EXP
          </div>
        </div>
      </template>
      <Accordion :value="openItems" multiple :lazy="true" expand-icon="pi pi-info-circle" collapse-icon="pi pi-times-circle">
        <AccordionPanel v-for="skill in skillType.skills" :key="skill.name" :value="skill.skillTypeId">
          <AccordionHeader>
            <div class="d-flex justify-content-between w-100 pr-3 flex-column flex-md-row">
              <div>{{ skill.name }}</div>
              <div class="md:text-right mt-md-0 mt-2">
                {{ skill.levelName }} ({{ skill.levelNumber }})
              </div>
            </div>
          </AccordionHeader>
          <AccordionContent>
            <p class="m-0">
              {{ skill.description }}
            </p>
            <EditSkillDetail :skill-type-id="skill.skillTypeId" :selected-level-id="skill.levelId" :remaining-xp="remainingXP" @update-level="getEditOptions()" />
          </AccordionContent>
        </AccordionPanel>
      </Accordion>
    </Panel>
  </div>
  
</template>

<style>
 .p-panel-header{
   background: var(--p-panel-background);
   border-bottom: 0px;
   padding: 1.5em;
   padding-bottom: 0em;
 }
</style>