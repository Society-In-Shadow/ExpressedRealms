<script setup lang="ts">

import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import axios from "axios";
import Router from "@/router";
import LoginBasePlate from "@/components/Login/LoginBasePlate.vue";
import { useForm } from 'vee-validate';
import * as yup from 'yup';

const schema = yup.object({
  email: yup.string().required().email().label('Email address'),
  password: yup.string().required().label('Password'),
});

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: schema,
});

const [email] = defineField('email');
const [password] = defineField('password');

const onSubmit = handleSubmit((values) => {
  axios.post('/api/auth/login', values)
  .then((response) => {
    Router.push('characters');
  });
});

</script>

<template>
  <login-base-plate>
    <div class="row">
      <div class="col">
        <form @submit="onSubmit">
          <div class="mb-3">
            <label for="email" class="block text-900 font-medium">Email</label>
            <InputText id="email" v-model="email" class="w-100" :class="{ 'p-invalid': errors.email }"/>
            <small id="email-help" class="text-danger">{{ errors.email }}</small>
          </div>
          <div class="mb-3">
            <label for="password" class="block text-900 font-medium">Password</label>
            <InputText id="password" type="password" v-model="password" class="w-100" :class="{ 'p-invalid': errors.password }"/>
            <small id="password-help" class="text-danger">{{ errors.password }}</small>
          </div>
          <Button label="Sign In" class="w-100 mb-2" type="submit"></Button>
        </form>
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