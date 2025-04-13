<script setup lang="ts">

import Textarea from 'primevue/textarea';
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
  <div class="mb-3">
    <label :for="dataCyTagCalc">{{ model.label }}</label>
    <Skeleton v-if="showSkeleton" :id="dataCyTagCalc + '-skeleton'" class="w-100" height="5em" />
    <Textarea
      v-else
      :id="dataCyTagCalc" v-model="model.field.value" :data-cy="dataCyTagCalc" class="w-100"
      :class="{ 'p-invalid': model.error && model.error.length > 0 }" v-bind="$attrs" auto-resize
    />
    <small :data-cy="dataCyTagCalc + '-help'" class="text-danger">{{ model.error }}</small>
    <slot />
  </div>
</template>
