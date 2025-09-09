<script setup lang="ts">

import axios from "axios";
import Panel from "primevue/panel";
import {computed, onMounted, ref, type Ref} from "vue";
import {useRoute} from 'vue-router'
import type {
  CharacterSkillsResponse
} from "@/components/characters/character/skills/interfaces/CharacterSkillsResponse";
import {skillStore} from "@/components/characters/character/skills/Stores/skillStore";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";
import Column from "primevue/column";
import Button from "primevue/button";
import DataTable from "primevue/datatable";
import EditSkillDetail from "@/components/characters/character/wizard/skills/supporting/EditSkillDetail.vue";

const route = useRoute()

const offensiveSkills:Ref<Array<CharacterSkillsResponse>> = ref([{}, {}, {}, {}, {}, {}]);
const defensiveSkills:Ref<Array<CharacterSkillsResponse>> = ref([{}, {}, {}, {}, {}, {}]);
const maxXP = 1000;
const appliedXp = ref(0);
const skillInfo = skillStore();
const experienceInfo = experienceStore();
const isLoading = ref(true);
const remainingXP = computed(() => maxXP - appliedXp.value);
const selectedSkill = ref<CharacterSkillsResponse>({});

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
        appliedXp.value = response.data.reduce((sum: number, item: CharacterSkillsResponse) => sum + item.totalXp, 0);
        selectedSkill.value = offensiveSkills.value[0];
        isLoading.value = false;
      })
}

function showDetailedStat(skill: CharacterSkillsResponse){
  selectedSkill.value = skill;
}

</script>

<template>
  <div class="text-right pb-3" v-if="experienceInfo.showAllExperience">{{ experienceInfo.experienceBreakdown.skillsXp}} Total XP - {{experienceInfo.experienceBreakdown.setupSkillsXp}} Creation XP = {{experienceInfo.experienceBreakdown.skillsXp - experienceInfo.experienceBreakdown.setupSkillsXp}} XP</div>
  <div class="d-inline-flex flex-wrap justify-content-center column-gap-3 row-gap-1 w-100">
    <Panel v-for="skillType in skillTypes" class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" style="">
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
      <DataTable :value="skillType.skills" data-key="statTypeId">
        <Column field="name" header="Name">
          <template #body="slotProps">
            <SkeletonWrapper height="1.5rem" width="2rem" :show-skeleton="isLoading">
              {{ slotProps.data.name }}
            </SkeletonWrapper>
          </template>
        </Column>
        <Column field="level" header="Name" header-class="text-center" body-class="text-center">
          <template #body="slotProps">
            <SkeletonWrapper height="1.5rem" width="2rem" :show-skeleton="isLoading">
              {{ slotProps.data.levelName }}
            </SkeletonWrapper>
          </template>
        </Column>
        <Column field="bonus" header="Level" header-class="text-center" body-class="text-center">
          <template #body="slotProps">
            <SkeletonWrapper height="1.5rem" width="2rem" :show-skeleton="isLoading">
              {{ slotProps.data.levelNumber }}
            </SkeletonWrapper>
          </template>
        </Column>
        <Column>
          <template #body="slotProps">
            <Button class="float-end " size="small" label="View" @click="showDetailedStat(slotProps.data)"/>
          </template>
        </Column>
      </DataTable>
    </Panel>
    <teleport to="#item-modification-section" v-if="!isLoading">
      <EditSkillDetail :skill="selectedSkill" :remaining-xp="remainingXP" @update-level="getEditOptions()" />
    </teleport>
  </div>
</template>

<style>
 .p-panel-header{
   background: var(--p-panel-background) !important;
   border-bottom: 0px !important;
   padding: 1.5em 1.5em 0em !important;
 }
</style>
