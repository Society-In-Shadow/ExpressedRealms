<script setup lang="ts">

import InputText from 'primevue/inputtext';
import Button from 'primevue/button';

import { ref } from 'vue'
import LoginBasePlate from "@/components/Login/LoginBasePlate.vue";

const email = ref("");
const password = ref("");
const confirmPassword = ref("");

</script>

<template>
  <LoginBasePlate>
    <div class="row">
      <div class="col">
        <div>
          <label for="email1" class="block text-900 font-medium mb-2">Email</label>
          <InputText id="email1" type="text" v-model="email" class="w-100 mb-3" />
        </div>
        <div>
          <label for="password1" class="block text-900 font-medium mb-2">Password</label>
          <InputText id="password1" type="password" v-model="password" class="w-100 mb-3" />
        </div>
        <div>
          <label for="confirmPassword" class="block text-900 font-medium mb-2">Confirm Password</label>
          <InputText id="confirmPassword" type="password" v-model="confirmPassword" class="w-100 mb-3" />
        </div>
      </div>
    </div>
    <div class="row">
      <div class="col">
        <Button label="Create Account" class="w-100 mb-2"></Button>
        <Button label="Back" class="w-100 mb-2" @click="$router.push('/login')"></Button>
      </div>
    </div>
  </LoginBasePlate>
</template>

<style scoped>

</style>