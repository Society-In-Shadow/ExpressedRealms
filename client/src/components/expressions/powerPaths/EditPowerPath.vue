<script setup lang="ts">

import FormEditorWrapper from "@/FormWrappers/FormEditorWrapper.vue";
import FormInputTextWrapper from "@/FormWrappers/FormInputTextWrapper.vue";
import Button from "primevue/button";
import axios from "axios";
import toaster from "@/services/Toasters";
import {getValidationInstance} from "@/components/expressions/powerPaths/validations/powerPathValidations";
import {powerPathStore} from "@/components/expressions/powerPaths/stores/powerPathStore";
import {onBeforeMount} from "vue";

const form = getValidationInstance();
const powerPaths = powerPathStore();

const emit = defineEmits<{
  cancelled: []
}>();

const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  },
  powerPathId: {
    type: Number,
    required: true,
  }
});

const onSubmit = form.handleSubmit(async (values) => {
  await axios.put(`/powerpath/${props.powerPathId}`, {
    id: props.powerPathId,
    name: values.name,
    description: values.description,
  })
  .then(async () => {
    await powerPaths.getPowerPaths(props.expressionId);
    reset();
    toaster.success("Successfully Updated Power Path!");
  });
});

onBeforeMount(async () => {
  const powerPath = await powerPaths.getPowerPath(props.powerPathId);
  form.setValues(powerPath);
})

const reset = () => {
  form.customResetForm();
  emit("cancelled");
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
