<script setup lang="ts">

import FormInputTextWrapper from "@/FormWrappers/FormInputTextWrapper.vue";
import Button from "primevue/button";
import {getValidationInstance} from "@/components/blessings/validations/blessingLevelForm.ts";
import {blessingsStore} from "@/components/blessings/stores/blessingsStore.ts";
import FormInputNumberWrapper from "@/FormWrappers/FormInputNumberWrapper.vue";
import {inject, type Ref, ref} from "vue";
import FormTextAreaWrapper from "@/FormWrappers/FormTextAreaWrapper.vue";

const store = blessingsStore();
const form = getValidationInstance()

const dialogRef = inject('dialogRef') as Ref;
const blessingId = ref(dialogRef.value.data.blessingId);

const onSubmit = form.handleSubmit(async (_) => {
  if(await store.addBlessingLevel(blessingId.value, form)){
    cancel();
  }
});

const cancel = () => {
  dialogRef.value.close();
}

</script>

<template>
  <form @submit="onSubmit">

    <FormInputTextWrapper v-model="form.fields.level" />
    
    <FormTextAreaWrapper v-model="form.fields.description" />

    <FormInputNumberWrapper v-model="form.fields.xpGain" />
    
    <FormInputNumberWrapper v-model="form.fields.xpCost" />

    <div class="m-3 text-right">
      <Button label="Cancel" class="m-2" type="reset" @click="cancel" />
      <Button label="Add" class="m-2" type="submit" />
    </div>
  </form>
</template>
