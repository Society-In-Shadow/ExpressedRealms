<script setup lang="ts">

import ExpressionSection from "@/components/expressions/ExpressionSection.vue";
import axios from "axios";
import {onBeforeRouteUpdate, useRoute} from 'vue-router'
const route = useRoute()

import {onMounted, ref } from "vue";
import Card from "primevue/card";
import ExpressionToC from "@/components/expressions/ExpressionToC.vue";
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
  <div id="expression" class="d-flex justify-content-center align-items-center boxCenterHelper m-lg-3 m-md-3 m-sm-1 m-1">
    <div class="row">
      <div class="col-lg-3 col-sm-12 col-xl-3 col-md-3">
        <div class="sticky-md-top sticky-lg-top sticky-xl-top zIndexFix">
          <Card class="mb-3 p-0 mt-0 pt-0" style="max-width: 800px">
            <template #title>
              Table Of Contents
            </template>
            <template #content>
              <article id="expression-body">
                <ExpressionToC :sections="sections" :current-level="0" />
              </article>
            </template>
          </Card>
        </div>
      </div>
      <div class="col">
        <Card class="mb-3 p-0 mt-0 pt-0" style="max-width: 800px">
          <template #header>
            <img src="../../../public/ifIHadOne.png" class="w-100">
          </template>
          <template #content>
            <article id="expression-body">
              <ExpressionSection :sections="sections" :current-level="1" />
            </article>
          </template>
        </Card>
      </div>
    </div>
  </div>
</template>

<style>

.zIndexFix {
  z-index: inherit !important;
}

#expression .p-card-content {
  padding-top: 0;
  margin-top: 0;
}

#expression > div > div.p-card-body > div {
  padding-top: 0;
  margin-top: 0;
}

#expression-body > div:nth-child(1) > h1 {
  padding-top: 0;
  margin-top: 0;
}

</style>
