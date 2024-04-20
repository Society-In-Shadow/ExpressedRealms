<script setup lang="ts">
  import axios from "axios";
  import {onMounted, ref} from "vue";
  import Listbox from 'primevue/listbox';

  import { useRoute } from 'vue-router'
  const route = useRoute()
  
  const stats = ref([]);
  
  onMounted(() =>{
    axios.get(`/api/stats/${route.params.id}`)
        .then((response) => {
          stats.value = response.data;
        })
  });

</script>

<template>

  <div v-for="stat in stats">
    <h3>{{stat.name}}</h3>
    <div class="mb-3">{{stat.description}}</div>
    
    <Listbox v-model="stat.statLevel" :options="stat.statLevels" class="w-100" option-value="level">
      <template #option="slotProps">
        <div class="row">
          <div class="col">
            <div class="row">
              <div class="col-4 text-center">
                <div>Level</div>
                <div>{{slotProps.option.level}}</div>
              </div>
              <div class="col-4 text-center">
                <div>Bonus</div>
                <div>{{slotProps.option.bonus}}</div>
              </div>
              <div class="col-4 text-center">
                <div>XP</div>
                <div>{{slotProps.option.xp}}</div>
              </div>
            </div>
            <div class="row">
              <div class="col">
                {{slotProps.option.description}}
              </div>
            </div>
          </div>
        </div>
      </template>
    </Listbox>
  </div>

</template>

<style scoped>

</style>