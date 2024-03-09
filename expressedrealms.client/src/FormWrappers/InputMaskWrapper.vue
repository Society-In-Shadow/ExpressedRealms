<script setup lang="ts">

import InputMask from 'primevue/inputmask';
import {computed} from "vue";

const model = defineModel<string>({ required: true });

defineOptions({
  inheritAttrs: false
})


const props = defineProps({
  fieldName: {
    type: String,
    required: true,
  },
  mask: {
    type: String,
    required: true
  },
  placeholder: {
    type: String,
    default: ""
  },
  dataCyTag: {
    type: String,
    default: ""
  },
  errorText: {
    required: true,
    type: String
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
    <InputMask
      :id="dataCyTagCalc" v-model="model" :data-cy="dataCyTagCalc" class="w-100"
      :class="{ 'p-invalid': errorText }" :mask="mask" :placeholder="placeholder"
      v-bind="$attrs"
    />
    <small :data-cy="dataCyTagCalc + '-help'" class="text-danger">{{ errorText }}</small>
  </div>
</template>

<style scoped>

</style>