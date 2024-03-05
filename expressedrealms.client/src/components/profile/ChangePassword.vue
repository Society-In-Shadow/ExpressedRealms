<script setup lang="ts">

import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import Card from 'primevue/card';
import axios from "axios";
import { useForm } from 'vee-validate';
import { object, string, ref }  from 'yup';
import Message from "primevue/message";
import { ref as vueRef } from 'vue';

const { defineField, handleSubmit, errors, setErrors } = useForm({
  validationSchema: object({
    currentPassword: string().required()
        .label('Current Password'),
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

const [currentPassword] = defineField('currentPassword');
const [password] = defineField('password');
const [confirmPassword] = defineField('confirmPassword');
const successfullyChangedPassword = vueRef(false);

const onPasswordSubmit = handleSubmit((values, { resetForm }) => {
  axios.post('/api/auth/manage/info',
      {
        oldPassword: values.currentPassword,
        newPassword: values.confirmPassword
      }).then(() => {
        successfullyChangedPassword.value = true;
        resetForm();
      })
      .catch((error) => {
        successfullyChangedPassword.value = false;
        if (error.response?.status === 400){
          if(error.response.data?.errors?.PasswordMismatch) {
            setErrors({ currentPassword: error.response.data.errors.PasswordMismatch });
          }
          return;
        }
        throw error;
      });
});

</script>

<template>
  <Card class="mb-3">
    <template #title>
      Reset Password
    </template>
    <template #content>
      <Message v-if="successfullyChangedPassword" severity="success" data-cy="successful-change-password">
        Successfully changed your password!
      </Message>
      <form @submit="onPasswordSubmit">
        <div class="flex flex-column gap-2 mb-3">
          <label for="currentPassword">Current Password</label>
          <InputText
            id="currentPassword" v-model="currentPassword" data-cy="current-password" type="password"
            :class="{ 'p-invalid': errors.currentPassword }"
          />
          <small data-cy="current-password-help" class="text-danger">{{ errors.currentPassword }}</small>
        </div>
        <div class="flex flex-column gap-2 mb-3">
          <label for="password">New Password</label>
          <InputText
            id="password" v-model="password" data-cy="password" type="password"
            :class="{ 'p-invalid': errors.password }"
          />
          <small data-cy="password-help" class="text-danger">{{ errors.password }}</small>
        </div>
        <div class="flex flex-column gap-2 mb-3">
          <label for="confirmPassword">Confirm Password</label>
          <InputText
            id="confirmPassword" v-model="confirmPassword" data-cy="confirm-password" type="password"
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