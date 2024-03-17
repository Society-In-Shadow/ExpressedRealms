<script setup lang="ts">

import Button from 'primevue/button';
import axios from "axios";
import Router from "@/router";
import { useForm } from 'vee-validate';
import { object, string, ref }  from 'yup';
import { useRoute } from 'vue-router'
import InputTextWrapper from "@/FormWrappers/InputTextWrapper.vue";

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

const route = useRoute();

const onSubmit = handleSubmit((values) => {
  console.log(values);
  axios.post('/api/auth/resetPassword',
      {
        email: values.email,
        resetCode: route.query.resetToken,
        newPassword: values.confirmPassword
      }).then(() => {
        Router.push('login?resetPassword=1');
      });
});

</script>

<template>
  <form @submit="onSubmit">
    <InputTextWrapper field-name="Email" v-model="email"  :error-text="errors.email" />
    <InputTextWrapper field-name="Password" v-model="password" :error-text="errors.password" type="password"/>
    <InputTextWrapper field-name="Confirm Password" v-model="confirmPassword" :error-text="errors.confirmPassword" type="password"/>
    <Button data-cy="reset-password-button" label="Reset Password" class="w-100 mb-2" type="submit" />
  </form>
  <Button data-cy="back-button" label="Back" class="w-100 mb-2" @click="Router.push('/login')" />
</template>

<style scoped>

</style>