<script setup lang="ts">

import FormEditorWrapper from "@/FormWrappers/FormEditorWrapper.vue";
import Button from "primevue/button";
import axios from "axios";
import toaster from "@/services/Toasters";
import {onBeforeMount, type PropType} from "vue";
import {progressionPathStore} from "@/components/expressions/progressionPaths/stores/progressionPathsStore.ts";

import FormInputNumberWrapper from "@/FormWrappers/FormInputNumberWrapper.vue";
import {
  getValidationInstance
} from "@/components/expressions/progressionLevels/validations/progressionLevelValidations.ts";
import type {ProgressionLevel} from "@/components/expressions/progressionPaths/types.ts";

const form = getValidationInstance();
const progressionPathInfo = progressionPathStore();

const emit = defineEmits<{
  cancelled: []
}>();

const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  },
  progressionId: {
    type: Number,
    required: true,
  },
  level: {
    type: Object as PropType<ProgressionLevel>,
    required: true
  }
});

const onSubmit = form.handleSubmit(async (values) => {
  await axios.put(`/expression/${props.expressionId}/progressions/${props.progressionId}/levels/${props.level.id}`, {
    xlLevel: values.xlLevel,
    description: values.description,
  })
  .then(async () => {
    await progressionPathInfo.getProgressionPaths(props.expressionId);
    reset();
    toaster.success("Successfully Updated Progression Level!");
  });
});

onBeforeMount(async () => {
  form.setValues(props.level);
})

const reset = () => {
  form.customResetForm();
  emit("cancelled");
}

</script>

<template>
  <div class="m-2">
    <form @submit="onSubmit">
      <FormInputNumberWrapper v-model="form.fields.xlLevel" />

      <FormEditorWrapper v-model="form.fields.description" />

      <div class="text-right">
        <Button label="Cancel" class="m-2" type="reset" @click="reset" />
        <Button label="Submit" class="m-2" type="submit" />
      </div>
    </form>
  </div>
</template>
