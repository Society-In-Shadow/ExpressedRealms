<script setup lang="ts">

import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import axios from "axios";
import Router from "@/router";
import { useForm } from 'vee-validate';
import { object, string, ref }  from 'yup';

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    email: string().required()
        .email()
        .label('Email address'),
    password: string()
        .required()
        .min(8)
        .matches(/[0-9]/, 'Password requires a number')
        .matches(/[a-z]/, 'Password requires a lowercase letter')
        .matches(/[A-Z]/, 'Password requires an uppercase letter')
        .matches(/[^\w]/, 'Password requires a symbol')
        .label('Password'),
    confirmPassword: string().required()
        .oneOf([ref('password')], 'Passwords must match')
        .label('Confirm password')
  })
});

const [email] = defineField('email');
const [password] = defineField('password');
const [confirmPassword] = defineField('confirmPassword')

const onSubmit = handleSubmit((values) => {
  console.log(values);
  axios.post('/api/auth/createaccount', 
      {
        email: values.email,
        password: values.confirmPassword
  }).then((response) => {
    Router.push('characters');
  });
});

</script>

<template>
  <form @submit="onSubmit">
    <div class="mb-3">
      <label for="email">Email</label>
      <InputText data-cy="email" id="email" type="text" v-model="email" class="w-100" :class="{ 'p-invalid': errors.email }"/>
      <small data-cy="email-help" class="text-danger">{{ errors.email }}</small>
    </div>
    <div class="mb-3">
      <label for="password">Password</label>
      <InputText data-cy="password" id="password" type="password" v-model="password" class="w-100" :class="{ 'p-invalid': errors.password }"/>
      <small data-cy="password-help" class="text-danger">{{ errors.password }}</small>
    </div>
    <div class="mb-3">
      <label for="confirmPassword">Confirm Password</label>
      <InputText data-cy="confirm-password" id="confirmPassword" type="password" v-model="confirmPassword" class="w-100" :class="{ 'p-invalid': errors.confirmPassword }"/>
      <small data-cy="confirm-password-help" class="text-danger">{{ errors.confirmPassword }}</small>
    </div>
    <Button data-cy="create-account-button" label="Create Account" class="w-100 mb-2" type="submit"></Button>
  </form>
  <Button data-cy="back-button" label="Back" class="w-100 mb-2" @click="$router.push('/login')"></Button>
</template>

<style scoped>

</style>