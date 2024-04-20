<script setup lang="ts">
  import axios from "axios";
  import {onMounted, ref} from "vue";
  import Listbox from 'primevue/listbox';
  import { useRoute } from 'vue-router'
  import StatTile from "@/components/characters/character/StatTile.vue";
  const route = useRoute()
  
  const stats = ref([]);
  
  onMounted(() =>{
    axios.get(`/api/stats/${route.params.id}`)
        .then((response) => {
          
          response.data.forEach(function(stat) {
            stat.showOptions = false;
            stat.oldValue = stat.statLevel;
          });
          
          stats.value = response.data;
        })
  });
  
  function getSelectedStatInfo(level:number, levels){
    return levels.find(x => x.level == level);
  }

  function handleStatUpdate(stat){
    // Don't allow them to unselect the option
    if(stat.statLevel == undefined)
      stat.statLevel = stat.oldValue;

    stat.showOptions = !stat.showOptions
    document.getElementById(stat.name).scrollIntoView(true)
  }
</script>

<template>

  <div v-for="stat in stats" style="max-width: 500px" :id="stat.name">
    <h3>{{stat.name}}</h3>
    <div class="mb-3">{{stat.description}}</div>
    
    <div v-if="!stat.showOptions" class="p-listbox">
      <StatTile :state-info="getSelectedStatInfo(stat.statLevel, stat.statLevels)" @click="stat.showOptions = !stat.showOptions" class="p-3"></StatTile>
    </div>
    <Listbox v-else v-model="stat.statLevel" :options="stat.statLevels" option-value="level" @change="handleStatUpdate(stat)">
      <template #option="slotProps">
        <StatTile :state-info="slotProps.option"></StatTile>
      </template>
    </Listbox>
  </div>

</template>

<style scoped>

</style>