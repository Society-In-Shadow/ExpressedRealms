<script setup lang="ts">

import {onMounted, type PropType, ref} from "vue";
import type {Blessing} from "@/components/blessings/types";
import EditBlessing from "@/components/blessings/EditBlessing.vue";
import {UserRoles, userStore} from "@/stores/userStore.ts";
import Button from "primevue/button";
import {blessingConfirmationPopup} from "@/components/blessings/services/blessingConfirmationPopupService.ts";
import {addBlessingDialog} from "@/components/blessings/services/dialogs.ts";

const popups = blessingConfirmationPopup();
const dialogs = addBlessingDialog();
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
const hasBlessingRole = ref(false);

onMounted(async () => {
  hasBlessingRole.value = await userInfo.hasUserRole(UserRoles.BlessingsManagementRole);
})

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
      <h3 class="p-0 m-0">
        {{ props.blessing.name }}
      </h3>
    </div>
    <div
        v-if="!showEdit && hasBlessingRole && !props.isReadOnly"
        class="p-0 m-0 d-inline-flex align-items-start"
    >
      <Button class="mr-2" severity="danger" label="Delete" @click="popups.deleteConfirmation($event, props.blessing.id)" />
      <Button class="float-end" label="Edit" @click="toggleEdit" />
    </div>
  </div>
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
</template>
