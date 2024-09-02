<script setup lang="ts">

import axios from "axios";
import {onMounted, ref, type Ref} from "vue";
import { useRoute } from 'vue-router'
import Button from 'primevue/button';
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";
import StatLevel from "@/components/characters/character/StatLevel.vue";
import Listbox from 'primevue/listbox';
import toasters from "@/services/Toasters";
const route = useRoute()

interface LevelInfo {
  bonus: number;
  description: string;
  level: number;
  totalXP: number;
  xp: number;
  disabled: boolean;
}

interface Stat {
  availableXP: number;
  description: string;
  id: number;
  name: string;
  statLevel: number;
  statLevelInfo: LevelInfo;
}

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
const statLevels:Ref<Array<LevelInfo>> = ref([]);
const loading = ref(true);
const showOptions = ref(false);
const oldValue = ref(props.statTypeId);

onMounted(() =>{
  reloadStatInfo();
});

function reloadStatInfo() {
  axios.get(`/characters/${route.params.id}/stat/${props.statTypeId}`)
      .then((response) => {
        stat.value = response.data;
        loading.value = false;
      })
}

function getEditOptions() {
  axios.get(`/stats/${props.statTypeId}`)
      .then((response) => {
        
        const selectedXP = response.data.find(x => x.level == stat.value.statLevel).totalXP;
        
        response.data.forEach(function(level:LevelInfo) {
          level.disabled = level.totalXP > stat.value.availableXP + selectedXP && level.level > stat.value.statLevel;
        });

        statLevels.value = response.data;
        showOptions.value = true;
      })
}

function handleStatUpdate(stat:Stat){
  // Don't allow them to unselect the option
  if(stat.statLevel == undefined)
  {
    stat.statLevel = oldValue.value;
    showOptions.value = !showOptions.value;
    return;
  }
  axios.put(`/characters/${route.params.id}/stat/${props.statTypeId}`, {
    levelTypeId: stat.statLevel,
    statTypeId: props.statTypeId,
    characterId: route.params.id
  }).then(function(){
    stat.statLevelInfo = statLevels.value.find(x => x.level == stat.statLevel);

    oldValue.value = stat.statLevel;
    
    emit("updateStat", stat.statLevelInfo.level, stat.statLevelInfo.bonus);
    toasters.success("Successfully updated " + stat.name + " to level " + stat.statLevel);
    
    reloadStatInfo();
    showOptions.value = !showOptions.value;
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
              <div v-if="showOptions" class="col text-right">
                {{ stat.availableXP }} XP
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
    <div class="row">
      <div class="col">
        <div v-if="!showOptions" class="p-listbox p-3" style="cursor: pointer" @click="getEditOptions()">
          <StatLevel :stat-level-info="stat.statLevelInfo" :is-loading="loading" :current-level-xp="stat.statLevelInfo.totalXP" :current-level-id="stat.statLevel" :display-only="true" />
        </div>
        <Listbox
          v-else v-model="stat.statLevel" :options="statLevels" option-value="level" option-disabled="disabled"
          @change="handleStatUpdate(stat)"
        >
          <template #option="slotProps">
            <StatLevel :stat-level-info="slotProps.option" :current-level-xp="stat.statLevelInfo.totalXP" :current-level-id="stat.statLevel" />
          </template>
        </Listbox>
      </div>
    </div>
    <div class="row">
      <div class="col">
        <Button data-cy="logoff-button" label="Back" class="w-100 mb-2" @click="emit('toggleStat')" />
      </div>
    </div>
  </div>
</template>
