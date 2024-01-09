<script setup lang="ts">

import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import { useForm } from 'vee-validate';
import { object, string }  from 'yup';
import axios from "axios";
import Router from "@/router";

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    email: string().required()
        .email()
        .label('Email address')
  })
});

const [email] = defineField('email');

const onSubmit = handleSubmit((values) => {
  axios.post('/api/auth/forgotPassword', values)
      .then((response) => {
        Router.push('characters');
      });
});

</script>

<template>
  <form @submit="onSubmit">
    <div class="mb-3">
      <label for="email">Email</label>
      <InputText id="email" type="text" v-model="email" class="w-100 " :class="{ 'p-invalid': errors.email }"/>
      <small id="email-help" class="text-danger">{{ errors.email }}</small>
    </div>
    <Button label="Reset Password" class="w-100 mb-2" type="submit"></Button>
  </form>
  <Button label="Back" class="w-100 mb-2" @click="$router.push('/login')"></Button>
</template>

<style scoped>

</style>