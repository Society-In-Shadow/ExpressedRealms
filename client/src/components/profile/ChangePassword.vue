<script setup lang="ts">

import Button from 'primevue/button';
import Card from 'primevue/card';
import axios from "axios";
import { useForm } from 'vee-validate';
import { object, string, ref }  from 'yup';
import Message from "primevue/message";
import { ref as vueRef } from 'vue';
import InputTextWrapper from "@/FormWrappers/InputTextWrapper.vue";

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
  axios.post('/auth/manage/info',
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
        <InputTextWrapper v-model="currentPassword" field-name="Current Password" :error-text="errors.currentPassword" type="password" />
        <InputTextWrapper v-model="password" field-name="New Password" :error-text="errors.password" type="password" />
        <InputTextWrapper v-model="confirmPassword" field-name="Confirm Password" :error-text="errors.confirmPassword" type="password" />
        <Button data-cy="reset-password-button" label="Reset Password" class="w-100" type="submit" />
      </form>
    </template>
  </Card>
</template>
