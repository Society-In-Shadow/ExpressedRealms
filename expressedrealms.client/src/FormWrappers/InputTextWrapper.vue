<script setup lang="ts">

import InputText from "primevue/inputtext";
import {computed} from "vue";

const model = defineModel({ required: true });

const props = defineProps({
  fieldName: {
    type: String,
    required: true,
  },
  dataCyTag: {
    type: String
  },
  errorText: {
    required: true
  }
});

const dataCyTagCalc = computed(() => props.dataCyTag ?? props.fieldName.replace(" ", "-").toLowerCase())

</script>

<template>
  <div class="mb-3">
    <label :for="dataCyTagCalc">{{ props.fieldName }}</label>
    <InputText
        :id="dataCyTagCalc" v-model="model" :data-cy="dataCyTagCalc" class="w-100"
        :class="{ 'p-invalid': errorText }"
    />
    <small :data-cy="dataCyTagCalc + '-help'" class="text-danger">{{ errorText }}</small>
  </div>
</template>

<style scoped>

</style>