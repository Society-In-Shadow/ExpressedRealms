<script setup lang="ts">

import ExpressionSection from "@/components/expressions/ExpressionSection.vue";
import axios from "axios";
import {useRoute} from 'vue-router'
import { expressionStore } from "@/stores/expressionStore";
const expressionInfo = expressionStore();
const route = useRoute()

import {onMounted, ref, nextTick, watch} from "vue";
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

const expressionHeader = ref({});

const isLoading = ref(true);
const headerIsLoading = ref(true);
const showEdit = ref(expressionInfo.canEdit);
const showCreate = ref(false);
const showPreview = ref(false);

async function fetchData() {

  if(route.path.includes("/rulebook")){
    await expressionInfo.getExpressionId("ruleBook");
  }
  else if(route.path.includes("/treasuredtales"))
  {
    await expressionInfo.getExpressionId("treasuredTales");
  }
  else
  {
    await expressionInfo.getExpressionId(route.params.name);
  }
  
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
  await fetchData();
})

watch(
    () => route.path,
    async (newPath, oldPath) => {
      if (newPath !== oldPath) {
        await fetchData()
      }
    }
)

</script>

<template>
  <div id="expression" class="container ms-md-auto me-md-auto ms-0 me-0 container-md p-0">
    <div class="d-flex flex-column flex-md-row ">
      <div class="col-12 col-lg-3 col-sm-12 col-xl-3 col-md-3 p-0 ms-0 me-0 mt-2 mb-2 m-md-2">
        <Card class="custom-toc sticky-md-top sticky-lg-top sticky-xl-top zIndexFix">
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
      <div class="col p-0 ms-0 me-0 mt-2 mb-2 m-md-2">
        <Card class="custom-card" style="max-width: 800px">
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

@media(min-width: 768px){
  .container-md {
    width: 100%;
    max-width:1000px
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
