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
const route = useRoute()

const skillLevels:Ref<Array<SkillResponse>> = ref([]);
const isLoading = ref(true);

const emptySkillResponse: SkillResponse = {
  levelId: 0,
  name: '',
  description: '',
};

onMounted(() =>{
  getEditOptions();
});

function getEditOptions() {
  axios.get(`characters/${route.params.id}/skills/${props.skillTypeId}`)
      .then((response) => {
        skillLevels.value = response.data;
        isLoading.value = false;
      })
}
</script>

<template>
  
  <SkillDetail :is-loading="isLoading" :selected-item="skillLevels.find(x => x.levelId === props.selectedLevelId) ?? emptySkillResponse"/>
  
</template>
