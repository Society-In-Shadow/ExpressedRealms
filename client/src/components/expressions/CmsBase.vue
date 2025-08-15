<script setup lang="ts">

import ExpressionSection from "@/components/expressions/ExpressionSection.vue";
import {useRoute} from 'vue-router'
import {expressionStore} from "@/stores/expressionStore";
import {nextTick, onMounted, ref, watch} from "vue";
import Card from "primevue/card";
import ScrollTop from 'primevue/scrolltop';
import CreateExpressionSection from "@/components/expressions/CreateExpressionSection.vue";
import Button from "primevue/button";
import '@he-tree/vue/style/default.css'
import '@he-tree/vue/style/material-design.css'
import ExpressionToC from "@/components/expressions/ExpressionToC.vue";
import axios from "axios";
import {FeatureFlags, userStore} from "@/stores/userStore.ts";

const expressionInfo = expressionStore();
const route = useRoute()
const userInfo = userStore()

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
const showEdit = ref(expressionInfo.canEdit);
const showCreate = ref(false);
const showPreview = ref(false);
const showReportButton = ref(false);

async function fetchData() {

  await expressionInfo.getExpressionId(route);
  
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
}

function toggleCreate(){
  showCreate.value = !showCreate.value;
}

function togglePreview(){
  showPreview.value = !showPreview.value;
}

onMounted(async () =>{
  await fetchData();
  showReportButton.value = await userInfo.hasFeatureFlag(FeatureFlags.ShowReportButtons);
})

watch(
    () => route.path,
    async (newPath, oldPath) => {
      if (newPath !== oldPath) {
        await fetchData()
      }
    }
)

async function downloadExpressionBooklet() {
  const res = await axios.get(`/expression/${expressionInfo.currentExpressionId}/report`, {
    responseType: 'blob',
  });
  const expression = route.name
  const url = URL.createObjectURL(res.data);
  const a = document.createElement('a');
  a.href = url;
  a.download = `${expression}Booklet.pdf`;
  document.body.appendChild(a);
  a.click();
  a.remove();
  URL.revokeObjectURL(url);
}

</script>

<template>
    <div id="expression" class="d-flex flex-column flex-md-row gap-2 ">
      <Card class="custom-toc flex-grow-0 sticky-md-top d-print-none zIndexFix">
        <template #title>
          Table Of Contents
        </template>
        <template #content>
          <article id="expression-body">
            <ExpressionToC v-model="sections" :can-edit="showEdit" :show-skeleton="isLoading" @toggle-preview="togglePreview" />
          </article>
        </template>
      </Card>
      <Card class="custom-card flex-grow-1">
        <template #content>
          <article id="expression-body">
            <div class="d-flex flex-row justify-content-end align-items-center" v-if="showReportButton">
              <Button label="Download Booklet" @click="downloadExpressionBooklet()"/>
            </div>
            <ExpressionSection :sections="sections" :current-level="1" :show-skeleton="isLoading" :show-edit="showEdit && !showPreview" @refresh-list="fetchData(route.params.name)" />
            <Button v-if="showEdit && !showPreview" label="Add Section" class="m-2" @click="toggleCreate" />
            <div v-if="showCreate">
              <CreateExpressionSection @cancel-event="toggleCreate" @added-section="fetchData(route.params.name)" />
            </div>
          </article>
        </template>
      </Card>
    </div>
    <ScrollTop />
</template>

<style>

@media(min-width: 768px){
  .custom-toc {
    max-height: calc(100vh - 1rem);
    overflow-y: auto;
    height:100%;
    min-width: 18em;
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
