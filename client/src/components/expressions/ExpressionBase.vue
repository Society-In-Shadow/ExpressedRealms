<script setup lang="ts">

import ExpressionSection from "@/components/expressions/ExpressionSection.vue";
import axios from "axios";
import {onBeforeRouteUpdate, useRoute} from 'vue-router'
import { expressionStore } from "@/stores/expressionStore";
const expressionInfo = expressionStore();
const route = useRoute()

import {onMounted, ref, nextTick } from "vue";
import Card from "primevue/card";
import ScrollTop from 'primevue/scrolltop';
import CreateExpressionSection from "@/components/expressions/CreateExpressionSection.vue";
import Button from "primevue/button";
import '@he-tree/vue/style/default.css'
import '@he-tree/vue/style/material-design.css'
import ExpressionToC from "@/components/expressions/ExpressionToC.vue";
let sections = ref([
  {
    id: 1,
    subSections: [
      { id: 2, subSections: []},
      { id: 3, subSections: []},
      { id: 4, subSections: []}
    ]
  },
  {
    id: 5,
    subSections: []
  },
  {
    id: 6,
    subSections: [{id: 7}]
  },
  {
    id: 8,
    subSections: [{id: 9,}]
  }
]);
const isLoading = ref(true);
const showEdit = ref(false);
const showCreate = ref(false);
const showPreview = ref(false);

function fetchData(name: string) {
  axios.get(`/expressionSubSections/${name}`)
      .then(async (json) => {
        sections.value = json.data.expressionSections;
        isLoading.value = false;
        expressionInfo.currentExpressionId = json.data.expressionId;
        showEdit.value = json.data.canEditPolicy
        if(location.hash){
          await nextTick();
          window.location.replace(location.hash);
        }        
      });
}

function toggleCreate(){
  showCreate.value = !showCreate.value;
}

function togglePreview(){
  showPreview.value = !showPreview.value;
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
  <div id="expression" class="container">
    <div class="row">
      <div class="col-12 col-lg-3 col-sm-12 col-xl-3 col-md-3">
        <Card class="sticky-md-top sticky-lg-top sticky-xl-top zIndexFix">
          <template #title>
            Table Of Contents
          </template>
          <template #content>
            <article id="expression-body">
              <ExpressionToC v-model="sections" :can-edit="showEdit" :show-skeleton="isLoading" @toggle-preview="togglePreview" />
            </article>
          </template>
        </Card>
      </div>
      <div class="col">
        <Card class="mb-3 p-0 mt-0 pt-0" style="max-width: 800px">
          <template #content>
            <article id="expression-body">
              <ExpressionSection :sections="sections" :current-level="1" :show-skeleton="isLoading" :show-edit="showEdit && !showPreview" @refresh-list="fetchData(route.params.name)" />
              <Button v-if="showEdit && !showPreview" label="Add Section" class="m-2" @click="toggleCreate" />
              <div v-if="showCreate">
                <CreateExpressionSection @cancel-event="toggleCreate" @added-section="fetchData(route.params.name)" />
              </div>
            </article>
          </template>
        </Card>
      </div>
    </div>
    <ScrollTop />
  </div>
</template>

<style>

.container {
  width: 100%; 
  margin-right: auto; 
  margin-left: auto; 
  max-width:1000px
}

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
