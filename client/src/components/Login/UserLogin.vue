<script setup lang="ts">

import Button from 'primevue/button';
import axios from "axios";
import { useForm } from 'vee-validate';
import { object, string }  from 'yup';
import {onBeforeMount, ref} from "vue";
import Message from 'primevue/message';
import {useRoute, useRouter} from "vue-router"
import InputTextWrapper from "@/FormWrappers/InputTextWrapper.vue";

const Router = useRouter();

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
  axios.get('/auth/antiforgeryToken');
})

const [email] = defineField('email');
const [password] = defineField('password');
let incorrectLogin = ref(false);
let lockedOut = ref(false);

const route = useRoute();

const onSubmit = handleSubmit((values) => {
  incorrectLogin.value = false;
  lockedOut.value = false;
  axios.post('/auth/login', values)
  .then(() => {
    // Reset antiforgery token after login
    axios.get('/auth/antiforgeryToken')
      .then(() => {
        Router.push('characters');
      });
  }).
  catch((response) => {
    if(response.response.data && response.response.data.detail == "LockedOut")
      lockedOut.value = true;
    else
      incorrectLogin.value = true;
  });
});

</script>

<template>
  <Message v-if="incorrectLogin" severity="error" :closable="false" data-cy="error-invalid-login">
    Your email or password was incorrect, please try again.
  </Message>
  <Message v-if="lockedOut" severity="error" :closable="false" data-cy="error-invalid-login">
    You've been locked out of your account.  Please wait 5 minutes before trying again.
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
    <InputTextWrapper v-model="email" field-name="Email" :error-text="errors.email" />
    <InputTextWrapper v-model="password" field-name="Password" :error-text="errors.password" type="password" />
    <Button data-cy="sign-in-button" label="Sign In" class="w-100 mb-2" type="submit" />
  </form>
  <router-link to="CreateAccount">
    <Button data-cy="create-account" label="Create Account" class="w-100 mb-2" />
  </router-link>
  <router-link to="ForgotPassword">
    <Button data-cy="forgot-password" label="Forgot Password?" class="w-100 mb-2" />
  </router-link>
  <a href="https://discord.gg/NSv3GxSAj7" target="_blank">
    <Button data-cy="join-on-discord" label="Join us on Discord!" class="w-100 mb-2" />
  </a>
</template>
