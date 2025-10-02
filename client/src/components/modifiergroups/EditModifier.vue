<script setup lang="ts">

import {onBeforeMount, type PropType} from "vue";
import FormDropdownWrapper from "@/FormWrappers/FormDropdownWrapper.vue";
import {getValidationInstance} from "@/components/modifiergroups/validations/modifierValidations.ts";
import Button from "primevue/button";
import {type StatModifierReturnModel} from "@/components/modifiergroups/types.ts";
import modifierGroupStore from "@/components/modifiergroups/stores/modifierGroupStore.ts";
import FormInputNumberWrapper from "@/FormWrappers/FormInputNumberWrapper.vue";
import FormCheckboxWrapper from "@/FormWrappers/FormCheckboxWrapper.vue";

const store = modifierGroupStore();

const form = getValidationInstance()
const emit = defineEmits<{
  canceled: []
}>();

const props = defineProps({
  groupId: {
    type: Number,
    required: true,
  },
  modifier: {
    type: Object as PropType<StatModifierReturnModel>,
    required: true,
  }
});

onBeforeMount(async () => {
  form.setValues(props.modifier);
})

const onSubmit = form.handleSubmit(async (values) => {
  await store.updateModifier(values, props.groupId, props.modifier.id);
  cancel();
});

const cancel = () => {
  emit("canceled");
}

</script>

<template>
  <form @submit="onSubmit">
    
    <FormDropdownWrapper
        v-model="form.fields.modifierType"
        :options="store.modifierTypes"
        option-label="name"
    />

    <FormInputNumberWrapper v-model="form.fields.modifier" />

    <FormCheckboxWrapper v-model="form.fields.scaleWithLevel" />

    <FormCheckboxWrapper v-model="form.fields.creationSpecificBonus" />
    
    <div class="m-3 text-right">
      <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
      <Button label="Update" class="m-2" type="submit" />
    </div>
  </form>
</template>
