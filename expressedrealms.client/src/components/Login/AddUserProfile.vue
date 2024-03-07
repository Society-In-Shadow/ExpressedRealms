<script setup lang="ts">

import Button from 'primevue/button';
import axios from "axios";
import Router from "@/router";
import { useForm } from 'vee-validate';
import { object, string, ref }  from 'yup';
import {logOff} from "@/services/Authentication";
import TextInput from "../../CustomFormElements/TextInput.vue"

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
  /*axios.post('/api/player/addPlayer', 
    {
      email: values.email,
      password: values.confirmPassword
    }).then(() => {
      Router.push("login?createdUser=1")
    });*/
});

</script>

<template>
  <form @submit="onSubmit">
    <div class="mb-3">
      <p>We need a few more pieces of information before we can continue.</p>
    </div>
    <TextInput data-cy-tag="name" friendly-field-name="Name" :error-text="errors.name" v-model="name"></TextInput>
    <TextInput data-cy-tag="phone-number" friendly-field-name="Phone Number" :error-text="errors.phoneNumber" v-model="phoneNumber"></TextInput>
    <TextInput data-cy-tag="city" friendly-field-name="City" :error-text="errors.city" v-model="city"></TextInput>
    <TextInput data-cy-tag="state" friendly-field-name="State" :error-text="errors.state" v-model="state"></TextInput>
    <Button data-cy="create-account-button" label="Update Profile" class="w-100 mb-2" type="submit" />
  </form>
  <Button data-cy="logoff-button" label="Logoff" class="w-100 mb-2" @click="logOff" />
</template>

<style scoped>

</style>