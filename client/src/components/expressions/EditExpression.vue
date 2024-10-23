<script setup lang="ts">

import {useForm} from "vee-validate";
import {object, string, number} from "yup";
import InputTextWrapper from "@/FormWrappers/InputTextWrapper.vue";
import TextAreaWrapper from "@/FormWrappers/TextAreaWrapper.vue";
import DropdownWrapper from "@/FormWrappers/DropdownWrapper.vue";
import {onMounted, ref} from "vue";
import axios from "axios";
import toaster from "@/services/Toasters";

const isLoading = ref(true);
const publishStatusOptions = ref([]);

const emit = defineEmits<{
  refreshList: []
}>();

const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  }
});

onMounted(() =>{
  axios.get(`/expression/${props.expressionId}`)
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
  axios.put(`/expression/${props.expressionId}`, {
    name: values.name,
    shortDescription: values.shortDescription,
    id: props.expressionId,
    publishStatus: values.publishStatus.id,
    navMenuImage: values.navMenuImage
  }).then(() => {
    emit('refreshList');
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
    <DropdownWrapper
      v-model="publishStatus" option-label="name" :options="publishStatusOptions" field-name="Publish Status" :error-text="errors.publishStatus"
      :show-skeleton="isLoading" @change="onSubmit"
    />
  </form>
</template>

<style scoped>

</style>
