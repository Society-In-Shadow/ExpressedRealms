<script setup lang="ts">

import {ref} from "vue";
import Button from 'primevue/button';
import EditPowerPath from "@/components/expressions/powerPaths/EditPowerPath.vue";
import {
  powerPathConfirmationPopups
} from "@/components/expressions/powerPaths/services/powerPathConfirmationPopupService";

const showEdit = ref(false);
const toggleEdit = () => {
  showEdit.value = !showEdit.value;
}

const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  },
  path:{
    type: Object,
    required: true
  }
});

const popups = powerPathConfirmationPopups(props.path.id, props.path.name);

</script>

<template>
  <div v-if="!showEdit">
    <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
      <h1 class="p-0 m-0">
        {{ props.path.name }}
      </h1>
      <div class="d-inline-flex align-items-start">
        <Button class="m-2" severity="danger" label="Delete" @click="popups.deleteConfirmation($event)" />
        <Button label="Edit" class="float-end m-2" @click="toggleEdit()" />
      </div>
    </div>
    <div class="mb-0 pb-0" v-html="props.path.description" />
  </div>
  <div v-else>
    <EditPowerPath :power-path-id="props.path.id" :expression-id="props.expressionId" @cancelled="toggleEdit()" />
  </div>
</template>
