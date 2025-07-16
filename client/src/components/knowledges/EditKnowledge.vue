<script setup lang="ts">

import {onBeforeMount, ref} from "vue";
import {knowledgeStore} from "@/components/knowledges/stores/knowledgeStore";
import FormDropdownWrapper from "@/FormWrappers/FormDropdownWrapper.vue";
import FormInputTextWrapper from "@/FormWrappers/FormInputTextWrapper.vue";
import {getValidationInstance} from "@/components/knowledges/Validations/knowledgeValidations";
import type {EditKnowledge} from "@/components/knowledges/types";
import Button from "primevue/button";
import FormTextAreaWrapper from "@/FormWrappers/FormTextAreaWrapper.vue";

const store = knowledgeStore();

const form = getValidationInstance()
const knowledge = ref<EditKnowledge>();
const emit = defineEmits<{
  canceled: []
}>();

const props = defineProps({
  knowledgeId: {
    type: Number,
    required: true,
  }
});

onBeforeMount(async () => {
  knowledge.value = await store.getKnowledge(props.knowledgeId)
  form.setValues(knowledge.value);
})

const onSubmit = form.handleSubmit(async (values) => {
  await store.updateKnowledge(values, props.knowledgeId);
  cancel();
});

const cancel = () => {
  emit("canceled");
}

</script>

<template>
  <form @submit="onSubmit">
    <FormInputTextWrapper v-model="form.name" />
    
    <FormTextAreaWrapper v-model="form.description" />
    
    <FormDropdownWrapper
      v-model="form.knowledgeType"
      :options="store.knowledgeTypes"
      option-label="name"
    />
    
    <div class="m-3 text-right">
      <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
      <Button label="Update" class="m-2" type="submit" />
    </div>
  </form>
</template>
