<script setup lang="ts">

import Fieldset from 'primevue/fieldset';
import axios from "axios";
import {onMounted, ref} from "vue";
import { useRoute } from 'vue-router'
const route = useRoute()
const stats = ref([]);

onMounted(() =>{
  axios.get(`/api/stats/${route.params.id}/smallStats`)
      .then((response) => {
        stats.value = response.data;
      })
});

</script>

<template>
  <div class="flex flex-wrap justify-content-center column-gap-3 row-gap-3" style="max-width: 350px">
    <div v-for="stat in stats" class="align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch m-0 p-0">
      <Fieldset :legend="stat.shortName" class="statBlock">
        <div class=""><strong>{{stat.bonus}}</strong></div> <br/>
        <div><small>{{stat.level}}</small></div>
      </Fieldset>
    </div>
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