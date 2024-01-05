<script setup lang="ts">

import InputText from 'primevue/inputtext';
import Button from 'primevue/button';

import { ref } from 'vue'
import LoginBasePlate from "@/components/Login/LoginBasePlate.vue";

const email = ref("");
const password = ref("");

</script>

<template>
  <login-base-plate>
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
      </div>
    </div>
    <div class="row">
      <div class="col">
        <Button label="Sign In" class="w-100 mb-2"></Button>
        <router-link to="CreateAccount">
          <Button label="Create Account" class="w-100 mb-2"></Button>
        </router-link>
        <router-link to="ForgotPassword">
          <Button label="Forgot Password?" class="w-100"></Button>
        </router-link>
      </div>
    </div>
  </login-base-plate>
</template>

<style scoped>

</style>