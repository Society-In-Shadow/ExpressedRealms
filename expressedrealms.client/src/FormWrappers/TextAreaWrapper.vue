<script setup lang="ts">


import Textarea from 'primevue/textarea';
import {computed} from "vue";

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
    <Textarea
      :id="dataCyTagCalc" v-model="model" :data-cy="dataCyTagCalc" class="w-100"
      :class="{ 'p-invalid': errorText }" v-bind="$attrs" autoResize
    />
    <small :data-cy="dataCyTagCalc + '-help'" class="text-danger">{{ errorText }}</small>
  </div>
</template>

<style scoped>

</style>