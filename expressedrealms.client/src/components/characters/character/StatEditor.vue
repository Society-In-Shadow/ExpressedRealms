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
          stats.value = response.data;
        })
  });
  
  function getSelectedStatInfo(level:number, levels){
    return levels.find(x => x.level == level);
  }

</script>

<template>

  <div v-for="stat in stats" style="max-width: 500px">
    <h3>{{stat.name}}</h3>
    <div class="mb-3">{{stat.description}}</div>
    
    <StatTile :state-info="getSelectedStatInfo(stat.statLevel, stat.statLevels)"></StatTile>
    
    <Listbox v-model="stat.statLevel" :options="stat.statLevels" option-value="level">
      <template #option="slotProps">
        <StatTile :state-info="slotProps.option"></StatTile>
      </template>
    </Listbox>
  </div>

</template>

<style scoped>

</style>