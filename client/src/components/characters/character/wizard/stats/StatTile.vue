<script setup lang="ts">

import axios from "axios";
import {onMounted, ref, type Ref, watch} from "vue";
import {useRoute} from 'vue-router'
import Button from 'primevue/button';
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";
import toasters from "@/services/Toasters";
import {proficiencyStore} from "@/components/characters/character/proficiency/stores/proficiencyStore";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import type {LevelInfo, Stat} from "@/components/characters/character/wizard/stats/types.ts";

const route = useRoute()

const emit = defineEmits<{
  toggleStat: [],
  updateStat: [level: number, bonus: number]
}>();

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

const statLevels:Ref<Array<LevelInfo>> = ref([]);
const loading = ref(true);
const oldValue = ref(props.statTypeId);
const profStore = proficiencyStore();
const experienceInfo = experienceStore();
const expandedRows = ref({});

onMounted(async () =>{
  await reloadData();
});

async function reloadData(){
  await reloadStatInfo();
  await getEditOptions();
  expandedRows.value = Object.fromEntries(statLevels.value.map(p => [p.level, true]));
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

async function getEditOptions() {
  await axios.get(`/stats/${props.statTypeId}`)
      .then((response) => {
        
        const selectedXP = response.data.find(x => x.level == stat.value.statLevel).totalXP;
        
        response.data.forEach(function(level:LevelInfo) {
          level.disabled = level.totalXP > stat.value.availableXP + selectedXP && level.level > stat.value.statLevel;
        });

        statLevels.value = response.data;
      })
}

function handleStatUpdate(stat:Stat){
  // Don't allow them to unselect the option
  if(stat.statLevel == undefined)
  {
    stat.statLevel = oldValue.value;
    return;
  }
  axios.put(`/characters/${route.params.id}/stat/${props.statTypeId}`, {
    levelTypeId: newStat.value.statLevelInfo.level,
    statTypeId: props.statTypeId,
    characterId: route.params.id
  }).then(async function(){
    stat.statLevelInfo = statLevels.value.find(x => x.level == stat.statLevel);

    oldValue.value = stat.statLevel;
    
    emit("updateStat", stat.statLevelInfo.level, stat.statLevelInfo.bonus);
    toasters.success("Successfully updated " + stat.name + " to level " + stat.statLevel);
    experienceInfo.updateExperience(route.params.id);
    profStore.getUpdateProficiencies(route.params.id);
    await reloadData();
  }).catch(function() {
    stat.statLevel = oldValue.value;
  })

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
    <h3 class="d-flex justify-content-between">
      <span>Experience Cost: {{newStat.statLevelInfo.totalXP > stat.statLevelInfo.totalXP ? "-" : "+"}}{{ Math.abs(newStat.statLevelInfo.totalXP - stat.statLevelInfo.totalXP) }}</span>
      <span>Available Experience: Infinite</span>
    </h3>
    <DataTable v-model:selection="newStat.statLevelInfo" v-model:expandedRows="expandedRows" :value="statLevels" selection-mode="single" data-key="level">
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
        <SkeletonWrapper :show-skeleton="loading" height="2rem" width="100%" style="padding-left: 3rem; cursor: pointer;" @click="newStat.statLevelInfo = slotProps.data">
          <div >
            {{ slotProps.data.description }}
          </div>
        </SkeletonWrapper>
      </template>
    </DataTable>
  </div>
</template>

<style>
  :deep(th.text-center .p-datatable-column-header-content) {
    justify-content: center;
  }
</style>