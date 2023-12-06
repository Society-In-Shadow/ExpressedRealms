<script setup lang="ts">

import InputText from 'primevue/inputtext';
import Button from 'primevue/button';

import { ref } from 'vue'

const email = ref("");
const emit = defineEmits(['transitionToScreen']);

</script>

<template>
  <div class="row">
    <div class="col">
      <div>
        <label for="email1" class="block text-900 font-medium mb-2">Email</label>
        <InputText id="email1" type="text" v-model="email" class="w-100 mb-3" />
      </div>
    </div>
  </div>
  <div class="row">
    <div class="col">
      <Button label="Reset Password" class="w-100 mb-2"></Button>
      <Button label="Back" class="w-100 mb-2" @click="emit('transitionToScreen', 'Login')"></Button>
    </div>
  </div>
</template>

<style scoped>

</style>