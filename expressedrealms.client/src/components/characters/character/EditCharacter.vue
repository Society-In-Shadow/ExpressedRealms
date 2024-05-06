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
import SmallStatDisplay from "@/components/characters/character/SmallStatDisplay.vue";
const route = useRoute()
import Breadcrumb from 'primevue/breadcrumb';
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";

onMounted(() =>{
  axios.get(`/api/characters/${route.params.id}`)
      .then((response) => {
        name.value = response.data.name;
        background.value = response.data.background;
        expression.value = response.data.expression;
        isLoading.value = false;
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
const isLoading = ref(true);

const onSubmit = handleSubmit((values) => {
  axios.put('/api/characters/', {
    name: values.name,
    background: values.background,
    id: route.params.id
  }).then(() => {
    toaster.success("Successfully Updated Character Info!");
  });
});

const items = ref([
  { label: name },
]);
const home = ref({
  icon: 'pi pi-home',
  route: '/characters'
});
</script>

<template>
  <Breadcrumb :home="home" :model="items" class="m-3">
    <template #item="{ item, props }">
      <router-link v-if="item.route" v-slot="{ href, navigate }" :to="item.route" custom>
        <SkeletonWrapper :show-skeleton="isLoading" width="1em" height="1em">
          <a :href="href" v-bind="props.action" @click="navigate">
            <span :class="[item.icon, 'text-color']" />
            <span class="text-primary font-semibold">{{ item.label }}</span>
          </a>
        </SkeletonWrapper>
      </router-link>
      <a v-else :href="item.url" :target="item.target" v-bind="props.action">
        <SkeletonWrapper :show-skeleton="isLoading" width="3em" height="1em"><span class="text-color">{{ item.label }}</span></SkeletonWrapper>
      </a>
    </template>
  </Breadcrumb>
  <div class="flex flex-xs-column flex-sm-column flex-lg-row flex-md-row gap-3 m-1 m-sm-3 m-md-3 m-lg-3 m-xl-3">
    <Card class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" style="width: 390px">
      <template #content>
        <form @submit="onSubmit">
          <InputTextWrapper v-model="name" field-name="Name" :error-text="errors.name" :show-skeleton="isLoading" @change="onSubmit" />
          <InputTextWrapper v-model="expression" field-name="Expression" disabled :show-skeleton="isLoading" @change="onSubmit" />
          <TextAreaWrapper v-model="background" field-name="Background" :error-text="errors.background" :show-skeleton="isLoading" @change="onSubmit" />
        </form>
      </template>
    </Card>
    <Card class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch">
      <template #content>
        <SmallStatDisplay />
      </template>
    </Card>
  </div>
</template>

<style scoped>
@media (max-width: 576px) {
  .flex-xs-column {
    flex-direction: column !important;
  }
}
</style>
