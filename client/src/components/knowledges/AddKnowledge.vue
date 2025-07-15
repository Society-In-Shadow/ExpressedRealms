<script setup lang="ts">

import {knowledgeStore} from "@/components/knowledges/stores/knowledgeStore";
import FormDropdownWrapper from "@/FormWrappers/FormDropdownWrapper.vue";
import FormEditorWrapper from "@/FormWrappers/FormEditorWrapper.vue";
import FormInputTextWrapper from "@/FormWrappers/FormInputTextWrapper.vue";
import {getValidationInstance} from "@/components/knowledges/Validations/knowledgeValidations";
import Button from "primevue/button";

const store = knowledgeStore();

const form = getValidationInstance()
const emit = defineEmits<{
  canceled: []
}>();

const onSubmit = form.handleSubmit(async (values) => {
  await store.addKnowledge(values);
  cancel();
});

const cancel = () => {
  emit("canceled");
}

</script>

<template>
  <form @submit="onSubmit">
    <FormInputTextWrapper v-model="form.name" />

    <FormEditorWrapper v-model="form.description" />

    <FormDropdownWrapper
        v-model="form.knowledgeType"
        :options="store.knowledgeTypes"
        option-label="name"
    />

    <div class="m-3 text-right">
      <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
      <Button label="Add" class="m-2" type="submit" />
    </div>
  </form>
</template>
