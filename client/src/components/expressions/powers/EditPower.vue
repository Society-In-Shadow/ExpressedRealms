<script setup lang="ts">

import FormDropdownWrapper from "@/FormWrappers/FormDropdownWrapper.vue";
import FormEditorWrapper from "@/FormWrappers/FormEditorWrapper.vue";
import FormInputTextWrapper from "@/FormWrappers/FormInputTextWrapper.vue";
import Button from "primevue/button";
import {onBeforeMount, ref} from "vue";
import {getValidationInstance} from "@/components/expressions/powers/Validations/PowerValidations";
import FormCheckboxWrapper from "@/FormWrappers/FormCheckboxWrapper.vue";
import FormMultiSelectWrapper from "@/FormWrappers/FormMultiSelectWrapper.vue";
import {powersStore} from "@/components/expressions/powers/stores/powersStore";
import type {EditPower} from "@/components/expressions/powers/types";
import PowerPrerequisites from "@/components/expressions/powers/PowerPrerequisites.vue";

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
const prerequisiteChild = ref();

onBeforeMount(async () => {
  power.value = await powers.getPower(props.powerId);
  form.setValues(power.value);
})

const onSubmit = form.handleSubmit(async (values) => {
  await powers.updatePower(values, props.powerId, props.powerPathId);
  await prerequisiteChild.value.addUpdatePrerequisite(props.powerId);
  cancel();
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

      <FormInputTextWrapper v-model="form.cost" />
      
      <FormEditorWrapper v-model="form.other" />
      
      <PowerPrerequisites ref="prerequisiteChild" :power-id="props.powerId" :power-path-id="props.powerPathId" />

      <div class="m-3 text-right">
        <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
        <Button label="Update" class="m-2" type="submit" />
      </div>
    </form>
  </div>
</template>
