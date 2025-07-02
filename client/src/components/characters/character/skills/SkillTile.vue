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
import EditSkillDetail from "@/components/characters/character/skills/EditSkillDetail.vue";
import {skillStore} from "@/components/characters/character/skills/Stores/skillStore";

const offensiveSkills:Ref<Array<CharacterSkillsResponse>> = ref([]);
const defensiveSkills:Ref<Array<CharacterSkillsResponse>> = ref([]);
const showEdit = ref(false);
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
  <Card v-for="skillType in skillTypes" class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" style="width: 25em">
    <template #title>
      <div class="row">
        <div class="col">
          {{ skillType.name }}
        </div>
        <div v-if="skillInfo.showExperience" class="col text-right">
          {{ remainingXP }} EXP
        </div>
      </div>
    </template>
    <template #content>
      <Accordion :value="openItems" multiple :lazy="true" expand-icon="pi pi-info-circle" collapse-icon="pi pi-times-circle">
        <AccordionPanel v-for="skill in skillType.skills" :key="skill.name" :value="skill.skillTypeId">
          <AccordionHeader>  
            <div class="d-flex justify-content-between w-100 pr-3">
              <div>{{ skill.name }}</div>
              <div class="text-right">
                {{ skill.levelName }} ({{skill.levelNumber}})
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
    </template>
  </Card>
</template>
