<script setup lang="ts">

import Fieldset from 'primevue/fieldset';
import axios from "axios";
import {onMounted, ref} from "vue";
import { useRoute } from 'vue-router'
import StatTile from "@/components/characters/character/StatTile.vue";
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";
const route = useRoute()
const stats = ref([ {}, {}, {}, {}, {}, {}]);
const showDetails = ref(false);
const selectedStatType = ref(1);
const isLoading = ref(true);

onMounted(() =>{
  axios.get(`/api/characters/${route.params.id}/stats`)
      .then((response) => {
        stats.value = response.data;
        isLoading.value = false;
      })
});

function showDetailedStat(statTypeId:number){
  selectedStatType.value = statTypeId;
  showDetails.value = !showDetails.value;
  
}

function updateStat(level:number, bonus:number){
  var updatedStat = stats.value.find(x => x.statTypeId == selectedStatType.value);
  updatedStat.bonus = bonus;
  updatedStat.level = level;
}

</script>

<template>
  <div class="flex flex-wrap justify-content-center column-gap-3 row-gap-3" style="max-width: 350px">
    <div v-for="stat in stats" v-if="!showDetails" :key="stat.statTypeId" class="align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch m-0 p-0">
      <Fieldset class="statBlock mb-3" style="cursor: pointer;" @click="showDetailedStat(stat.statTypeId)">
        <template #legend>
          <SkeletonWrapper height="1.5rem" width="2rem" :show-skeleton="isLoading">
            {{ stat.shortName }}
          </SkeletonWrapper>
        </template>
        <h2 class="m-1 text-center">
          <SkeletonWrapper :show-skeleton="isLoading" height="2rem" width="2.75em">
            <strong><span v-if="stat.bonus > 0">+</span>{{ stat.bonus }}</strong>
          </SkeletonWrapper>
        </h2>
        <div class="levelDisplay p-fieldset-legend ">
          <SkeletonWrapper :show-skeleton="isLoading">
            {{ stat.level }}
          </SkeletonWrapper>
        </div>
      </Fieldset>
    </div>
    <StatTile v-else :stat-type-id="selectedStatType" @toggle-stat="showDetails = !showDetails" @update-stat="updateStat" />
  </div>
</template>

<style scoped>

.statBlock{
  width: 80px;
}

.statBlock .levelDisplay{
  position: relative;
  top: .4em;
  width: 30px;
  left: .9em; 
  padding: 5px !important;
  font-size: small
}

.statBlock:deep(.p-fieldset-legend) {
  padding: .5rem !important;
}

.statBlock:deep(.p-fieldset-content) {
  padding: 0px !important;
  text-align: center;
  height: 45px
}

</style>
