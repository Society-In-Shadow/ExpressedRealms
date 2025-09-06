<script setup lang="ts">

import FormInputTextWrapper from "@/FormWrappers/FormInputTextWrapper.vue";
import Button from "primevue/button";
import {getValidationInstance} from "@/components/blessings/validations/blessingLevelForm.ts";
import {blessingsStore} from "@/components/blessings/stores/blessingsStore.ts";
import FormEditorWrapper from "@/FormWrappers/FormEditorWrapper.vue";

const store = blessingsStore();

const form = getValidationInstance()
const emit = defineEmits<{
  canceled: []
}>();


const onSubmit = form.handleSubmit(async (values) => {
  form.setFieldError("level", "this is a test v2");
  //await store.addBlessingLevel(form, values);
  cancel();
});

const cancel = () => {
  emit("canceled");
}

</script>

<template>
  <form @submit="onSubmit">

    <FormInputTextWrapper v-model="form.type" />
    
    <FormInputTextWrapper v-model="form.subCategory" />
    
    <FormInputTextWrapper v-model="form.name" />

    <FormEditorWrapper v-model="form.description" />

    <div class="m-3 text-right">
      <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
      <Button label="Add" class="m-2" type="submit" />
    </div>
  </form>
</template>
