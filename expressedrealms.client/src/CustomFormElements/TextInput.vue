<script setup lang="ts">

import InputText from "primevue/inputtext";

const model = defineModel();

const props = defineProps({
  friendlyFieldName: {
    type: String,
    required: true,
  },
  dataCyTag: {
    type: String,
    required: true
  },
  errorText: {
    required: true
  }
});

</script>

<template>
  <div class="mb-3">
    <label :for="dataCyTag">{{ props.friendlyFieldName }}</label>
    <InputText
        :id="dataCyTag" v-model="model" :data-cy="dataCyTag" class="w-100"
        :class="{ 'p-invalid': errorText }"
    />
    <small :data-cy="dataCyTag + '-help'" class="text-danger">{{ errorText }}</small>
  </div>
</template>

<style scoped>

</style>