<script setup lang="ts">

import FormDropdownWrapper from "@/FormWrappers/FormDropdownWrapper.vue";
import FormEditorWrapper from "@/FormWrappers/FormEditorWrapper.vue";
import FormInputTextWrapper from "@/FormWrappers/FormInputTextWrapper.vue";
import Button from "primevue/button";
import {onBeforeMount, ref} from "vue";
import axios from "axios";
import toaster from "@/services/Toasters";
import {getValidationInstance} from "@/components/expressions/powers/Validations/PowerValidations";
import FormCheckboxWrapper from "@/FormWrappers/FormCheckboxWrapper.vue";
import FormMultiSelectWrapper from "@/FormWrappers/FormMultiSelectWrapper.vue";
import {powersStore} from "@/components/expressions/powers/stores/powersStore";
import type {EditPower, Power} from "@/components/expressions/powers/types/power";
import {expressionStore} from "@/stores/expressionStore";
const expressionInfo = expressionStore();

const powers = powersStore();
const form = getValidationInstance();

const emit = defineEmits<{
  canceled: []
}>();

const props = defineProps({
  powerId: {
    type: Number,
    required: true,
  },
});

onBeforeMount(async () => {
  power.value = await powers.getPower(expressionInfo.currentExpressionId, props.powerId);
  form.setValues(power.value);
})

const onSubmit = form.handleSubmit(async (values) => {
  await axios.put(`/powers/${props.powerId}`, {
    expressionId: expressionInfo.currentExpressionId,
    id: props.powerId,
    name: values.name,
    description: values.description,
    gameMechanicEffect: values.gameMechanicEffect,
    limitation: values.limitation,
    powerDurationId: values.powerDuration.id,
    areaOfEffectId: values.areaOfEffect.id,
    powerLevelId: values.powerLevel.id,
    powerActivationTypeId: values.powerActivationType.id,
    categoryIds: values.category.map((item: { id: string | number }) => item.id),
    other: values.other,
    isPowerUse: values.isPowerUse,
  })
  .then(async () => {
    await powers.getPowers(expressionInfo.currentExpressionId);
    toaster.success("Successfully Updated Power!");
    cancel();
  });
});

const cancel = () => {
  emit("canceled");
}

</script>

<template>
  <div class="m-2">
    <form @submit="onSubmit">
      <FormInputTextWrapper v-model="form.name" />
      
      <FormMultiSelectWrapper
        v-model="form.category"
        :options="powers.categories"
        option-label="name"
      />

      <FormEditorWrapper v-model="form.description" />

      <FormEditorWrapper v-model="form.gameMechanicEffect" />

      <FormEditorWrapper v-model="form.limitation" />

      <FormDropdownWrapper
        v-model="form.powerDuration"
        :options="powers.powerDurations"
        option-label="name"
      />

      <FormDropdownWrapper
        v-model="form.areaOfEffect"
        :options="powers.areaOfEffects"
        option-label="name"
      />

      <FormDropdownWrapper
        v-model="form.powerLevel"
        :options="powers.powerLevels"
        option-label="name"
      />

      <FormDropdownWrapper
        v-model="form.powerActivationType"
        :options="powers.powerActivationTypes"
        option-label="name"
      />

      <FormEditorWrapper v-model="form.other" />

      <FormCheckboxWrapper v-model="form.isPowerUse" />

      <div class="m-3 text-right">
        <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
        <Button label="Update" class="m-2" type="submit" />
      </div>
    </form>
  </div>
</template>
