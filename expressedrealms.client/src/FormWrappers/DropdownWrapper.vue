<script setup lang="ts">

import Dropdown from 'primevue/dropdown';
import {computed} from "vue";

interface DropDown {
  id: number,
  name: string,
  shortDescription: string
}

const model = defineModel({ required: true, default: {} });

defineOptions({
  inheritAttrs: false
})

const props = defineProps({
  fieldName: {
    type: String,
    required: true,
  },
  options: {
    type: Array,
    required: true
  },
  optionLabel: {
    type: String,
    required: true
  },
  dataCyTag: {
    type: String,
    default: ""
  },
  errorText: {
    type: String,
    default: ""
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
    <Dropdown
      :id="dataCyTagCalc" v-model="model" :options="options" :option-label="optionLabel" :data-cy="dataCyTagCalc"
      class="w-100" :class="{ 'p-invalid': errorText }" v-bind="$attrs"
    />
    <small :data-cy="dataCyTagCalc + '-help'" class="text-danger">{{ errorText }}</small>
    <slot />
  </div>
</template>

<style scoped>

</style>