<script setup lang="ts">

import axios from "axios";
import { useForm } from 'vee-validate';
import { object, string }  from 'yup';
import Card from "primevue/card";
import InputTextWrapper from "@/FormWrappers/InputTextWrapper.vue";
import TextAreaWrapper from "@/FormWrappers/TextAreaWrapper.vue";
import {onMounted, ref} from "vue";
import { useRoute } from 'vue-router'
import toaster from "@/services/Toasters";

const route = useRoute()

onMounted(() =>{
  axios.get(`/api/characters/${route.params.id}`)
      .then((response) => {
        name.value = response.data.name;
        background.value = response.data.background;
        expression.value = response.data.expression;
      })
});

const { defineField, handleSubmit, errors } = useForm({
  validationSchema: object({
    name: string().required()
        .max(150)
        .label("Name"),
    background: string()
        .label('Background'),
  })
});

const [name] = defineField('name');
const [background] = defineField('background');
const expression = ref("");

const onSubmit = handleSubmit((values) => {
  axios.put('/api/characters/', {
    name: values.name,
    background: values.background,
    id: route.params.id
  }).then(() => {
    toaster.success("Successfully Updated Character Info!");
  });
});

</script>

<template>
  <div class="flex flex-xs-column flex-sm-column flex-lg-row flex-md-row gap-3 m-3 ">
    <Card class="mb-3">
      <template #content>
        <form @submit="onSubmit">
          <InputTextWrapper v-model="name" field-name="Name" :error-text="errors.name" @change="onSubmit" />
          <InputTextWrapper v-model="expression" field-name="Expression" disabled @change="onSubmit" />          
          <TextAreaWrapper v-model="background" field-name="Background" :error-text="errors.background" @change="onSubmit" />
        </form>
      </template>
    </Card>
  </div>
</template>

<style scoped>

</style>