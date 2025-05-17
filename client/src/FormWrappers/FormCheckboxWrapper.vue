<script setup lang="ts">

import Checkbox from 'primevue/checkbox';
import {computed} from "vue";
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
  <div class="mb-3 d-flex align-items-center">
    
    <Skeleton v-if="showSkeleton" :id="dataCyTagCalc + '-skeleton'" class="w-100" height="3em" />
    <Checkbox
        v-if="!showSkeleton"
        :input-id="dataCyTagCalc" v-model="model.field.value" :data-cy="dataCyTagCalc"
        v-bind="$attrs" :invalid="model.error && model.error.length > 0" binary
    />
    <label v-if="!showSkeleton" :for="dataCyTagCalc" class="ml-2">{{ model.label }}</label>
    <small :data-cy="dataCyTagCalc + '-help'" class="text-danger" v-if="model.error && model.error.length > 0">{{ model.error }}</small>
    <slot />
  </div>
</template>
