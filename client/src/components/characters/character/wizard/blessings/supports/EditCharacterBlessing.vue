<script setup lang="ts">

import FormTextAreaWrapper from "@/FormWrappers/FormTextAreaWrapper.vue";
import Button from "primevue/button";
import {useRoute} from "vue-router";
import {onBeforeMount, type PropType} from "vue";
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

onBeforeMount(async () => {
  const currentBlessing = store.blessings.filter(x => x.blessingId == props.blessing?.id)[0];
  const currentLevel = props.blessing.levels.filter(x => x.id == currentBlessing.blessingLevelId)[0];
  form.setValues(currentBlessing, currentLevel);  
})

const onSubmit = form.handleSubmit(async (values) => {
  const currentBlessing = store.blessings.filter(x => x.blessingId == props.blessing?.id)[0];
  await store.updateBlessing(values, route.params.id, currentBlessing.id);
});

</script>

<template>
  <h3 class="d-flex justify-content-between">
    <span>Experience Cost: {{ form.blessingLevel.field.xpCost }}</span>
    <span>Available Experience: Infinite</span>
  </h3>



  <form @submit="onSubmit">

    <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
      <div>
        <h2 class="p-0 m-0">
          {{ props.blessing.name }}
        </h2>
      </div>
      <div v-if="!props.isReadOnly" class="p-0 m-2 d-inline-flex align-items-start align-items-center gap-2">
<!--
        <Button label="Delete" size="small" severity="danger" @click="popupService.deleteConfirmation($event, props.knowledge.mappingId )" />
-->
        <Button label="Update" size="small" type="submit" />
      </div>
    </div>

    <div v-html="props.blessing?.description"></div>
    
    <div v-for="level in props.blessing.levels" :key="level.id" class="mt-3">
      <div class="d-flex flex-column flex-md-row align-self-center">
        <RadioButton v-model="form.blessingLevel.field" :inputId="level.id.toString()" :value="level" class="mr-4" variant="filled"/>
        <label :for="level.id.toString()">{{ level.name }} – {{ level.description }}</label>
      </div>
    </div>
    
    <div class="mt-4">
      <FormTextAreaWrapper v-model="form.notes"/>
    </div>
  </form>
</template>

<style>
:deep(th.text-center .p-datatable-column-header-content) {
  justify-content: center;
}

</style>