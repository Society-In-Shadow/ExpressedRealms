<script setup lang="ts">

import axios from "axios";
import {onMounted, ref, type Ref, watch} from "vue";
import {useRoute} from 'vue-router'
import Button from 'primevue/button';
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import type {LevelInfo, Stat} from "@/components/characters/character/wizard/stats/types.ts";
import {statStore} from "@/components/characters/character/wizard/stats/stores/statStore.ts";
import {experienceStore, XpSectionTypes} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import ShowXPCosts from "@/components/characters/character/wizard/ShowXPCosts.vue";

const route = useRoute()
const statInfo = statStore();
const xpInfo = experienceStore();

const props = defineProps({
  statTypeId: {
    type: Number,
    required: true,
  },
});

const stat:Ref<Stat> = ref({
  statLevelInfo: {}
});

const newStat:Ref<Stat> = ref({
  statLevelInfo: {}
});

const loading = ref(true);
const oldValue = ref(props.statTypeId);
const expandedRows = ref({});

onMounted(async () =>{
  await reloadData();
});

async function reloadData(){
  await reloadStatInfo();
  await statInfo.getEditOptions(stat.value.id);
  const info = xpInfo.getExperienceInfoForSection(XpSectionTypes.stats);
  statInfo.statLevels.forEach(function(level:LevelInfo) {
    level.disabled = level.totalXP - stat.value.statLevelInfo.totalXP > info.availableXp;
  });
  expandedRows.value = Object.fromEntries(statInfo.statLevels.map(p => [p.level, true]));
}

watch(() => props.statTypeId, (newValue, oldValue) => {
  reloadData();
})

async function reloadStatInfo() {
  await axios.get(`/characters/${route.params.id}/stat/${props.statTypeId}`)
      .then((response) => {
        stat.value = response.data;
        newStat.value = structuredClone(response.data);
        loading.value = false;
      })
}

async function handleStatUpdate(stat:Stat){
  // Don't allow them to unselect the option
  if(stat.statLevel == undefined)
  {
    stat.statLevel = oldValue.value;
    return;
  }
  
  await statInfo.updateStat(stat, route.params.id, props.statTypeId);
  oldValue.value = stat.statLevel;

}

</script>

<template>
  <div class="w-100" style="min-width: 300px">
    <div class="row">
      <div class="col">
        <h3 class="mt-0">
          <SkeletonWrapper :show-skeleton="loading" height="2rem">
            <div class="row">
              <div class="col">
                {{ stat.name }}
              </div>
              <div class="col text-right">
                <Button label="Update" size="small" @click="handleStatUpdate(newStat)" />
              </div>
            </div>
          </SkeletonWrapper>
        </h3>
        <div class="mb-3">
          <SkeletonWrapper :show-skeleton="loading" height="3rem">
            {{ stat.description }}
          </SkeletonWrapper>
        </div>
      </div>
    </div>
    <ShowXPCosts :section-type="XpSectionTypes.stats" />
    <DataTable v-model:selection="newStat.statLevelInfo" v-model:expandedRows="expandedRows" :value="statInfo.statLevels" selection-mode="single" data-key="level" :rowClass="row => (row.disabled ? 'non-selectable' : '')">
      <Column selection-mode="single" headerStyle="width: 3rem"></Column>
      <Column field="level" header="Level">
        <template #body="slotProps">
          <SkeletonWrapper :show-skeleton="loading" height="2rem" width="100%" header-class="text-center" body-class="text-center">
            {{ slotProps.data.level }}
          </SkeletonWrapper>
        </template>
      </Column>
      <Column header="Bonus">
        <template #body="slotProps">
          <SkeletonWrapper :show-skeleton="loading" height="2rem" width="100%" header-class="text-center" body-class="text-center">
            <span v-if="slotProps.data.bonus > 0">+</span>{{ slotProps.data.bonus }}
          </SkeletonWrapper>
        </template>
      </Column>
      <Column field="xp" header="XP" header-class="text-center" body-class="text-center">
        <template #body="slotProps">
          <SkeletonWrapper :show-skeleton="loading" height="2rem" width="100%">
            {{slotProps.data.totalXP > stat.statLevelInfo.totalXP ? "-" : "+"}}{{ Math.abs(slotProps.data.totalXP - stat.statLevelInfo.totalXP) }}
          </SkeletonWrapper>
        </template>
      </Column>
      <template #expansion="slotProps">
        <div :class="slotProps.data.disabled ? 'non-selectable' : ''">
          <SkeletonWrapper :show-skeleton="loading" height="2rem" width="100%" style="padding-left: 3rem; cursor: pointer;" @click="newStat.statLevelInfo = slotProps.data">
            <div >
              {{ slotProps.data.description }}
            </div>
          </SkeletonWrapper>
        </div>

      </template>
    </DataTable>
  </div>
</template>

<style>
  :deep(th.text-center .p-datatable-column-header-content) {
    justify-content: center;
  }
  .non-selectable { opacity:.6; pointer-events:none; }
</style>