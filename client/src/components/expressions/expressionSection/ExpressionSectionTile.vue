<script setup lang="ts">

import {makeIdSafe} from "@/utilities/stringUtilities";
import Skeleton from "primevue/skeleton";
import Button from "primevue/button";
import {ref} from "vue";
import DataTable from "primevue/datatable";
import KnowledgeList from "@/components/knowledges/KnowledgeList.vue";
import EditExpressionSection from "@/components/expressions/expressionSection/EditExpressionSection.vue";


const props = defineProps({
  sectionInfo: {
    type: Object,
    required: true,
  },
  currentLevel: {
    type: Number,
    required: true
  },
  showSkeleton:{
    type: Boolean,
    required: true
  },
  showEdit:{
    type: Boolean,
    required: true
  },
  isHeaderSection:{
    type: Boolean
  }
});

const showEditor = ref(false);
const showCreate = ref(false);

function toggleCreate(){
  showCreate.value = !showCreate.value;
}

function toggleEditor(){
  showEditor.value = !showEditor.value;
}

</script>

<template>
  <div class="d-none">
    <DataTable />
  </div>
  <div v-if="showSkeleton">
    <Skeleton id="expression-section-title-skeleton" class="mb-2" height="1.5em" />
    <Skeleton id="expression-section-body-skeleton" class="mb-2" height="5em" />
  </div>
  <div v-else-if="showEditor" class="m-2">
    <EditExpressionSection :section-id="props.sectionInfo.id"/>
  </div>
  <div v-else>
    <div class="flex">
      <div class="col-flex flex-grow-1">
        <component
            :is="`h${Math.min(Math.max(currentLevel, 1), 6)}`"
            :id="makeIdSafe(sectionInfo.name)">
          {{ sectionInfo.name }}
        </component>
      </div>
      <div v-if="props.sectionInfo.sectionTypeName != 'Knowledges Section'" class="col-flex">
        <Button v-if="showEdit && !isHeaderSection" label="Add Child Section" class="m-2" @click="toggleCreate" />
        <Button v-if="!showEditor && showEdit" label="Edit" class="float-end m-2" @click="toggleEditor()" />
      </div>
    </div>
    <div v-if="props.sectionInfo.sectionTypeName === 'Knowledges Section'">
      <KnowledgeList :is-read-only="!showEdit" />
    </div>

    <div v-else class="mb-2 fix-wrapping" v-html="props.sectionInfo.content" />
  </div>
</template>

<style>
div.ql-editor > p {
  font-family: var(--font-family);
  font-feature-settings: var(--font-feature-settings, normal);
  font-size: 1rem;
  font-weight: normal;
  margin-top: 1em;
  margin-bottom: 1em;
}

.fix-wrapping {
  overflow-wrap: break-word;
}

.custom-table {
  width: 100%; /* Full width table */
  border-collapse: collapse; /* Clean collapse of borders */
}

.custom-table tr {
  background-color: var(--surface-ground); /* PrimeVue table row color */
  border-bottom: 1px solid var(--surface-border); /* Row bottom border */
}

.custom-table tr:nth-child(odd) {
  background: var(--p-datatable-row-striped-background);
}

.custom-table td {
  text-align: start;
  border-color: var(--p-datatable-body-cell-border-color);
  border-style: solid;
  border-width: 0 0 1px 0;
  padding: var(--p-datatable-body-cell-padding);
}

.p-editor-content .custom-table td {
  border-width: 3px;
}

.p-editor-content .custom-table th {
  border-width: 3px;
}

.custom-table td p{
  margin: 0px;
  padding: 0px;
  min-height: 1em;
}

.custom-table th p{
  margin: 0px;
  padding: 0px;
  min-height: 1em;
}

.custom-table th {
  padding: var(--p-datatable-header-cell-padding);
  background: var(--p-datatable-header-cell-background);
  border-color: var(--p-datatable-header-cell-border-color);
  border-style: solid;
  border-width: 0 0 1px 0;
  color: var(--p-datatable-header-cell-color);
  font-weight: normal;
  text-align: start;
  transition: background var(--p-datatable-transition-duration), color var(--p-datatable-transition-duration), border-color var(--p-datatable-transition-duration), outline-color var(--p-datatable-transition-duration), box-shadow var(--p-datatable-transition-duration);
}

.custom-table-container {
  display: block; /* Container for overflow */
  margin: 1rem 0; /* Add space around the table */
  overflow-x: auto; /* Enable horizontal scrolling for small screens */
  border: 1px solid var(--surface-border); /* Border matching PrimeVue style */
  border-radius: 4px; /* Rounding of corners */
  background-color: var(--surface-card); /* Background color for container */
}

</style>
