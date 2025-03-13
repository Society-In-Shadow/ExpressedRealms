<script setup lang="ts">

import {computed} from "vue";
import Skeleton from 'primevue/skeleton';
import Editor from "primevue/editor";
import Button from "primevue/button";

const model = defineModel<string>({ required: true, default: "" });

defineOptions({
  inheritAttrs: false
})

const props = defineProps({
  fieldName: {
    type: String,
    required: true,
  },
  dataCyTag: {
    type: String,
    default: ""
  },
  errorText: {
    required: true,
    type: String,
    default: ""
  },
  showSkeleton: {
    type: Boolean,
    default: false
  }
});

const dataCyTagCalc = computed(() => {
  if(props.dataCyTag != ""){
    return props.dataCyTag;
  }
  return props.fieldName.replace(" ", "-").toLowerCase();
});

</script>

<template>
  <div class="mb-3">
    <label :for="dataCyTagCalc">{{ props.fieldName }}</label>
    <Skeleton v-if="showSkeleton" :id="dataCyTagCalc + '-skeleton'" class="w-100" height="10em" />
    <Editor
      v-else
      :id="dataCyTagCalc" v-model="model" :data-cy="dataCyTagCalc" class="w-100"
      :class="{ 'p-invalid': errorText }" v-bind="$attrs"
      :modules="{
        clipboard: { matchVisual: false }
      }"
    >
      <template #toolbar>
        <span class="ql-formats">
          <button class="ql-bold" />
          <button class="ql-italic" />
          <button class="ql-underline" />
        </span>
      </template>
    </Editor>
    <small :data-cy="dataCyTagCalc + '-help'" class="text-danger">{{ errorText }}</small>
    <slot />
  </div>
</template>
