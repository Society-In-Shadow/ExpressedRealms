<script setup lang="ts">

import InputText from "primevue/inputtext";
import {computed, watch, ref} from "vue";
import Skeleton from 'primevue/skeleton';
import type {FormField} from "@/FormWrappers/Interfaces/FormField";

const model = defineModel<FormField>({ required: true });

defineOptions({
  inheritAttrs: false
})

const props = defineProps({
  dataCyTag: {
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
  return model.value.label.replace(" ", "-").toLowerCase();
});

</script>

<template>
  <div class="mb-3">
    <label :for="dataCyTagCalc">{{ model.label }}<span v-if="model.isRequired" class="text-danger font-italic"> (Required)</span></label>
    <Skeleton v-if="showSkeleton" :id="dataCyTagCalc + '-skeleton'" class="w-100" height="3em" />
    <InputText
      v-else
      :id="dataCyTagCalc" v-model="model.field.value" :data-cy="dataCyTagCalc" class="w-100"
      :class="{ 'p-invalid': model.error && model.error.length > 0 }" v-bind="$attrs"
    />
    <small :data-cy="dataCyTagCalc + '-help'" class="text-danger">{{ model.error }}</small>
    <slot />
  </div>
</template>
