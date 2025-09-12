<script setup lang="ts">

import FormTextAreaWrapper from "@/FormWrappers/FormTextAreaWrapper.vue";
import Button from "primevue/button";
import {getValidationInstance} from "@/components/characters/character/knowledges/validations/knowledgeValidations";
import {characterKnowledgeStore} from "@/components/characters/character/knowledges/stores/characterKnowledgeStore";
import {useRoute} from "vue-router";
import {type PropType} from "vue";
import type {Blessing} from "@/components/blessings/types.ts";

const store = characterKnowledgeStore();
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

/*onBeforeMount(async () => {
  await store.getKnowledgeLevels(route.params.id);

})*/

const onSubmit = form.handleSubmit(async (values) => {
  await store.addKnowledge(values, route.params.id, props.blessing.id);
});

</script>

<template>
  <h1 class="pt-0 mt-0">
    {{ props.blessing.name }}
  </h1>
  <div v-html="props.blessing?.description"></div>
  <ul>
    <li v-for="level in props.blessing.levels" :key="level.id" class="mt-3">
      <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
        <div>{{ level.name }} – {{ level.description }}</div>
        <div
            v-if="!showEdit && hasBlessingRole && !props.isReadOnly"
            class="p-0 m-0 d-inline-flex align-items-start"
        >
          <Button class="mr-2" severity="danger" label="Delete" @click="popups.deleteBlessingLevelConfirmation($event, props.blessing.id, level.id)" />
          <Button class="float-end" label="Edit" @click="dialogs.showEditBlessingLevel(props.blessing.id, level.id)" />
        </div>
      </div>
    </li>
    <li v-if="hasBlessingRole">
      <Button label="Add Level" @click="dialogs.showAddBlessingLevel(props.blessing.id)"/>
    </li>
  </ul>
  <h3 class="text-right">
    Available Experience: {{ store.currentExperience }}
  </h3>
  <form @submit="onSubmit">

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