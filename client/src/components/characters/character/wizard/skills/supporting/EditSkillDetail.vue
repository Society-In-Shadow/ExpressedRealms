<script setup lang="ts">

import {computed, onMounted, type PropType, ref, type Ref, watch} from "vue";
import axios from "axios";
import type {SkillResponse} from "@/components/characters/character/skills/interfaces/SkillOptionsResponse";
import {useRoute} from 'vue-router'
import toasters from "@/services/Toasters";
import {skillStore} from "@/components/characters/character/skills/Stores/skillStore";
import {proficiencyStore} from "@/components/characters/character/proficiency/stores/proficiencyStore";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import type {
  CharacterSkillsResponse
} from "@/components/characters/character/skills/interfaces/CharacterSkillsResponse.ts";
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";
import Column from "primevue/column";
import DataTable from "primevue/datatable";
import Button from "primevue/button";

const props = defineProps({
  skill: {
    type: Object as PropType<CharacterSkillsResponse>,
    required: true,
  },
  remainingXp:{
    type: Number,
    required: true
  }
});

const route = useRoute()
const experienceInfo = experienceStore();

const skillLevels:Ref<Array<SkillResponse>> = ref([]);
const isLoading = ref(true);
const showOptions = ref(false);

const profStore = proficiencyStore();
const skillInfo = skillStore()
const skillLevel:Ref<CharacterSkillsResponse> = ref({});
const expandedRows = ref({});
function getSelectedLevelInformation(){
  return skillLevels.value.find(x => x.levelId === selectedItem.value)
}

const selectedItem = ref({});

const emit = defineEmits<{
  updateLevel: [],
  editToggle: []
}>();

watch(() => props.remainingXp, (newValue, oldValue) => {
  getEditOptions()
});

onMounted(() =>{
  getEditOptions();
});

watch(() => props.skill, (newValue, oldValue) => {
  getEditOptions();
})

function getEditOptions() {
  axios.get(`characters/${route.params.id}/skills/${props.skill.skillTypeId}`)
      .then((response) => {
        skillLevels.value = response.data;

        const levelInfo = getSelectedLevelInformation();
        skillLevels.value.forEach(function(level:SkillResponse) {
          level.disabled = level.experienceCost - (levelInfo?.experienceCost ?? 0) > props.remainingXp && level.levelId > selectedItem.value;
        });
        
        isLoading.value = false;
        expandedRows.value = skillLevels.value.reduce((acc, p) => (acc[p.levelId] = true) && acc, {});
        //oldValue.value = props.skill.levelId;
        //selectedItem.value = props.skill.levelId;      
      })
}

const currentXP = computed(() => {
  return skillLevels.value.find(x => x.levelId === selectedItem.value)?.experienceCost ?? 0;
});

function toggleEditOptions() {
  showOptions.value = true;
  skillInfo.editSkillTypeId = props.skill.skillTypeId;
  skillInfo.showExperience = true;
  emit("editToggle");
}

function handleStatUpdate(skill:SkillResponse){
  // Don't allow them to unselect the option
  if(selectedItem.value == undefined)
  {
    //selectedItem.value  = oldValue.value;
    showOptions.value = false;
    skillInfo.showExperience = false;
    skillInfo.editSkillTypeId = 0;
    return;
  }

  axios.put(`/characters/${route.params.id}/skill/${props.skill.skillTypeId}`, {
    characterId: route.params.id,
    skillTypeId: props.skill.skillTypeId,
    skillLevelId: selectedItem.value
  }).then(async function(){
    //oldValue.value = selectedItem.value;
    showOptions.value = false;
    await experienceInfo.updateExperience(route.params.id);
    var levelInfo = getSelectedLevelInformation();
    skillInfo.showExperience = false;
    skillInfo.editSkillTypeId = 0;
    emit("updateLevel");
    profStore.getUpdateProficiencies(route.params.id);
    skillLevels.value.forEach(function(level:SkillResponse) {
      level.disabled = level.experienceCost - (levelInfo?.experienceCost ?? 0) >= props.remainingXp && level.levelId > selectedItem.value;
    });
    toasters.success("Successfully updated to level " + levelInfo.name);
  }).catch(function() {
    //selectedItem.value  = oldValue.value;
  })

}

</script>

<template>
    <div class="row">
      <div class="col">
        <h3 class="mt-0">
          <SkeletonWrapper :show-skeleton="isLoading" height="2rem">
            <div class="row">
              <div class="col">
                {{ props.skill.name }} asdf
              </div>
              <div class="col text-right">
                <Button label="Update" size="small" @click="handleStatUpdate(newStat)" />
              </div>
            </div>
          </SkeletonWrapper>
        </h3>
        <div class="mb-3">
          <SkeletonWrapper :show-skeleton="isLoading" height="3rem">
            {{ props.skill.description }}
          </SkeletonWrapper>
        </div>
      </div>
    </div>
    <DataTable v-model:selection="skillLevel" v-model:expandedRows="expandedRows" :value="skillLevels" selection-mode="single" data-key="levelId">
      <Column selection-mode="single" headerStyle="width: 3rem"></Column>
      <Column field="level" header="Name">
        <template #body="slotProps">
          <SkeletonWrapper :show-skeleton="isLoading" height="2rem" width="100%" header-class="text-center" body-class="text-center">
            {{ slotProps.data.name }}
          </SkeletonWrapper>
        </template>
      </Column>
      <Column header="Level">
        <template #body="slotProps">
          <SkeletonWrapper :show-skeleton="isLoading" height="2rem" width="100%" header-class="text-center" body-class="text-center">
            {{ slotProps.data.levelNumber }}
          </SkeletonWrapper>
        </template>
      </Column>
      <Column field="xp" header="XP" header-class="text-center" body-class="text-center">
        <template #body="slotProps">
          <SkeletonWrapper :show-skeleton="isLoading" height="2rem" width="100%">
<!--              {{slotProps.data.totalXP > stat.statLevelInfo.totalXP ? "-" : "+"}}{{ Math.abs(slotProps.data.totalXP - stat.statLevelInfo.totalXP) }}-->
          </SkeletonWrapper>
        </template>
      </Column>
      <template #expansion="slotProps">
        <SkeletonWrapper :show-skeleton="isLoading" height="2rem" width="100%" style="padding-left: 3rem; cursor: pointer;">
          <div >
            {{ slotProps.data.description }}
          </div>
        </SkeletonWrapper>
      </template>
    </DataTable>
</template>
