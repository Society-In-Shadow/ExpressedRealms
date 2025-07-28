<script setup lang="ts">

import Button from "primevue/button";
import {onMounted, ref, watch} from "vue";
import axios from "axios";
import {useForm} from "vee-validate";
import {object, string} from "yup";
import InputTextWrapper from "@/FormWrappers/InputTextWrapper.vue";
import DropdownWrapper from "@/FormWrappers/DropdownWrapper.vue";

import { expressionStore } from "@/stores/expressionStore";
import EditorWrapper from "@/FormWrappers/EditorWrapper.vue";
import toaster from "@/services/Toasters";
import {expressionSectionStore} from "@/components/expressions/expressionSection/store/expressionSectionStore";
import type {ListItem} from "@/types/ListItem";
import {getValidationInstance} from "@/components/expressions/expressionSection/validators/expressionSectionValidator";
import FormInputTextWrapper from "@/FormWrappers/FormInputTextWrapper.vue";
import FormDropdownWrapper from "@/FormWrappers/FormDropdownWrapper.vue";
import FormEditorWrapper from "@/FormWrappers/FormEditorWrapper.vue";
const expressionInfo = expressionStore();
const expressionSectionInfo = expressionSectionStore();

const emit = defineEmits<{
  cancelEvent: [],
  addedSection: []
}>();

const props = defineProps({
  parentId: {
    type: Number
  },
  addExpressionHeader:{
    type: Boolean,
    value: false
  }
});

onMounted(() => {
  if(expressionInfo.isDoneLoading) {
    loadSectionInfo();
  }
})

const isHeaderSection = ref(false);

const form = getValidationInstance()

const sectionTypeOptions = ref<Array<ListItem>>([]);

function cancelEdit(){
  emit("cancelEvent");
}

function reset(){
  loadSectionInfo();
}

async function loadSectionInfo(){
  await expressionSectionInfo.getOptions();
  sectionTypeOptions.value = expressionSectionInfo.sectionTypes;
}

const onSubmit = form.handleSubmit(async (values) => {
  await expressionSectionInfo.addSection(values, expressionInfo.currentExpressionId, props.parentId)
  cancelEdit();
  emit("addedSection");
});

</script>

<template>
  <div class="m-2">
    <form @submit="onSubmit">
      <FormInputTextWrapper v-model="form.name" :show-skeleton="expressionSectionInfo.haveSectionTypes" />
      <FormEditorWrapper v-model="form.content" :show-skeleton="expressionSectionInfo.haveSectionTypes" />
      <FormDropdownWrapper
          v-if="!isHeaderSection"
          v-model="form.sectionType" option-label="name" :options="sectionTypeOptions" :show-skeleton="expressionSectionInfo.haveSectionTypes"
      />
      <div class="flex">
        <div class="col-flex flex-grow-1">
          <div class="float-end">
            <Button label="Reset" class="m-2" @click="reset()" />
            <Button v-if="!props.addExpressionHeader" label="Cancel" class="m-2" @click="cancelEdit()" />
            <Button label="Add" class="m-2" @click="onSubmit" />
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
</style>
