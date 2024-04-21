<script setup lang="ts">

import Fieldset from 'primevue/fieldset';
import axios from "axios";
import {onMounted, ref} from "vue";
import { useRoute } from 'vue-router'
import StatTileV2 from "@/components/characters/character/StatTileV2.vue";
const route = useRoute()
const stats = ref([]);
const showDetails = ref(false);
const selectedStatType = ref(1);

onMounted(() =>{
  axios.get(`/api/stats/${route.params.id}/smallStats`)
      .then((response) => {
        stats.value = response.data;
      })
});

function showDetailedStat(statTypeId:number){
  console.log("far");
  selectedStatType.value = statTypeId;
  showDetails.value = !showDetails.value;
  
}

</script>

<template>
  <div class="flex flex-wrap justify-content-center column-gap-3 row-gap-3" style="max-width: 350px">
    <div v-if="!showDetails" v-for="stat in stats" class="align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch m-0 p-0">
      <Fieldset :legend="stat.shortName" class="statBlock" @click="showDetailedStat(stat.statTypeId)" style="cursor: pointer">
        <div class=""><strong>{{stat.bonus}}</strong></div> <br/>
        <div><small>{{stat.level}}</small></div>
      </Fieldset>
    </div>
    <StatTileV2 v-else :stat-type-id="selectedStatType" @toggle-stat="showDetails = !showDetails"></StatTileV2>
  </div>

</template>

<style scoped>

.statBlock{
  width: 80px;
}

.statBlock:deep(.p-fieldset-legend) {
  padding: .5rem !important;
}

.statBlock:deep(.p-fieldset-content) {
  padding: 0px !important;
  text-align: center;
}


</style>