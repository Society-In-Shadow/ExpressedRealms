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
const userInfo = userStore();

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    name: string().required()
        .max(100)
        .label("Name"),
    phoneNumber: string().required()
        .max(15)
        .matches("\\(\\d{3}\\) \\d{3}\\-\\d{4}", "Format must be (555) 555-5555")
        .label('Phone Number'),
    city: string().required()
        .max(100)
        .label('City'),
    state: string().required()
        .min(2)
        .matches("AL|AK|AZ|AR|CA|CO|CT|DE|FL|GA|HI|ID|IL|IN|IA|KS|KY|LA|ME|MD|MA|MI|MN|MS|MO|MT|NV|NH|NJ|NM|NY|NC|ND|OH|OK|OR|PA|RI|SC|SD|TN|TX|UT|VT|VA|WA|WV|WI|WY|NE", "Not a valid state")
        .label('State'),
  })
});

const [name] = defineField('name');
const [phoneNumber] = defineField('phoneNumber');
const [city] = defineField('city')
const [state] = defineField('state');
const isLoading = ref(true);

onMounted(() =>{
  axios.get("/api/player")
      .then((response) => {
        name.value = response.data.name;
        phoneNumber.value = response.data.phoneNumber;
        city.value = response.data.city;
        state.value = response.data.state;
        isLoading.value = false;
      })
});

const onSubmit = handleSubmit((values) => {
  axios.put('/api/player', values).then(() => {
      userInfo.name = values.name;
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
        <InputMaskWrapper v-model="phoneNumber" field-name="Phone Number" :error-text="errors.phoneNumber" mask="(999) 999-9999" :show-skeleton="isLoading" />
        <InputTextWrapper v-model="city" field-name="City" :error-text="errors.city" :show-skeleton="isLoading" />
        <InputTextWrapper v-model="state" field-name="State" :error-text="errors.state" maxlength="2" :show-skeleton="isLoading" />
        <Button data-cy="update-profile-button" label="Update Profile" class="w-100 mb-2" type="submit" :disabled="isLoading" />
      </form>
    </template>
  </Card>
</template>
