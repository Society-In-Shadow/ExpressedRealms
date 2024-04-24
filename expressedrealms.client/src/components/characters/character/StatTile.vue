<script setup lang="ts">

import axios from "axios";
import {onMounted, ref} from "vue";
import { useRoute } from 'vue-router'
import Button from 'primevue/button';
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";
import StatLevel from "@/components/characters/character/StatLevel.vue";
import Listbox from 'primevue/listbox';
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

const stat = ref({
  statLevelInfo: {}
});
const statLevels = ref([]);
const loading = ref(true);
const showOptions = ref(false);
const oldValue = ref(props.statTypeId);

onMounted(() =>{
  axios.get(`/api/characters/${route.params.id}/stat/${props.statTypeId}`)
      .then((response) => {
        stat.value = response.data;
        loading.value = false;
      })
});

function getEditOptions() {
  axios.get(`/api/stats/${props.statTypeId}`)
      .then((response) => {
        statLevels.value = response.data;
        showOptions.value = true;
      })
}

function handleStatUpdate(stat){
  // Don't allow them to unselect the option
  if(stat.statLevel == undefined)
    stat.statLevel = oldValue.value;

  axios.put(`/api/characters/${route.params.id}/stat/${props.statTypeId}`, {
    levelTypeId: stat.statLevel,
    statTypeId: props.statTypeId,
    characterId: route.params.id
  }).then(function(){
    stat.statLevelInfo = statLevels.value.find(x => x.level == stat.statLevel);

    emit("updateStat", stat.statLevelInfo.level, stat.statLevelInfo.bonus);

    showOptions.value = !showOptions.value;
  })

}

</script>

<template>
  <div class="w-100" style="min-width: 350px">
    <div class="row">
      <div class="col">
        <h3 class="mt-0">
          <SkeletonWrapper :show-skeleton="loading" height="2rem">
            {{ stat.name }}
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
          <StatLevel :stat-level-info="stat.statLevelInfo" :is-loading="loading" />
        </div>
        <Listbox v-else v-model="stat.statLevel" :options="statLevels" option-value="level" @change="handleStatUpdate(stat)">
          <template #option="slotProps">
            <StatLevel :stat-level-info="slotProps.option" />
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

<style scoped>

</style>