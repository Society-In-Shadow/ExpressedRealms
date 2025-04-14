<script setup lang="ts">

import FormDropdownWrapper from "@/FormWrappers/FormDropdownWrapper.vue";
import FormEditorWrapper from "@/FormWrappers/FormEditorWrapper.vue";
import FormInputTextWrapper from "@/FormWrappers/FormInputTextWrapper.vue";
import Button from "primevue/button";
import {onBeforeMount, ref} from "vue";
import axios from "axios";
import toaster from "@/services/Toasters";
import * as Validations from "@/components/expressions/powers/Validations/AddPowerValidations";

const props = defineProps({
  expressionId: {
    type: Number
  }
});

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

const onSubmit = Validations.handleSubmit(async (values) => {
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
      
      <FormInputTextWrapper v-model="Validations.name"/>
      
      <FormDropdownWrapper
          v-model="Validations.category"
          :options="categories"
          option-label="name"
      />

      <FormEditorWrapper v-model="Validations.description" />

      <FormEditorWrapper v-model="Validations.gameMechanicEffect" />

      <FormEditorWrapper v-model="Validations.limitation" />

      <FormDropdownWrapper
          v-model="Validations.powerDuration"
          :options="powerDurations"
          option-label="name"
      />

      <FormDropdownWrapper
          v-model="Validations.areaOfEffect"
          :options="areaOfEffects"
          option-label="name"
      />

      <FormDropdownWrapper
          v-model="Validations.powerLevel"
          :options="powerLevels"
          option-label="name"
      />

      <FormDropdownWrapper
          v-model="Validations.powerActivationType"
          :options="powerActivationTypes"
          option-label="name"
      />

      <FormEditorWrapper v-model="Validations.other" />

      <Checkbox
          v-model="isPowerUse"
          :field-name="'Is Power Use'"
          :options="[{ id: true, label: 'Yes' }, { id: false, label: 'No' }]"
          option-label="label"
          :error-text="errors.isPowerUse"
      />

      <div class="float-end">
        <Button label="Submit" class="m-2" type="submit" />
      </div>
    </form>

  </div>
</template>
