<script setup lang="ts">

import Button from 'primevue/button';
import { useForm } from 'vee-validate';
import { object, string }  from 'yup';
import axios from "axios";
import Router from "@/router";
import InputTextWrapper from "@/FormWrappers/InputTextWrapper.vue";

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    email: string().required()
        .email()
        .label('Email address')
  })
});

const [email] = defineField('email');

const onSubmit = handleSubmit((values) => {
  axios.post('/auth/forgotPassword', values)
      .then(() => {
        Router.push('login?forgotPassword=1');
      });
});

</script>

<template>
  <form @submit="onSubmit">
    <InputTextWrapper v-model="email" field-name="Email" :error-text="errors.email" />
    <Button label="Reset Password" class="w-100 mb-2" type="submit" />
  </form>
  <Button label="Back" class="w-100 mb-2" @click="Router.push('/login')" />
</template>
