<script setup lang="ts">

import Button from "primevue/button";
import {onMounted, ref} from "vue";
import axios from "axios";
import {useForm} from "vee-validate";
import {object, string} from "yup";
import InputTextWrapper from "@/FormWrappers/InputTextWrapper.vue";
import DropdownWrapper from "@/FormWrappers/DropdownWrapper.vue";

import { expressionStore } from "@/stores/expressionStore";
import EditorWrapper from "@/FormWrappers/EditorWrapper.vue";
import toaster from "@/services/Toasters";
const expressionInfo = expressionStore();

const emit = defineEmits<{
  cancelEvent: [],
  addedSection: []
}>();

const props = defineProps({
  parentId: {
    type: Number
  },
});

onMounted(() => {
  loadSectionInfo();
})

const showOptionLoader = ref(true);
const sectionTypeOptions = ref([]);

function cancelEdit(){
  emit("cancelEvent");
}

function reset(){
  showOptionLoader.value = true;
  loadSectionInfo();
}

function loadSectionInfo(){
  if(!showOptionLoader.value) return; // Don't load in 2nd time
  console.log('expressionid' + expressionInfo.currentExpressionId);
  axios.get(`/expressionSubSections/${expressionInfo.currentExpressionId}/0/options`)
      .then(async (response) => {
        sectionTypeOptions.value = response.data.sectionTypes;
        showOptionLoader.value = false;
      });
}

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    name: string().required()
        .label('Name'),
    content: string()
        .required()
        .label('Content'),
    sectionType: object().nullable()
        .label('Section Type')
  })
});

const [name] = defineField('name');
const [content] = defineField('content');
const [sectionType] = defineField('sectionType');

const onSubmit = handleSubmit((values) => {
  axios.post(`/expressionSubSections/${expressionInfo.currentExpressionId}`, {
    name: values.name,
    content: values.content,
    sectionTypeId: values.sectionType.id,
    parentId: props.parentId
  }).then(() => {
    emit("addedSection");
    toaster.success("Successfully Added Expression Section Info!");
    cancelEdit();
  });
});

</script>

<template>
  <div class="m-2">
    <form @submit="onSubmit">
      <InputTextWrapper v-model="name" field-name="Name" :error-text="errors.name" />
      <EditorWrapper v-model="content" field-name="Content" :error-text="errors.content" />
      <DropdownWrapper
        v-model="sectionType" option-label="name" :options="sectionTypeOptions" field-name="Section Types" :show-skeleton="showOptionLoader"
        :error-text="errors.sectionType"
      />
      <div class="flex">
        <div class="col-flex flex-grow-1">
          <div class="float-end">
            <Button label="Reset" class="m-2" @click="reset()" />
            <Button label="Cancel" class="m-2" @click="cancelEdit()" />
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
