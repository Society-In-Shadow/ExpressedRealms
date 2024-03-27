<script setup lang="ts">

import ExpressionSection from "@/components/expressions/ExpressionSection.vue";
import axios from "axios";
import {onBeforeRouteUpdate, useRoute} from 'vue-router'
const route = useRoute()

import {onMounted, ref, watch} from "vue";
import Card from "primevue/card";
let sections = ref([]);
function fetchData(name: string) {
  axios.get(`/api/expression/${name}`)
      .then((json) => {
        sections.value = json.data;
      });
}

onMounted(() =>{
  fetchData(route.params.name);
})

onBeforeRouteUpdate(async (to, from) => {
  if (to.params.name !== from.params.name) {
    fetchData(to.params.name)
  }
})

</script>

<template>
  <div id="expression" class="d-flex justify-content-center align-items-center boxCenterHelper m-3">
    <Card class="mb-3 p-0 mt-0 pt-0" style="max-width: 800px">
      <template #header>
        <img src="../../../public/ifIHadOne.png" class="w-100"/>
      </template>
      <template #content class="mt-0 pt-0">
        <article id="expression-body">
          <ExpressionSection :sections="sections" :current-level="1"/>
        </article>
      </template>
    </Card>
  </div>
</template>

<style>

#expression > div > div.p-card-body > div {
  padding-top: 0;
  margin-top: 0;
}

#expression-body > div:nth-child(1) > h1 {
  padding-top: 0;
  margin-top: 0;
}

</style>