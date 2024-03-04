<script setup lang="ts">

import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import Card from 'primevue/card';
import axios from "axios";
import Router from "@/router";
import { useForm } from 'vee-validate';
import { object, string, ref }  from 'yup';
import { useRoute } from 'vue-router'

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
const [confirmEmail] = defineField('confirmEmail');
const [password] = defineField('password');
const [confirmPassword] = defineField('confirmPassword')

const route = useRoute();

const onPasswordSubmit = handleSubmit((values) => {
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
  <Card class="mb-3">
    <template #title>Reset Password</template>
    <template #content>
      <form @submit="onPasswordSubmit">
        <div class="flex flex-column gap-2 mb-3">
          <label for="password">Password</label>
          <InputText
              id="password" v-model="password" data-cy="password" type="password" class=""
              :class="{ 'p-invalid': errors.password }"
          />
          <small data-cy="password-help" class="text-danger">{{ errors.password }}</small>
        </div>
        <div class="flex flex-column gap-2 mb-3">
          <label for="confirmPassword">Confirm Password</label>
          <InputText
              id="confirmPassword" v-model="confirmPassword" data-cy="confirm-password" type="password" class=""
              :class="{ 'p-invalid': errors.confirmPassword }"
          />
          <small data-cy="confirm-password-help" class="text-danger">{{ errors.confirmPassword }}</small>
        </div>
        <Button data-cy="reset-password-button" label="Reset Password" class="flex flex-column gap-3" type="submit" />
      </form>
    </template>
  </Card>
</template>

<style scoped>

</style>