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

const route = useRoute();

const onEmailSubmit = handleSubmit((values) => {
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
    <template #title>Change Email</template>
    <template #content>
      <form @submit="onEmailSubmit">
        <div class="flex flex-column gap-2 mb-3">
          <label for="email">Email</label>
          <InputText
              id="email" v-model="email" data-cy="email" type="text" class=""
              :class="{ 'p-invalid': errors.email }"
          />
          <small data-cy="email-help" class="text-danger">{{ errors.email }}</small>
        </div>
        <div class="flex flex-column gap-2 mb-3">
          <label for="email">Confirm Email</label>
          <InputText
              id="confirm-email" v-model="confirmEmail" data-cy="confirm-email" type="text" class=""
              :class="{ 'p-invalid': errors.email }"
          />
          <small data-cy="email-help" class="text-danger">{{ errors.email }}</small>
        </div>
        <Button data-cy="reset-email-button" label="Reset Email" class="" type="submit" />
      </form>
    </template>
  </Card>
  
</template>

<style scoped>
</style>