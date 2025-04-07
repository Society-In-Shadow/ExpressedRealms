<script setup lang="ts">

import Button from 'primevue/button';
import axios from "axios";
import { useRouter } from "vue-router";
const Router = useRouter();
import { useForm } from 'vee-validate';
import { object, string }  from 'yup';
import {logOff} from "@/services/Authentication";
import InputTextWrapper from "../../FormWrappers/InputTextWrapper.vue"
import { userStore } from "@/stores/userStore";
const userInfo = userStore();

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    name: string().required()
        .max(100)
        .label("Name")
  })
});

const [name] = defineField('name');
const onSubmit = handleSubmit((values) => {
  axios.post('/player', values).then(() => {
      userInfo.name = values.name;
      Router.push("characters");
    });
});

</script>

<template>
  <form @submit="onSubmit">
    <div class="mb-3">
      <h1 class="mt-md-0 pt-md-0">
        User Profile Setup
      </h1>
    </div>
    <InputTextWrapper v-model="name" field-name="Name" :error-text="errors.name" />
    <Button data-cy="update-profile-button" label="Update Profile" class="w-100 mb-2" type="submit" />
  </form>
  <Button data-cy="logoff-button" label="Logoff" class="w-100 mb-2" @click="logOff" />
</template>
