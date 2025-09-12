<script setup lang="ts">

import FormTextAreaWrapper from "@/FormWrappers/FormTextAreaWrapper.vue";
import Button from "primevue/button";
import {useRoute} from "vue-router";
import {type PropType, ref} from "vue";
import type {Blessing} from "@/components/blessings/types.ts";
import RadioButton from "primevue/radiobutton";
import {
  getValidationInstance
} from "@/components/characters/character/wizard/blessings/validators/blessingValidations.ts";
import {
  characterBlessingsStore
} from "@/components/characters/character/wizard/blessings/stores/characterBlessingStore.ts";

const store = characterBlessingsStore();
const form = getValidationInstance();
const route = useRoute();


const props = defineProps({
  blessing: {
    type: Object as PropType<Blessing>,
    required: true,
  },
  isReadOnly:{
    type: Boolean,
    required: false
  }
});

const selectedLevel = ref(0);

const onSubmit = form.handleSubmit(async (values) => {
  await store.addBlessing(values, route.params.id, props.blessing.id);
});

</script>

<template>
  <h3 class="d-flex justify-content-between">
    <span>Experience Cost: {{ selectedLevel.name }}</span>
    <span>Available Experience: Infinite</span>
  </h3>
  <h1 class="pt-0 mt-0">
    {{ props.blessing.name }}
  </h1>
  
  <div v-html="props.blessing?.description"></div>


  <form @submit="onSubmit">
    <div v-for="level in props.blessing.levels" :key="level.id" class="mt-3">
      <div class="d-flex flex-column flex-md-row align-self-center">
        <RadioButton v-model="form.blessingLevel.field" :inputId="level.id.toString()" :value="level" class="mr-4" variant="filled"/>
        <label :for="level.id.toString()">{{ level.name }} – {{ level.description }}</label>
      </div>
    </div>
    
    <div class="mt-4">
      <FormTextAreaWrapper v-model="form.notes"/>
    </div>

    <div class="m-3 text-right">
      <Button label="Add" class="m-2" type="submit" />
    </div>
  </form>
</template>

<style>
:deep(th.text-center .p-datatable-column-header-content) {
  justify-content: center;
}

</style>