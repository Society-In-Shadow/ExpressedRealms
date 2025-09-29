<script setup lang="ts">

import FormEditorWrapper from "@/FormWrappers/FormEditorWrapper.vue";
import FormInputTextWrapper from "@/FormWrappers/FormInputTextWrapper.vue";
import Button from "primevue/button";
import axios from "axios";
import toaster from "@/services/Toasters";
import {progressionPathStore} from "@/components/expressions/progressionPaths/stores/progressionPathsStore.ts";
import {
  getValidationInstance
} from "@/components/expressions/progressionPaths/validations/progressionPathValidations.ts";

const form = getValidationInstance();
const progressionPathInfo = progressionPathStore();

const emit = defineEmits<{
  canceled: []
}>();

const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  }
});

const onSubmit = form.handleSubmit(async (values) => {
  await axios.post(`/expression/${props.expressionId}/progressions`, {
    expressionId: props.expressionId,
    name: values.name,
    description: values.description,
  })
  .then(async () => {
    await progressionPathInfo.getProgressionPaths(props.expressionId);
    reset();
    toaster.success("Successfully Added Progression Path!");
  });
});

const reset = () => {
  form.customResetForm();
  emit("canceled");
}

</script>

<template>
  <div class="m-2">
    <form @submit="onSubmit">
      <FormInputTextWrapper v-model="form.name" />

      <FormEditorWrapper v-model="form.description" />

      <div class="text-right">
        <Button label="Cancel" class="m-2" type="reset" @click="reset" />
        <Button label="Submit" class="m-2" type="submit" />
      </div>
    </form>
  </div>
</template>
