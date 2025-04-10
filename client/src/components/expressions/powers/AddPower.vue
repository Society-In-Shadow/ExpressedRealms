<script setup lang="ts">

import DropdownWrapper from "@/FormWrappers/DropdownWrapper.vue";
import EditorWrapper from "@/FormWrappers/EditorWrapper.vue";
import InputTextWrapper from "@/FormWrappers/InputTextWrapper.vue";
import Button from "primevue/button";
import { useForm } from "vee-validate";
import {onBeforeMount, ref} from "vue";
import axios from "axios";
import toaster from "@/services/Toasters";
import {createPowerModelSchema} from "@/components/expressions/powers/Validations/AddPowerValidations";

const props = defineProps({
  expressionId: {
    type: Number
  }
});

// Destructure `useForm` to define handlers and fields
const { defineField, handleSubmit, errors } = useForm({
  validationSchema: createPowerModelSchema
});

// Define all fields using `defineField`
const [name] = defineField("name");
const [category] = defineField("category");
const [description] = defineField("description");
const [gameMechanicEffect] = defineField("gameMechanicEffect");
const [limitation] = defineField("limitation");
const [powerDuration] = defineField("powerDuration");
const [areaOfEffect] = defineField("areaOfEffect");
const [powerLevel] = defineField("powerLevel");
const [powerActivationType] = defineField("powerActivationType");
const [other] = defineField("other");
const [isPowerUse] = defineField("isPowerUse");

interface Category {
  id: number;
  name: string;
  description: string;
}

const categories = ref<Category[]>([]);
const powerDurations = ref<Category[]>([]);
const powerLevels = ref<Category[]>([]);
const areaOfEffects = ref<Category[]>([]);
const powerActivationTypes = ref<Category[]>([]);

onBeforeMount(async () => {
  await axios.get("/powers/options")
      .then((response) => {
        
        categories.value = response.data.categories;
        powerDurations.value = response.data.powerDurations;
        powerLevels.value = response.data.powerLevels;
        areaOfEffects.value = response.data.areaOfEffects;
        powerActivationTypes.value = response.data.powerActivationTypes;
      })
})

const onSubmit = handleSubmit(async (values) => {
  await axios.post(`/powers/${props.expressionId}`, {
    expressionId: props.expressionId,
    name: values.name,
    description: values.description,
    gameMechanicEffect: values.gameMechanicEffect,
    limitation: values.limitation,
    powerDuration: values.powerDuration,
    areaOfEffect: values.areaOfEffect || 0,
    powerLevel: values.powerLevel,
    powerActivationType: values.powerActivationType,
    categoryIds: values.category,
    otherInfo: values.other,
    isPowerUse: values.isPowerUse,
  })
  .then(() => {
    //emit("addedSection");
    toaster.success("Successfully Added Power!");
    //cancelEdit();
  });
});

</script>

<template>
  <div class="m-2">
    <form @submit="onSubmit">
      <!-- Name -->
      <InputTextWrapper
          v-model="name"
          :field-name="'Name'"
          :error-text="errors.name"
      />

      <!-- Category -->
      <DropdownWrapper
          v-model="category"
          :field-name="'Category'"
          :options="categories"
          option-label="name"
          :error-text="errors.category"
      />

      <!-- Description -->
      <EditorWrapper
          v-model="description"
          :field-name="'Description'"
          :error-text="errors.description"
      />

      <!-- Game Mechanic Effect -->
      <EditorWrapper
          v-model="gameMechanicEffect"
          :field-name="'Game Mechanic Effect'"
          :error-text="errors.gameMechanicEffect"
      />

      <!-- Limitation -->
      <EditorWrapper
          v-model="limitation"
          :field-name="'Limitation'"
          :error-text="errors.limitation"
      />

      <!-- Power Duration -->
      <DropdownWrapper
          v-model="powerDuration"
          :field-name="'Power Duration'"
          :options="powerDurations"
          option-label="name"
          :error-text="errors.powerDuration"
      />

      <!-- Area Of Effect -->
      <DropdownWrapper
          v-model="areaOfEffect"
          :field-name="'Area of Effect'"
          :options="areaOfEffects"
          option-label="name"
          :error-text="errors.areaOfEffect"
      />

      <!-- Power Level -->
      <DropdownWrapper
          v-model="powerLevel"
          :field-name="'Power Level'"
          :options="powerLevels"
          option-label="name"
          :error-text="errors.powerLevels"
      />

      <!-- Power Activation Type -->
      <DropdownWrapper
          v-model="powerActivationType"
          :field-name="'Power Activation Type'"
          :options="powerActivationTypes"
          option-label="name"
          :error-text="errors.powerActivationType"
      />

      <!-- Other -->
      <EditorWrapper
          v-model="other"
          :field-name="'Other'"
          :error-text="errors.other"
      />

      <!-- Is Power Use -->
      <Checkbox
          v-model="isPowerUse"
          :field-name="'Is Power Use'"
          :options="[{ id: true, label: 'Yes' }, { id: false, label: 'No' }]"
          option-label="label"
          :error-text="errors.isPowerUse"
      />

      <!-- Submit Button -->
      <div class="float-end">
        <Button label="Submit" class="m-2" type="submit" />
      </div>
    </form>

  </div>
</template>

<style scoped>

</style>