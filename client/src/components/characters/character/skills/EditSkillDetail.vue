<script setup lang="ts">

import {watch, computed, onMounted, ref, type Ref} from "vue";
import axios from "axios";
import type {SkillResponse} from "@/components/characters/character/skills/interfaces/SkillOptionsResponse";

const props = defineProps({
  skillTypeId: {
    type: Number,
    required: true,
  },
  selectedLevelId:{
    type: Number,
    required: true,
  },
  remainingXp:{
    type: Number,
    required: true
  }
});

import { useRoute } from 'vue-router'
import SkillDetail from "@/components/characters/character/skills/SkillDetail.vue";
import Listbox from "primevue/listbox";
import toasters from "@/services/Toasters";
import {skillStore} from "@/components/characters/character/skills/Stores/skillStore";
import {proficiencyStore} from "@/components/characters/character/proficiency/stores/proficiencyStore";
const route = useRoute()

const skillLevels:Ref<Array<SkillResponse>> = ref([]);
const isLoading = ref(true);
const showOptions = ref(false);

const profStore = proficiencyStore();
const skillInfo = skillStore()
const oldValue = ref(props.selectedLevelId);
const selectedItem = ref(props.selectedLevelId);

function getSelectedLevelInformation(){
  return skillLevels.value.find(x => x.levelId === selectedItem.value)
}

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

function getEditOptions() {
  axios.get(`characters/${route.params.id}/skills/${props.skillTypeId}`)
      .then((response) => {
        skillLevels.value = response.data;

        skillLevels.value.forEach(function(level:SkillResponse) {
          level.disabled = level.experienceCost > props.remainingXp && level.levelId > selectedItem.value;
        });
        
        isLoading.value = false;
        oldValue.value = props.selectedLevelId;
        selectedItem.value = props.selectedLevelId;      
      })
}

const currentXP = computed(() => {
  return skillLevels.value.find(x => x.levelId === selectedItem.value)?.experienceCost ?? 0;
});

function toggleEditOptions() {
  showOptions.value = true;
  skillInfo.editSkillTypeId = props.skillTypeId;
  skillInfo.showExperience = true;
  emit("editToggle");
}

function handleStatUpdate(skill:SkillResponse){
  // Don't allow them to unselect the option
  if(selectedItem.value == undefined)
  {
    selectedItem.value  = oldValue.value;
    showOptions.value = false;
    skillInfo.showExperience = false;
    skillInfo.editSkillTypeId = 0;
    return;
  }

  axios.put(`/characters/${route.params.id}/skill/${props.skillTypeId}`, {
    characterId: route.params.id,
    skillTypeId: props.skillTypeId,
    skillLevelId: selectedItem.value
  }).then(function(){
    oldValue.value = selectedItem.value;
    showOptions.value = false;
    
    var levelInfo = getSelectedLevelInformation();
    skillInfo.showExperience = false;
    skillInfo.editSkillTypeId = 0;
    emit("updateLevel");
    profStore.getUpdateProficiencies(route.params.id);
    toasters.success("Successfully updated to level " + levelInfo.name);
  }).catch(function() {
    selectedItem.value  = oldValue.value;
  })

}

</script>

<template>
  <div class="row pt-3">
    <div class="col p-0 m-0">
      <div v-if="!showOptions || skillInfo.editSkillTypeId != props.skillTypeId" class="p-listbox p-3" style="cursor: pointer;" @click="toggleEditOptions()">
        <SkillDetail :is-loading="isLoading" :selected-item="getSelectedLevelInformation()" :showcase-only="true" scroll-height="30em" />
      </div>
      <Listbox
        v-else v-model="selectedItem" 
        :options="skillLevels" option-value="levelId" option-disabled="disabled"
        @change="handleStatUpdate(selectedItem)"
      >
        <template #option="slotProps">
          <SkillDetail :is-loading="isLoading" :selected-item="slotProps.option" :current-xp-level="currentXP" />
        </template>
      </Listbox>
    </div>
  </div>
</template>

<style>
  .p-listbox-list-container {
    max-height: 25em !important;
  }
</style>
