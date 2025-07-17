<script setup lang="ts">

import Tabs from 'primevue/tabs';
import TabList from 'primevue/tablist';
import Tab from 'primevue/tab';
import TabPanels from 'primevue/tabpanels';
import TabPanel from 'primevue/tabpanel';

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
import EditExpressionSection from "@/components/expressions/EditExpressionSection.vue";
import PowerTab from "@/components/expressions/powers/PowerTab.vue";
import PowersToC from "@/components/expressions/PowersToC.vue";

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

const expressionHeader = ref({});

const isLoading = ref(true);
const headerIsLoading = ref(true);
const showEdit = ref(expressionInfo.canEdit);
const showCreate = ref(false);
const showPreview = ref(false);
const currentTab = ref('0');

async function fetchData() {
  await expressionInfo.getExpressionSections()
      .then(async () => {
        sections.value = expressionInfo.sections;
        showEdit.value = expressionInfo.canEdit;
        isLoading.value = false;
        if(location.hash){
          await nextTick();
          window.location.replace(location.hash);
        }        
      });
  
  headerIsLoading.value = true;
  await axios.get(`/expressionSubSections/${expressionInfo.currentExpressionId}/expression`)
      .then(async (json) => {
        expressionHeader.value = json.data;
        headerIsLoading.value = false;
      });
}

function toggleCreate(){
  showCreate.value = !showCreate.value;
}

function togglePreview(){
  showPreview.value = !showPreview.value;
}

onMounted(async () =>{
  await expressionInfo.getExpressionId(route.params.name);
  await fetchData();
})

onBeforeRouteUpdate(async (to, from) => {
  if (to.params.name !== from.params.name) {
    await expressionInfo.getExpressionId(to.params.name);
    await fetchData()
  }
})

</script>

<template>
  <div id="expression" class="ms-md-auto me-md-auto ms-0 me-0 container-md p-0">
    <div class="d-flex flex-column flex-md-row">
      <div class="col-12 col-lg-3 col-sm-12 col-xl-3 col-md-3 p-0 ms-0 me-0 mt-2 mb-2 m-md-2">
        <Card class="custom-toc sticky-md-top sticky-lg-top sticky-xl-top zIndexFix">
          <template #title>
            Table Of Contents
          </template>
          <template #content>
            <article id="expression-body">
              <ExpressionToC v-if="currentTab == '0'" v-model="sections" :can-edit="showEdit" :show-skeleton="isLoading" @toggle-preview="togglePreview" />
              <PowersToC v-else :show-skeleton="isLoading" />
            </article>
          </template>
        </Card>
      </div>
      <div class="col p-0 ms-0 me-0 mt-2 mb-2 m-md-2">
        <Card class="custom-card" style="max-width: 800px">
          <template #content>
            <div class="pb-4">
              <div class="d-flex flex-column flex-md-row">
                <div class="col-12 order-1 order-md-0 col-md-8">
                  <CreateExpressionSection v-if="expressionHeader.id === 0" :add-expression-header="true" @added-section="fetchData(route.params.name)" />
                  <EditExpressionSection
                    v-else :section-info="expressionHeader" :current-level="1" :show-skeleton="headerIsLoading" :show-edit="showEdit"
                    :is-header-section="true"
                  />
                </div>
                <div class="col-12 align-self-center text-center order-0 order-md-1 col-md-4">
                  <img src="/IfIHadOne2.jpg" class="w-100" style="max-width: 250px" alt="Timmy Turners Father Regretting not having a trophy">
                </div>
              </div>
            </div>
            <Tabs v-model:value="currentTab">
              <TabList>
                <Tab value="0">
                  Background
                </Tab>
                <Tab v-if="expressionInfo.showPowersTab" value="1">
                  Powers
                </Tab>
              </TabList>
              <TabPanels class="">
                <TabPanel value="0">
                  <article id="expression-body">
                    <ExpressionSection :sections="sections" :current-level="1" :show-skeleton="isLoading" :show-edit="showEdit && !showPreview" @refresh-list="fetchData(route.params.name)" />
                    <Button v-if="showEdit && !showPreview" label="Add Section" class="m-2" @click="toggleCreate" />
                    <div v-if="showCreate">
                      <CreateExpressionSection @cancel-event="toggleCreate" @added-section="fetchData(route.params.name)" />
                    </div>
                  </article>
                </TabPanel>
                <TabPanel v-if="expressionInfo.showPowersTab" value="1">
                  <PowerTab v-if="expressionInfo.isDoneLoading" :expression-id="expressionInfo.currentExpressionId" />
                </TabPanel>
              </TabPanels>
            </Tabs>
          </template>
        </Card>
      </div>
    </div>
    <ScrollTop />
  </div>
</template>

<style>

@media(min-width: 768px){
  .container-md {
    width: 100%;
    max-width:1000px
  }
  .custom-toc {
    max-height: calc(100vh - 1rem);
    overflow-y: auto;
  }
}

@media(max-width: 768px){ 
  .custom-card > .p-card-body{
    padding: 0.75rem !important;
  }
  
  .custom-toc > .p-card-body{
    padding-left: 1rem !important; 
    padding-right: 1rem !important; 
  }
  
  .custom-card .p-tabpanels{
    padding: 0.5rem !important;
  }
}

.zIndexFix {
  z-index: inherit !important;
}

</style>
