<script setup lang="ts">

import axios from "axios";
import {onMounted, ref} from "vue";
import {useRoute} from 'vue-router'
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import Button from "primevue/button";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import StatTile from "@/components/characters/character/wizard/stats/StatTile.vue";

const route = useRoute()
const experienceInfo = experienceStore();
const stats = ref([ {}, {}, {}, {}, {}, {}]);
const showDetails = ref(false);
const selectedStatType = ref(1);
const isLoading = ref(true);

onMounted(() =>{
  loadData();
});

function loadData(){
  isLoading.value = true;
  axios.get(`/characters/${route.params.id}/stats`)
      .then((response) => {
        stats.value = response.data;
        isLoading.value = false;
      })
}

function showDetailedStat(statTypeId:number){
  selectedStatType.value = statTypeId;
  showDetails.value = !showDetails.value;
}

function updateStat(){
  loadData();
}

const toggleAdd = (id: number) => {
  selectedStatType.value = id;
}

</script>

<template>
  <div class="text-right pb-3" v-if="experienceInfo.showAllExperience">{{ experienceInfo.experienceBreakdown.statsXp}} Total XP - {{experienceInfo.experienceBreakdown.setupStatsXp}} Creation XP = {{experienceInfo.experienceBreakdown.statsXp - experienceInfo.experienceBreakdown.setupStatsXp}} XP</div>
  <div class="">
    <DataTable :value="stats" datakey="id">
      <Column field="shortName" header="Name">
        <template #body="slotProps">
          <SkeletonWrapper height="1.5rem" width="2rem" :show-skeleton="isLoading">
            {{ slotProps.data.name }}
          </SkeletonWrapper>
        </template>
      </Column>
      <Column field="level" header="Level">
        <template #body="slotProps">
          <SkeletonWrapper height="1.5rem" width="2rem" :show-skeleton="isLoading">
            {{ slotProps.data.level }}
          </SkeletonWrapper>
        </template>
      </Column>
      <Column field="bonus" header="Bonus" header-style="text-align: center" body-style="text-align: center;">
        <template #body="slotProps">
          <SkeletonWrapper height="1.5rem" width="2rem" :show-skeleton="isLoading">
            {{ slotProps.data.bonus }}
          </SkeletonWrapper>
        </template>
      </Column>
      <Column>
        <template #body="slotProps">
          <Button class="float-end" size="small" label="View" @click="showDetailedStat(slotProps.data.statTypeId)"/>
        </template>
      </Column>
    </DataTable>
    </div>
    <teleport to="#item-modification-section">
      <StatTile :stat-type-id="selectedStatType" @toggle-stat="showDetails = !showDetails" @update-stat="updateStat" />
    </teleport>
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
