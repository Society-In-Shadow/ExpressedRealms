<script setup lang="ts">

import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import axios from "axios";
import Router from "@/router";
import { useForm } from 'vee-validate';
import { object, string }  from 'yup';

import {onBeforeMount, ref} from "vue";
import Message from 'primevue/message';
import {useRoute} from "vue-router"

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
let incorrectLogin = ref(false);

const route = useRoute();

const onSubmit = handleSubmit((values) => {
  incorrectLogin.value = false;
  axios.post('/api/auth/login', values)
  .then(() => {
    // Reset antiforgery token after login
    axios.get('/api/auth/getAntiforgeryToken')
      .then(() => {
        Router.push('characters');
      });
  }).
  catch(() => {
    incorrectLogin.value = true;
  });
});

</script>

<template>
  <Message v-if="incorrectLogin" severity="error" :closable="false" data-cy="error-invalid-login">
    Your email or password was incorrect, please try again.
  </Message>
  <Message v-else-if="route.query.resetPassword" severity="success" :closable="false" data-cy="success-password-reset-message">
    Password was successfully changed, please log in.
  </Message>
  <Message v-else-if="route.query.createdUser" severity="success" :closable="false" data-cy="success-created-user-message">
    User was successfully created, please log in.
  </Message>
  <Message v-else-if="route.query.confirmedEmail" severity="success" :closable="false" data-cy="success-confirmed-email-message">
    Your email was successfully confirmed, please log in.
  </Message>
  <Message v-else-if="route.query.forgotPassword" severity="success" :closable="false" data-cy="success-forgot-password-message">
    An email was sent to your email, please continue with the email, or login below.
  </Message>
  <form @submit="onSubmit">
    <div class="mb-3">
      <label for="email">Email</label>
      <InputText id="email" v-model="email" data-cy="email" class="w-100" :class="{ 'p-invalid': errors.email }" />
      <small id="email-help" data-cy="email-help" class="text-danger">{{ errors.email }}</small>
    </div>
    <div class="mb-3">
      <label for="password">Password</label>
      <InputText
        id="password" v-model="password" data-cy="password" type="password" class="w-100"
        :class="{ 'p-invalid': errors.password }"
      />
      <small id="password-help" data-cy="password-help" class="text-danger">{{ errors.password }}</small>
    </div>
    <Button data-cy="sign-in-button" label="Sign In" class="w-100 mb-2" type="submit" />
  </form>
  <router-link to="CreateAccount">
    <Button data-cy="create-account" label="Create Account" class="w-100 mb-2" />
  </router-link>
  <router-link to="ForgotPassword">
    <Button data-cy="forgot-password" label="Forgot Password?" class="w-100 mb-2" />
  </router-link>
</template>

<style scoped>

</style>