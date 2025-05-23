<script setup lang="ts">

import FormDropdownWrapper from "@/FormWrappers/FormDropdownWrapper.vue";
import FormEditorWrapper from "@/FormWrappers/FormEditorWrapper.vue";
import FormInputTextWrapper from "@/FormWrappers/FormInputTextWrapper.vue";
import Button from "primevue/button";
import {onBeforeMount, ref} from "vue";
import axios from "axios";
import toaster from "@/services/Toasters";
import * as form from "@/components/expressions/powers/Validations/AddPowerValidations";
import FormCheckboxWrapper from "@/FormWrappers/FormCheckboxWrapper.vue";
import type {Category} from "@/components/expressions/powers/Validations/AddPowerValidations";
import FormMultiSelectWrapper from "@/FormWrappers/FormMultiSelectWrapper.vue";

const props = defineProps({
  expressionId: {
    type: Number
  }
});

const categories = ref<Category[]>([]);
const powerDurations = ref<Category[]>([]);
const powerLevels = ref<Category[]>([]);
const areaOfEffects = ref<Category[]>([]);
const powerActivationTypes = ref<Category[]>([]);

onBeforeMount(async () => {
  await axios.get("/powers/options")
      .then((response) => {
        categories.value = response.data.category;
        powerDurations.value = response.data.powerDuration;
        powerLevels.value = response.data.powerLevel;
        areaOfEffects.value = response.data.areaOfEffect;
        powerActivationTypes.value = response.data.powerActivationType;
      })
})

const onSubmit = form.handleSubmit(async (values) => {
  await axios.post(`/powers/${props.expressionId}`, {
    expressionId: props.expressionId,
    name: values.name,
    description: values.description,
    gameMechanicEffect: values.gameMechanicEffect,
    limitation: values.limitation,
    powerDuration: values.powerDuration.id,
    areaOfEffect: values.areaOfEffect.id,
    powerLevel: values.powerLevel.id,
    powerActivationType: values.powerActivationType.id,
    categoryIds: values.category.map((item: { id: string | number }) => item.id),
    other: values.other,
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
      
      <FormInputTextWrapper v-model="form.name"/>
      
      <FormMultiSelectWrapper
          v-model="form.category"
          :options="categories"
          option-label="name"
      />

      <FormEditorWrapper v-model="form.description" />

      <FormEditorWrapper v-model="form.gameMechanicEffect" />

      <FormEditorWrapper v-model="form.limitation" />

      <FormDropdownWrapper
          v-model="form.powerDuration"
          :options="powerDurations"
          option-label="name"
      />

      <FormDropdownWrapper
          v-model="form.areaOfEffect"
          :options="areaOfEffects"
          option-label="name"
      />

      <FormDropdownWrapper
          v-model="form.powerLevel"
          :options="powerLevels"
          option-label="name"
      />

      <FormDropdownWrapper
          v-model="form.powerActivationType"
          :options="powerActivationTypes"
          option-label="name"
      />

      <FormEditorWrapper v-model="form.other" />

      <FormCheckboxWrapper v-model="form.isPowerUse" />

      <div class="float-end">
        <Button label="Submit" class="m-2" type="submit" />
      </div>
    </form>

  </div>
</template>
