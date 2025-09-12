<script setup lang="ts">

import axios from "axios";
import {onMounted, ref} from "vue";
import {useRoute} from 'vue-router'
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";
import Button from "primevue/button";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import StatTile from "@/components/characters/character/wizard/stats/StatTile.vue";
import ShowXPCosts from "@/components/characters/character/wizard/ShowXPCosts.vue";

const route = useRoute()
const stats = ref([ {}, {}, {}, {}, {}, {}]);
const showDetails = ref(false);
const selectedStatType = ref(0);
const isLoading = ref(true);

onMounted(async() =>{
  await loadData();
});

async function loadData(){
  isLoading.value = true;
  await axios.get(`/characters/${route.params.id}/stats`)
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


</script>

<template>
  <ShowXPCosts xp-name-tag="Stat XP" />
  <div>
    <DataTable :value="stats" data-key="statTypeId">
      <Column field="name" header="Name">
        <template #body="slotProps">
          <SkeletonWrapper height="1.5rem" width="2rem" :show-skeleton="isLoading">
            {{ slotProps.data.name }}
          </SkeletonWrapper>
        </template>
      </Column>
      <Column field="level" header="Level" header-class="text-center" body-class="text-center">
        <template #body="slotProps">
          <SkeletonWrapper height="1.5rem" width="2rem" :show-skeleton="isLoading"> 
            {{ slotProps.data.level }}
          </SkeletonWrapper>
        </template>
      </Column>
      <Column field="bonus" header="Bonus" header-class="text-center" body-class="text-center">
        <template #body="slotProps">
          <SkeletonWrapper height="1.5rem" width="2rem" :show-skeleton="isLoading">
            {{ slotProps.data.bonus }}
          </SkeletonWrapper>
        </template>
      </Column>
      <Column>
        <template #body="slotProps">
          <Button class="float-end " size="small" label="View" @click="showDetailedStat(slotProps.data.statTypeId)"/>
        </template>
      </Column>
    </DataTable>
    </div>
    <teleport v-if="selectedStatType != 0" to="#item-modification-section">
      <StatTile :stat-type-id="selectedStatType" @toggle-stat="showDetails = !showDetails" @update-stat="updateStat" />
    </teleport>
</template>

<style scoped>

:deep(th.text-center .p-datatable-column-header-content) {
  justify-content: center;
}

</style>
