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

const offensiveSkills:Ref<Array<CharacterSkillsResponse>> = ref([]);
const defensiveSkills:Ref<Array<CharacterSkillsResponse>> = ref([]);
const showEdit = ref(false);
const maxXP = 28;
const appliedXp = ref(0);

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

function toggleEdit() 
{
  showEdit.value = !showEdit.value;
}

</script>

<template>
  
  <Card v-for="skillType in skillTypes" class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" style="width: 25em">
    <template #title>{{skillType.name}} <span>- {{remainingXP}} EXP</span></template>
    <template #content>
      <Accordion :value="[]" multiple :lazy="true" expandIcon="pi pi-info-circle" collapseIcon="pi pi-times-circle">
        <AccordionPanel v-for="skill in skillType.skills" :key="skill.name" :value="skill.skillTypeId">
          <AccordionHeader>  
            <div class="d-flex justify-content-between w-100 pr-3">
              <div>{{skill.name}}</div>
              <div class="text-right">{{skill.levelName}}</div>
            </div>
          </AccordionHeader>
          <AccordionContent>
            <p class="m-0">{{ skill.description}}</p>
            <EditSkillDetail :skill-type-id="skill.skillTypeId" :selected-level-id="skill.levelId" @update-level="getEditOptions()" @edit-toggle="toggleEdit()"/>
          </AccordionContent>
        </AccordionPanel>
      </Accordion>
    </template>
  </Card>

</template>
