<script setup lang="ts">

import Button from 'primevue/button';
import axios from "axios";
import Router from "@/router";
import { useForm } from 'vee-validate';
import { object, string }  from 'yup';
import {logOff} from "@/services/Authentication";
import InputTextWrapper from "../../FormWrappers/InputTextWrapper.vue"

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    name: string().required()
        .max(100)
        .label("Name"),
    phoneNumber: string().required()
        .max(15)
        .label('Phone Number'),
    city: string().required()
        .max(100)
        .label('City'),
    state: string().required()
        .max(2)
        .matches("AL|AK|AZ|AR|CA|CO|CT|DE|FL|GA|HI|ID|IL|IN|IA|KS|KY|LA|ME|MD|MA|MI|MN|MS|MO|MT|NV|NH|NJ|NM|NY|NC|ND|OH|OK|OR|PA|RI|SC|SD|TN|TX|UT|VT|VA|WA|WV|WI|WY|NE", "Not a valid state")
        .label('State'),
  })
});

const [name] = defineField('name');
const [phoneNumber] = defineField('phoneNumber');
const [city] = defineField('city')
const [state] = defineField('state');

const onSubmit = handleSubmit((values) => {
  axios.post('/api/player/addUserProfile', values).then(() => {
      Router.push("characters")
    });
});

</script>

<template>
  <form @submit="onSubmit">
    <div class="mb-3">
      <p>We need a few more pieces of information before we can continue.</p>
    </div>
    <InputTextWrapper v-model="name" field-name="Name" :error-text="errors.name"></InputTextWrapper>
    <InputTextWrapper v-model="phoneNumber" field-name="Phone Number" :error-text="errors.phoneNumber"></InputTextWrapper>
    <InputTextWrapper v-model="city" field-name="City" :error-text="errors.city"></InputTextWrapper>
    <InputTextWrapper v-model="state" field-name="State" :error-text="errors.state"></InputTextWrapper>
    <Button data-cy="create-account-button" label="Update Profile" class="w-100 mb-2" type="submit" />
  </form>
  <Button data-cy="logoff-button" label="Logoff" class="w-100 mb-2" @click="logOff" />
</template>

<style scoped>

</style>