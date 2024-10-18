<script setup lang="ts">

import {makeIdSafe} from "@/utilities/stringUtilities";
import Skeleton from "primevue/skeleton";
import Editor from "primevue/editor";
import Button from "primevue/button";

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
import { ref } from "vue";

const showEditor = ref(false);

</script>

<template>
  <div v-if="showSkeleton">
    <Skeleton id="expression-section-title-skeleton" class="mb-2" height="1.5em" />
    <Skeleton id="expression-section-body-skeleton" class="mb-2" height="5em" />
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
        <Button v-if="!showEditor" label="Edit" @click="showEditor = !showEditor" class="float-end m-2"></Button>
      </div>
    </div>


    <Editor v-if="showEditor" v-model="props.sectionInfo.content">
      <template v-slot:toolbar>
        <span class="ql-formats">
            <button v-tooltip.bottom="'Bold'" class="ql-bold"></button>
            <button v-tooltip.bottom="'Italic'" class="ql-italic"></button>
            <button v-tooltip.bottom="'Underline'" class="ql-underline"></button>
        </span>
      </template>
    </Editor>
    <div v-else v-html="props.sectionInfo.content" class="mb-2"></div>
    <div class="flex">
      <div class="col-flex flex-grow-1">
        <div class="float-end">
          <Button v-if="showEditor" label="Cancel" @click="showEditor = !showEditor" class="m-2"></Button>
          <Button v-if="showEditor" label="Save" @click="showEditor = !showEditor" class="m-2"></Button>
        </div>
      </div>
    </div>

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