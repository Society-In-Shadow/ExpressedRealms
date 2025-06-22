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
import type {EditPower} from "@/components/expressions/powers/types";

const form = getValidationInstance();
const power = ref<EditPower>();
const emit = defineEmits<{
  canceled: []
}>();

const props = defineProps({
  powerId: {
    type: Number,
    required: true,
  },
  powerPathId:{
    type: Number,
    required: true
  }
});

const powers = powersStore();

onBeforeMount(async () => {
  power.value = await powers.getPower(props.powerId);
  form.setValues(power.value);
})

const onSubmit = form.handleSubmit(async (values) => {
  await axios.put(`/powers/${props.powerId}`, {
    id: props.powerId,
    name: values.name,
    description: values.description,
    gameMechanicEffect: values.gameMechanicEffect,
    limitation: values.limitation,
    powerDurationId: values.powerDuration.id,
    areaOfEffectId: values.areaOfEffect.id,
    powerLevelId: values.powerLevel.id,
    powerActivationTypeId: values.powerActivationType.id,
    categoryIds: values.category?.map((item: { id: string | number }) => item.id),
    other: values.other,
    isPowerUse: values.isPowerUse,
  })
  .then(async () => {
    await powers.updatePowersByPathId(props.powerPathId);
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

      <FormDropdownWrapper
        v-model="form.powerLevel"
        :options="powers.powerLevels"
        option-label="name"
      />
      
      <FormMultiSelectWrapper
        v-model="form.category"
        :options="powers.categories"
        option-label="name"
      />

      <FormDropdownWrapper
        v-model="form.powerActivationType"
        :options="powers.powerActivationTypes"
        option-label="name"
      />

      <FormCheckboxWrapper v-model="form.isPowerUse" />
      
      <FormEditorWrapper v-model="form.description" />

      <FormEditorWrapper v-model="form.gameMechanicEffect" />

      <FormEditorWrapper v-model="form.limitation" />

      <FormDropdownWrapper
          v-model="form.areaOfEffect"
          :options="powers.areaOfEffects"
          option-label="name"
      />
      
      <FormDropdownWrapper
        v-model="form.powerDuration"
        :options="powers.powerDurations"
        option-label="name"
      />

      <FormEditorWrapper v-model="form.other" />

      <div class="m-3 text-right">
        <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
        <Button label="Update" class="m-2" type="submit" />
      </div>
    </form>
  </div>
</template>
