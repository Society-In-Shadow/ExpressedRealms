<script setup lang="ts">

import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import Card from 'primevue/card';
import axios from "axios";
import { useForm } from 'vee-validate';
import { object, string, ref }  from 'yup';
import {ref as vueRef} from "vue";
import Message from "primevue/message";
import {resetEmailConfirmation} from "@/services/Authentication";

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    email: string()
        .required()
        .email()
        .label('Email'),
    confirmEmail: string().required()
        .oneOf([ref('email')], 'Emails must match')
        .label('Confirm Email')
  })
});

const [email] = defineField('email');
const [confirmEmail] = defineField('confirmEmail');
const successfullyChangedEmail = vueRef(false);

const onEmailSubmit = handleSubmit((values, { resetForm }) => {
  axios.post('/api/auth/manage/info',
      {
        newEmail: values.confirmEmail
      }).then(() => {
        successfullyChangedEmail.value = true;
        resetEmailConfirmation();
        resetForm();
      });
});

</script>

<template>
  <Card class="mb-3">
    <template #title>
      Change Email
    </template>
    <template #content>
      <Message v-if="successfullyChangedEmail" severity="success" :closable="false" data-cy="successful-change-email">
        Email confirmation was sent to the provided email.  Once you click the link, it will update your email address. 
      </Message>
      <form @submit="onEmailSubmit">
        <div class="flex flex-column gap-2 mb-3">
          <label for="email">Email</label>
          <InputText
            id="email" v-model="email" data-cy="email" type="text"
            :class="{ 'p-invalid': errors.email }"
          />
          <small data-cy="email-help" class="text-danger">{{ errors.email }}</small>
        </div>
        <div class="flex flex-column gap-2 mb-3">
          <label for="confirm-email">Confirm Email</label>
          <InputText
            id="confirm-email" v-model="confirmEmail" data-cy="confirm-email" type="text"
            :class="{ 'p-invalid': errors.confirmEmail }"
          />
          <small data-cy="confirm-email-help" class="text-danger">{{ errors.confirmEmail }}</small>
        </div>
        <Button data-cy="reset-email-button" label="Reset Email" class="" type="submit" />
      </form>
    </template>
  </Card>
</template>

<style scoped>
</style>