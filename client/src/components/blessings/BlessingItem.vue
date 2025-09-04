<script setup lang="ts">

import {type PropType, ref} from "vue";
import type {Blessing} from "@/components/blessings/types";
import EditBlessing from "@/components/blessings/EditBlessing.vue";
import {UserRoles, userStore} from "@/stores/userStore.ts";
import Button from "primevue/button";
import {blessingConfirmationPopup} from "@/components/blessings/services/blessingConfirmationPopupService.ts";

const popups = blessingConfirmationPopup();
const userInfo = userStore();
const props = defineProps({
  blessing: {
    type: Object as PropType<Blessing>,
    required: true,
  },
  isReadOnly: {
    type: Boolean,
    required: true
  }
});

const showEdit = ref(false);

function toggleEdit(){
  showEdit.value = !showEdit.value;
}
</script>

<template>
  <div v-if="showEdit" class="mb-2">
    <EditBlessing :blessing="props.blessing" @canceled="toggleEdit" />
  </div>
  <div class="d-flex flex-column flex-md-row align-self-center justify-content-between mt-4">
    <div>
      <h1 class="p-0 m-0">
        {{ props.blessing.name }}
      </h1>
      <div class="p-0 m-0">
        {{ props.blessing?.subCategory }}
      </div>
    </div>
    <div
        v-if="!showEdit && userInfo.hasUserRole(UserRoles.BlessingsManagementRole) && !props.isReadOnly"
        class="p-0 m-0 d-inline-flex align-items-start"
    >
      <Button class="mr-2" severity="danger" label="Delete" @click="popups.deleteConfirmation($event, props.blessing.id)" />
      <Button class="float-end" label="Edit" @click="toggleEdit" />
    </div>
  </div>
  <p>{{ props.blessing.description }}</p>
  <ul>
    <li v-for="level in props.blessing.levels" :key="level.id" class="mt-3">
      <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
        <div>{{ level.name }} – {{ level.description }}</div>
      </div>
    </li>
  </ul>
</template>
