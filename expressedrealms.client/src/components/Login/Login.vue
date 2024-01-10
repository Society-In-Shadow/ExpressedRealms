<script setup lang="ts">

import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import axios from "axios";
import Router from "@/router";
import { useForm } from 'vee-validate';
import { object, string }  from 'yup';
import {userStore} from "@/stores/userStore";
import {onBeforeMount} from "vue";
let userInfo = userStore();

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    email: string().required()
        .email()
        .label('Email address'),
    password: string().required()
        .label('Password'),
  })
});

onBeforeMount(() => {
  axios.get('/api/auth/getAntiforgeryToken');
})

const [email] = defineField('email');
const [password] = defineField('password');

const onSubmit = handleSubmit((values) => {
  axios.post('/api/auth/login', values)
  .then((response) => {
    axios.get('/api/auth/getInitialLoginInfo')
        .then((stuff) => {
          userInfo.userEmail = stuff.data;
        })
    Router.push('characters');
  });
});

</script>

<template>
  <form @submit="onSubmit">
    <div class="mb-3">
      <label for="email">Email</label>
      <InputText id="email" v-model="email" class="w-100" :class="{ 'p-invalid': errors.email }"/>
      <small id="email-help" class="text-danger">{{ errors.email }}</small>
    </div>
    <div class="mb-3">
      <label for="password">Password</label>
      <InputText id="password" type="password" v-model="password" class="w-100" :class="{ 'p-invalid': errors.password }"/>
      <small id="password-help" class="text-danger">{{ errors.password }}</small>
    </div>
    <Button label="Sign In" class="w-100 mb-2" type="submit"></Button>
  </form>
  <router-link to="CreateAccount">
    <Button label="Create Account" class="w-100 mb-2"></Button>
  </router-link>
  <router-link to="ForgotPassword">
    <Button label="Forgot Password?" class="w-100 mb-2"></Button>
  </router-link>
</template>

<style scoped>

</style>