<script setup lang="ts">

import {onMounted, type PropType, ref} from "vue";
import type {Blessing} from "@/components/blessings/types";
import {UserRoles, userStore} from "@/stores/userStore.ts";
import Button from "primevue/button";
import {blessingConfirmationPopup} from "@/components/blessings/services/blessingConfirmationPopupService.ts";
import {addBlessingDialog} from "@/components/blessings/services/dialogs.ts";
import AddCharacterBlessing from "@/components/characters/character/wizard/blessings/supports/AddCharacterBlessing.vue";
import {
  characterBlessingsStore
} from "@/components/characters/character/wizard/blessings/stores/characterBlessingStore.ts";

const popups = blessingConfirmationPopup();
const dialogs = addBlessingDialog();
const userInfo = userStore();

const characterBlessingData = characterBlessingsStore();

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

const toggleAdd = () => {
  characterBlessingData.activeBlessingId = props.blessing.id;
}

</script>

<template>
  <div class="pl-5 d-flex flex-column flex-md-row align-self-center justify-content-between">
    <div>
      <h3 class="p-0 m-0">
        {{ props.blessing.name }}
      </h3>
    </div>
    <div v-if="!props.isReadOnly" class="p-0 m-2 d-inline-flex align-items-start align-items-center">
      <Button class="float-end" size="small" label="View" @click="toggleAdd" />
    </div>
  </div>
  <Teleport to="#item-modification-section" v-if="characterBlessingData.activeBlessingId == props.blessing.id">
    <AddCharacterBlessing :blessing="props.blessing" />
  </Teleport>
</template>
