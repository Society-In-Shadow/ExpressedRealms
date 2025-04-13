<script setup lang="ts">

import axios from "axios";
import toaster from "@/services/Toasters";
import Button from "primevue/button";
import {useRouter} from "vue-router";
import {nameField, navMenuImageField, shortDescriptionField, handleSubmit} from "@/components/expressions/expression/AddExpressionValidation";
import FormInputTextWrapper from "@/FormWrappers/FormInputTextWrapper.vue";
import FormTextAreaWrapper from "@/FormWrappers/FormTextAreaWrapper.vue";
const Router = useRouter();

const emit = defineEmits<{
  refreshList: []
  closeDialog: []
}>();

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
    <FormInputTextWrapper v-model="nameField"/>
    <FormTextAreaWrapper v-model="shortDescriptionField"/>
    <FormInputTextWrapper v-model="navMenuImageField" />
    <p>List of icons can be found here : <a href="https://primevue.org/icons/#list">Primevue Icons</a></p>
    <Button data-cy="add-expression-button" label="Add Expression" class="w-100 mb-2" type="submit" />
  </form>
</template>

