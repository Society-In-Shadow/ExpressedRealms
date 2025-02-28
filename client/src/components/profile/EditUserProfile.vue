<script setup lang="ts">

import Button from 'primevue/button';
import axios from "axios";
import { useForm } from 'vee-validate';
import { object, string }  from 'yup';
import InputTextWrapper from "../../FormWrappers/InputTextWrapper.vue"
import { userStore } from "@/stores/userStore";
import InputMaskWrapper from "@/FormWrappers/InputMaskWrapper.vue";
import Card from "primevue/card";
import {onMounted, ref} from "vue";
import toasters from "@/services/Toasters";
const userInfo = userStore();

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    name: string().required()
        .max(100)
        .label("Name"),
  })
});

const [name] = defineField('name');
const [phoneNumber] = defineField('phoneNumber');
const [city] = defineField('city')
const [state] = defineField('state');
const isLoading = ref(true);

onMounted(() =>{
  axios.get("/player")
      .then((response) => {
        name.value = response.data.name;
        isLoading.value = false;
      })
});

const onSubmit = handleSubmit((values) => {
  axios.put('/player', values).then(() => {
      userInfo.name = values.name;
    })
      .then(() => {
        toasters.success("Successfully Updated User Name!");
      });
});

</script>

<template>
  <Card class="mb-3" style="width: 390px">
    <template #title>
      User Profile
    </template>
    <template #content>
      <form @submit="onSubmit">
        <InputTextWrapper v-model="name" field-name="Name" :error-text="errors.name" :show-skeleton="isLoading" />
        <Button data-cy="update-profile-button" label="Update Profile" class="w-100 mb-2" type="submit" :disabled="isLoading" />
      </form>
    </template>
  </Card>
</template>
