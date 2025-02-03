<script setup lang="ts">

import {onMounted, ref, type Ref} from "vue";
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
  }
});

import { useRoute } from 'vue-router'
import SkillDetail from "@/components/characters/character/skills/SkillDetail.vue";
import Listbox from "primevue/listbox";
import toasters from "@/services/Toasters";
const route = useRoute()

const skillLevels:Ref<Array<SkillResponse>> = ref([]);
const isLoading = ref(true);
const showOptions = ref(false);


const oldValue = ref(props.selectedLevelId);
const selectedItem = ref(props.selectedLevelId);

function getSelectedLevelInformation(){
  return skillLevels.value.find(x => x.levelId === selectedItem.value)
}

onMounted(() =>{
  getEditOptions();
});

function getEditOptions() {
  axios.get(`characters/${route.params.id}/skills/${props.skillTypeId}`)
      .then((response) => {
        skillLevels.value = response.data;
        isLoading.value = false;
        oldValue.value = props.selectedLevelId;
        selectedItem.value = props.selectedLevelId;
      })
}

function toggleEditOptions() {
  showOptions.value = true;
}

function handleStatUpdate(skill:SkillResponse){
  // Don't allow them to unselect the option
  if(selectedItem.value == undefined)
  {
    selectedItem.value  = oldValue.value;
    showOptions.value = !showOptions.value;
    return;
  }

  axios.put(`/characters/${route.params.id}/skill/${props.skillTypeId}`, {
    characterId: route.params.id,
    skillTypeId: props.skillTypeId,
    skillLevelId: selectedItem.value
  }).then(function(){
    oldValue.value = selectedItem.value;
    showOptions.value = !showOptions.value;
    
    var levelInfo = getSelectedLevelInformation();
    
    toasters.success("Successfully updated to level " + levelInfo.name);
  }).catch(function() {
    selectedItem.value  = oldValue.value;
  })
  /*axios.put(`/characters/${route.params.id}/stat/${props.statTypeId}`, {
    levelTypeId: skill.statLevel,
    statTypeId: props.statTypeId,
    characterId: route.params.id
  }).then(function(){
    skill.statLevelInfo = statLevels.value.find(x => x.level == skill.statLevel);

    oldValue.value = skill.statLevel;

    emit("updateStat", skill.statLevelInfo.level, skill.statLevelInfo.bonus);
    toasters.success("Successfully updated " + skill.name + " to level " + skill.statLevel);

    reloadStatInfo();
    showOptions.value = !showOptions.value;
  })*/

}

</script>

<template>
  <div class="row pt-3">
    <div class="col">
      <div v-if="!showOptions" class="p-listbox p-3" style="cursor: pointer" @click="toggleEditOptions()">
        <SkillDetail :is-loading="isLoading" :selected-item="getSelectedLevelInformation()"/>
      </div>
      <Listbox v-else v-model="selectedItem" 
               :options="skillLevels" option-value="levelId" option-disabled="disabled"
                @change="handleStatUpdate(selectedItem)">
        <template #option="slotProps">
          <SkillDetail :is-loading="isLoading" :selected-item="slotProps.option"/>
        </template>
      </Listbox>
    </div>
  </div>

  
</template>
