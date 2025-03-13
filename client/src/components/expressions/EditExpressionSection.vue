<script setup lang="ts">

import {makeIdSafe} from "@/utilities/stringUtilities";
import Skeleton from "primevue/skeleton";
import Button from "primevue/button";
import {ref} from "vue";
import axios from "axios";
import {useForm} from "vee-validate";
import {object, string} from "yup";
import InputTextWrapper from "@/FormWrappers/InputTextWrapper.vue";
import DropdownWrapper from "@/FormWrappers/DropdownWrapper.vue";

import { expressionStore } from "@/stores/expressionStore";
import EditorWrapper from "@/FormWrappers/EditorWrapper.vue";
import toaster from "@/services/Toasters";
import CreateExpressionSection from "@/components/expressions/CreateExpressionSection.vue";
import {useConfirm} from "primevue/useconfirm";
const expressionInfo = expressionStore();

const emit = defineEmits<{
  refreshList: []
}>();

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
  }
});

const showEditor = ref(false);
const showOptionLoader = ref(true);
const sectionTypeOptions = ref([]);
const showCreate = ref(false);

function toggleEditor(){
  showEditor.value = !showEditor.value;
  loadSectionInfo();
}

function passThroughAddedSection(){
  emit("refreshList");
}

function cancelEdit(){
  showEditor.value = !showEditor.value;
}

function reset(){
  showOptionLoader.value = true;
  loadSectionInfo();
}

function loadSectionInfo(){
  if(!showOptionLoader.value) return; // Don't load in 2nd time
  axios.get(`/expressionSubSections/${expressionInfo.currentExpressionId}/${props.sectionInfo.id}/options`)
      .then(async (response) => {

        sectionTypeOptions.value = response.data.sectionTypes;

        axios.get(`/expressionSubSections/${expressionInfo.currentExpressionId}/${props.sectionInfo.id}`)
            .then(async (json) => {
              name.value = json.data.name;
              content.value = json.data.content;
              sectionType.value = sectionTypeOptions.value.find(x => x.id == json.data.sectionTypeId);
              showOptionLoader.value = false;
            });
      });
}

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    name: string().required()
        .label('Name'),
    content: string()
        .required()
        .label('Content'),
    parentSection: object().nullable()
        .label('Parent Section'),
    sectionType: object().nullable()
        .label('Section Type')
  })
});

const [name] = defineField('name');
const [content] = defineField('content');
const [parentSection] = defineField('parentSection');
const [sectionType] = defineField('sectionType');

function toggleCreate(){
  showCreate.value = !showCreate.value;
}

const onSubmit = handleSubmit((values) => {
  axios.put(`/expressionSubSections/${expressionInfo.currentExpressionId}/${props.sectionInfo.id}`, {
    name: values.name,
    content: values.content,
    sectionTypeId: values.sectionType.id,
  }).then(() => {
    props.sectionInfo.name = values.name;
    props.sectionInfo.content = values.content;
    showEditor.value = false;
    toaster.success("Successfully Updated Expression Section Info!");
  });
});

const confirm = useConfirm();
const deleteExpression = (event) => {
  confirm.require({
    target: event.currentTarget,
    header: 'Deleting Section',
    message: `Are you sure you want delete ${props.sectionInfo.name} section?  This will delete this section and any sub children`,
    icon: 'pi pi-exclamation-triangle',
    group: 'popup',
    rejectProps: {
      label: 'Cancel',
      severity: 'secondary',
      outlined: true
    },
    acceptProps: {
      label: 'Save'
    },
    accept: () => {
      axios.delete(`/expressionSubSections/${expressionInfo.currentExpressionId}/${props.sectionInfo.id}`).then(() => {
        emit('refreshList');
        toaster.success(`Successfully Deleted Section ${props.sectionInfo.name}!`);
      });
    },
    reject: () => {}
  });
};

</script>

<template>
  <div v-if="showSkeleton">
    <Skeleton id="expression-section-title-skeleton" class="mb-2" height="1.5em" />
    <Skeleton id="expression-section-body-skeleton" class="mb-2" height="5em" />
  </div>
  <div v-else-if="showEditor" class="m-2">
    <form @submit="onSubmit">
      <InputTextWrapper v-model="name" field-name="Name" :error-text="errors.name" :show-skeleton="showOptionLoader" />
      <EditorWrapper v-model="content" field-name="Content" :error-text="errors.content" :show-skeleton="showOptionLoader" />
      <DropdownWrapper
        v-model="sectionType" option-label="name" :options="sectionTypeOptions" field-name="Section Types" :show-skeleton="showOptionLoader"
        :error-text="errors.sectionType"
      />
      <div class="flex">
        <div class="col-flex flex-grow-1">
          <Button severity="danger" label="Delete" class="m-2" @click="deleteExpression($event)" />
          <div class="float-end">
            <Button label="Reset" class="m-2" @click="reset()" />
            <Button label="Cancel" class="m-2" @click="cancelEdit()" />
            <Button label="Save" class="m-2" @click="onSubmit" />
          </div>
        </div>
      </div>
    </form>
  </div>
  <div v-else>
    <div class="flex">
      <div class="col-flex flex-grow-1">
        <h1 v-if="currentLevel == 1" :id="makeIdSafe(sectionInfo.name)">
          {{ sectionInfo.name }}
        </h1>
        <h2 v-if="currentLevel == 2" :id="makeIdSafe(sectionInfo.name)">
          {{ sectionInfo.name }}
        </h2>
        <h3 v-if="currentLevel == 3" :id="makeIdSafe(sectionInfo.name)">
          {{ sectionInfo.name }}
        </h3>
        <h4 v-if="currentLevel == 4" :id="makeIdSafe(sectionInfo.name)">
          {{ sectionInfo.name }}
        </h4>
        <h5 v-if="currentLevel == 5" :id="makeIdSafe(sectionInfo.name)">
          {{ sectionInfo.name }}
        </h5>
        <h6 v-if="currentLevel == 6" :id="makeIdSafe(sectionInfo.name)">
          {{ sectionInfo.name }}
        </h6>
      </div>
      <div class="col-flex">
        <Button v-if="showEdit" label="Add Child Section" class="m-2" @click="toggleCreate" />
        <Button v-if="!showEditor && showEdit" label="Edit" class="float-end m-2" @click="toggleEditor()" />
      </div>
    </div>
    <div class="mb-2 fix-wrapping" v-html="props.sectionInfo.content" />
  </div>
  <div v-if="showCreate && showEdit">
    <CreateExpressionSection :parent-id="props.sectionInfo.id" @cancel-event="toggleCreate" @added-section="passThroughAddedSection()" />
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
</style>
