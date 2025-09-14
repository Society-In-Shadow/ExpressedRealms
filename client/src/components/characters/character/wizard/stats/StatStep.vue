<script setup lang="ts">

import {onMounted, ref} from "vue";
import {useRoute} from 'vue-router'
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";
import Button from "primevue/button";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import StatTile from "@/components/characters/character/wizard/stats/StatTile.vue";
import ShowXPCosts from "@/components/characters/character/wizard/ShowXPCosts.vue";
import {statStore} from "@/components/characters/character/wizard/stats/stores/statStore.ts";

const route = useRoute()
const statData = statStore();
const showDetails = ref(false);
const selectedStatType = ref(0);

onMounted(async() =>{
  await statData.loadData(route.params.id);
});

function showDetailedStat(statTypeId:number){
  selectedStatType.value = statTypeId;
  showDetails.value = !showDetails.value;
}

</script>

<template>
  <ShowXPCosts xp-name-tag="Stat XP" />
  <div>
    <DataTable :value="statData.stats" data-key="statTypeId">
      <Column field="name" header="Name">
        <template #body="slotProps">
          <SkeletonWrapper height="1.5rem" width="2rem" :show-skeleton="statData.isLoading">
            {{ slotProps.data.name }}
          </SkeletonWrapper>
        </template>
      </Column>
      <Column field="level" header="Level" header-class="text-center" body-class="text-center">
        <template #body="slotProps">
          <SkeletonWrapper height="1.5rem" width="2rem" :show-skeleton="statData.isLoading"> 
            {{ slotProps.data.level }}
          </SkeletonWrapper>
        </template>
      </Column>
      <Column field="bonus" header="Bonus" header-class="text-center" body-class="text-center">
        <template #body="slotProps">
          <SkeletonWrapper height="1.5rem" width="2rem" :show-skeleton="statData.isLoading">
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
      <StatTile :stat-type-id="selectedStatType" @toggle-stat="showDetails = !showDetails" />
    </teleport>
</template>

<style scoped>

:deep(th.text-center .p-datatable-column-header-content) {
  justify-content: center;
}

</style>
