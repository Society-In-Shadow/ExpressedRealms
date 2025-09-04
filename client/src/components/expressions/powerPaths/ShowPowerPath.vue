<script setup lang="ts">

import {onMounted, ref} from "vue";
import Button from 'primevue/button';
import EditPowerPath from "@/components/expressions/powerPaths/EditPowerPath.vue";
import {
  powerPathConfirmationPopups
} from "@/components/expressions/powerPaths/services/powerPathConfirmationPopupService";
import {UserRoles, userStore} from "@/stores/userStore";
import {makeIdSafe} from "@/utilities/stringUtilities";

let userInfo = userStore();

const showEdit = ref(false);
const toggleEdit = () => {
  showEdit.value = !showEdit.value;
}

const hasPowerManagementRole = ref(false);

onMounted(async () => {
  hasPowerManagementRole.value = await userInfo.hasUserRole(UserRoles.PowerManagementRole);
})

const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  },
  path:{
    type: Object,
    required: true
  },
  isReadOnly:{
    type: Boolean,
    required: false
  }
});

const popups = powerPathConfirmationPopups(props.path.id, props.path.name);

</script>

<template>
  <div v-if="!showEdit" :id="makeIdSafe(props.path.name)">
    <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
      <h1 class="p-0 m-0">
        {{ props.path.name }}
      </h1>
      <div v-if="hasPowerManagementRole && !props.isReadOnly" class="d-inline-flex align-items-start">
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
