<script setup lang="ts">

import Skeleton from "primevue/skeleton";
import Button from "primevue/button";
import {onMounted, ref} from "vue";

import { expressionStore } from "@/stores/expressionStore";
import DataTable from "primevue/datatable";
import {getValidationInstance} from "@/components/expressions/expressionSection/validators/expressionSectionValidator";
import FormInputTextWrapper from "@/FormWrappers/FormInputTextWrapper.vue";
import FormEditorWrapper from "@/FormWrappers/FormEditorWrapper.vue";
import FormDropdownWrapper from "@/FormWrappers/FormDropdownWrapper.vue";
import {expressionSectionStore} from "@/components/expressions/expressionSection/store/expressionSectionStore";
import {
  expressionSectionConfirmationPopup
} from "@/components/expressions/expressionSection/services/expressionSectionConfirmationPopup";

const expressionInfo = expressionStore();
const expressionSectionInfo = expressionSectionStore();

const emit = defineEmits<{
  canceled: []
}>();

const cancel = () => {
  emit("canceled");
}

const props = defineProps({
  sectionId: {
    type: Number,
    required: true,
  }
});

const popups = expressionSectionConfirmationPopup(props.sectionId);
const showOptionLoader = ref(true);
const sectionTypeOptions = ref([]);
const showSkeleton = ref(true);
const isHeaderSection = ref(false);

const form = getValidationInstance()

async function reset(){
  showOptionLoader.value = true;
  await loadSectionInfo();
}

onMounted(async () => {
  await loadSectionInfo();
})

async function loadSectionInfo(){
  const expressionSection = await expressionSectionInfo.getExpressionSection(expressionInfo.currentExpressionId, props.sectionId);
  isHeaderSection.value = expressionSection.isHeaderSection;
  form.setValues(expressionSection);
  showOptionLoader.value = false;
  showSkeleton.value = false;
}

const onSubmit = form.handleSubmit(async (values) => {
  await expressionSectionInfo.updateSection(values, expressionInfo.currentExpressionId, props.sectionId);
})

</script>

<template>
  <div class="d-none">
    <DataTable />
  </div>
  <div v-if="showSkeleton">
    <Skeleton id="expression-section-title-skeleton" class="mb-2" height="1.5em" />
    <Skeleton id="expression-section-body-skeleton" class="mb-2" height="5em" />
  </div>
  <div v-else class="m-2">
    <form @submit="onSubmit">
      <FormInputTextWrapper v-model="form.name" :show-skeleton="showOptionLoader" />
      <FormEditorWrapper v-model="form.content" :show-skeleton="showOptionLoader" />
      <FormDropdownWrapper
        v-if="!isHeaderSection"
        v-model="form.sectionType" option-label="name" :options="sectionTypeOptions" :show-skeleton="showOptionLoader"
      />
      <div class="flex">
        <div class="col-flex flex-grow-1">
          <Button v-if="!isHeaderSection" severity="danger" label="Delete" class="m-2" @click="popups.deleteConfirmation($event)" />
          <div class="float-end">
            <Button label="Reset" class="m-2" @click="reset()" />
            <Button label="Cancel" class="m-2" @click="cancel()" />
            <Button label="Save" class="m-2" @click="onSubmit" />
          </div>
        </div>
      </div>
    </form>
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
