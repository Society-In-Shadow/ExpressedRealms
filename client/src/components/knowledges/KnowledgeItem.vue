<script setup lang="ts">

import {type PropType, ref} from "vue";
import type {Knowledge} from "@/components/knowledges/types";
import {UserRoles, userStore} from "@/stores/userStore";
import Button from "primevue/button";
import {knowledgeConfirmationPopup} from "@/components/knowledges/services/knowledgeConfirmationPopupService";
import EditKnowledge from "@/components/knowledges/EditKnowledge.vue";

let userInfo = userStore();

const props = defineProps({
  knowledge: {
    type: Object as PropType<Knowledge>,
    required: true,
  },
  isReadOnly: {
    type: Boolean,
    required: true
  }
});

let popups = knowledgeConfirmationPopup(props.knowledge.id, props.knowledge.name)

const showEdit = ref(false);

function toggleEdit(){
  showEdit.value = !showEdit.value;
}
</script>

<template>
  <div v-if="showEdit" class="mb-2">
    <EditKnowledge :knowledge-id="props.knowledge.id" @canceled="toggleEdit" />
  </div>
  <div v-else class="d-flex flex-column flex-md-row align-self-center justify-content-between">
    <div>
      <h1 class="p-0 m-0">
        {{ props.knowledge.name }}
      </h1>
      <div class="p-0 m-0">
        {{ props.knowledge.typeName }}
      </div>
    </div>
    <div
      v-if="!showEdit && userInfo.hasUserRole(UserRoles.PowerManagementRole) && !props.isReadOnly"
      class="p-0 m-0 d-inline-flex align-items-start"
    >
      <Button class="mr-2" severity="danger" label="Delete" @click="popups.deleteConfirmation($event)" />
      <Button class="float-end" label="Edit" @click="toggleEdit" />
    </div>
  </div>
  <p>{{ props.knowledge.description }}</p>
</template>
