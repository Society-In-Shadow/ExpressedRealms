<script setup lang="ts">

import {useForm} from "vee-validate";
import {object, string, number} from "yup";
import InputTextWrapper from "@/FormWrappers/InputTextWrapper.vue";
import TextAreaWrapper from "@/FormWrappers/TextAreaWrapper.vue";
import DropdownWrapper from "@/FormWrappers/DropdownWrapper.vue";
import {inject, onMounted, ref} from "vue";
import axios from "axios";
import toaster from "@/services/Toasters";
import {cmsStore} from "@/stores/cmsStore.ts";

const isLoading = ref(true);
const publishStatusOptions = ref([]);
const cmsData = cmsStore();

const dialogRef = inject('dialogRef');
const expressionId = ref(dialogRef.value.data.expressionId);

onMounted(() =>{
  axios.get(`/expression/${expressionId.value}`)
      .then((response) => {
        name.value = response.data.name;
        shortDescription.value = response.data.shortDescription;
        navMenuImage.value = response.data.navMenuImage;
        publishStatus.value = response.data.publishTypes.find(x => x.id == response.data.publishStatus);
        publishStatusOptions.value = response.data.publishTypes;
        isLoading.value = false;
      })
});

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
        .label('Nav Menu Icon'),
    publishStatus: object().required()
        .label('Publish Status')
  })
});

const [name] = defineField('name');
const [shortDescription] = defineField('shortDescription');
const [navMenuImage] = defineField('navMenuImage');
const [publishStatus] = defineField('publishStatus');

const onSubmit = handleSubmit((values) => {
  axios.put(`/expression/${expressionId.value}`, {
    name: values.name,
    shortDescription: values.shortDescription,
    id: expressionId.value,
    publishStatus: values.publishStatus.id,
    navMenuImage: values.navMenuImage
  }).then(() => {
    cmsData.refreshCmsInformation();
    toaster.success("Successfully Updated Expression Info!");
  });
});

</script>

<template>
  <form @submit="onSubmit">
    <InputTextWrapper v-model="name" field-name="Name" :error-text="errors.name" :show-skeleton="isLoading" @change="onSubmit" />
    <TextAreaWrapper v-model="shortDescription" field-name="Short Description" :error-text="errors.shortDescription" :show-skeleton="isLoading" @change="onSubmit" />
    <InputTextWrapper v-model="navMenuImage" field-name="Nav Menu Icon" :error-text="errors.navMenuImage" :show-skeleton="isLoading" @change="onSubmit" />
    <p>List of icons can be found here : <a href="https://primevue.org/icons/#list">Primevue Icons</a></p>
    <p>Additional Icons can be found here: <a href="https://icons.getbootstrap.com/#:~:text=%E2%80%A2%20GitHub%20repo-,Icons,-Search%20for%20icons">Bootstrap Icons</a></p>
    <p>For bootstrap, click on icon and look for a string like this "bi bi-0-square"</p>
    <DropdownWrapper
      v-model="publishStatus" option-label="name" :options="publishStatusOptions" field-name="Publish Status" :error-text="errors.publishStatus"
      :show-skeleton="isLoading" @change="onSubmit"
    />
  </form>
</template>
