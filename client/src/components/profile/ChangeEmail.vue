<script setup lang="ts">

import Button from 'primevue/button';
import Card from 'primevue/card';
import axios from "axios";
import { useForm } from 'vee-validate';
import { object, string, ref }  from 'yup';
import {ref as vueRef} from "vue";
import Message from "primevue/message";
import {resetEmailConfirmation} from "@/services/Authentication";
import InputTextWrapper from "@/FormWrappers/InputTextWrapper.vue";
import toasters from "@/services/Toasters";

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
  axios.post('/auth/manage/info',
      {
        newEmail: values.confirmEmail
      }).then(() => {
        successfullyChangedEmail.value = true;
        resetEmailConfirmation();
        resetForm();
        toasters.success("Successfully updated email!");
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
        <InputTextWrapper v-model="email" field-name="Email" :error-text="errors.email" />
        <InputTextWrapper v-model="confirmEmail" field-name="Confirm Email" :error-text="errors.confirmEmail" />
        <Button data-cy="reset-email-button" label="Reset Email" class="w-100" type="submit" />
      </form>
    </template>
  </Card>
</template>
