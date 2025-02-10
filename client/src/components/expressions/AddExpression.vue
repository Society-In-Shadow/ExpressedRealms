<script setup lang="ts">

import {useForm} from "vee-validate";
import {object, string } from "yup";
import InputTextWrapper from "@/FormWrappers/InputTextWrapper.vue";
import TextAreaWrapper from "@/FormWrappers/TextAreaWrapper.vue";
import axios from "axios";
import toaster from "@/services/Toasters";
import Button from "primevue/button";
import Router from "@/router";

const emit = defineEmits<{
  refreshList: []
  closeDialog: []
}>();

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    name: string().required()
        .max(50)
        .label('Name'),
    shortDescription: string()
        .required()
        .max(125)
        .label('Short Description'),
    navMenuImage: string().required()
        .label('Nav Menu Icon')
  })
});

const [name] = defineField('name');
const [shortDescription] = defineField('shortDescription');
const [navMenuImage] = defineField('navMenuImage');

navMenuImage.value = 'pi-prime'

const onSubmit = handleSubmit((values) => {
  axios.post(`/expression/`, {
    name: values.name,
    shortDescription: values.shortDescription,
    navMenuImage: values.navMenuImage
  }).then(() => {
    emit('refreshList');
    emit('closeDialog');
    toaster.success(`Successfully added ${values.Name} Expression as a Draft!`);
    Router.push("/expressions/" + encodeURIComponent(values.name.toLowerCase()));
  });
});

</script>

<template>
  <form @submit="onSubmit">
    <InputTextWrapper v-model="name" field-name="Name" :error-text="errors.name" />
    <TextAreaWrapper v-model="shortDescription" field-name="Short Description" :error-text="errors.shortDescription" />
    <InputTextWrapper v-model="navMenuImage" field-name="Nav Menu Icon" :error-text="errors.navMenuImage" />
    <p>List of icons can be found here : <a href="https://primevue.org/icons/#list">Primevue Icons</a></p>
    <Button data-cy="add-expression-button" label="Add Expression" class="w-100 mb-2" type="submit" />
  </form>
</template>

