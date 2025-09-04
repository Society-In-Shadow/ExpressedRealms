<script setup lang="ts">

import FormInputTextWrapper from "@/FormWrappers/FormInputTextWrapper.vue";
import Button from "primevue/button";
import {getValidationInstance} from "@/components/blessings/validations/blessingForm.ts";
import {blessingsStore} from "@/components/blessings/stores/blessingsStore.ts";
import {onBeforeMount, type PropType} from "vue";
import type {Blessing} from "@/components/blessings/types.ts";
import FormEditorWrapper from "@/FormWrappers/FormEditorWrapper.vue";

const store = blessingsStore();

const form = getValidationInstance()
const emit = defineEmits<{
  canceled: []
}>();

const props = defineProps({
  blessing: {
    type: Object as PropType<Blessing>,
    required: true,
  }
});

onBeforeMount(async () => {
  form.setValues(props.blessing);
})


const onSubmit = form.handleSubmit(async (values) => {
  await store.editBlessing(props.blessing.id, values);
  cancel();
});

const cancel = () => {
  emit("canceled");
}

</script>

<template>
  <form @submit="onSubmit">

    <FormInputTextWrapper v-model="form.type" />
    
    <FormInputTextWrapper v-model="form.subCategory" />
    
    <FormInputTextWrapper v-model="form.name" />

    <FormEditorWrapper v-model="form.description" />

    <div class="m-3 text-right">
      <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
      <Button label="Update" class="m-2" type="submit" />
    </div>
  </form>
</template>
