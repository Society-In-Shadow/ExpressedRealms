<script setup lang="ts">

import InputText from "primevue/inputtext";
import {computed} from "vue";
import Skeleton from 'primevue/skeleton';

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
    <Skeleton v-if="showSkeleton" :id="dataCyTagCalc + '-skeleton'" class="w-100" height="3em" />
    <InputText
      v-else
      :id="dataCyTagCalc" v-model="model" :data-cy="dataCyTagCalc" class="w-100"
      :class="{ 'p-invalid': errorText }" v-bind="$attrs"
    />
    <small :data-cy="dataCyTagCalc + '-help'" class="text-danger">{{ errorText }}</small>
    <slot />
  </div>
</template>

<style scoped>

</style>