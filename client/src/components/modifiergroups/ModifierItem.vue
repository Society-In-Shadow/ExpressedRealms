<script setup lang="ts">

import {onMounted, type PropType, ref} from "vue";
import {UserRoles, userStore} from "@/stores/userStore";
import Button from "primevue/button";
import {type StatModifierReturnModel,} from "@/components/modifiergroups/types.ts";
import {modifierConfirmationPopup} from "@/components/modifiergroups/services/confirmationPopupService.ts";
import EditModifier from "@/components/modifiergroups/EditModifier.vue";
import Tag from "primevue/tag";

let userInfo = userStore();

const props = defineProps({
  groupId: {
    type: Number,
    required: true
  },
  modifier: {
    type: Object as PropType<StatModifierReturnModel>,
    required: true,
  },
  isReadOnly: {
    type: Boolean,
    required: true
  }
});

let popups = modifierConfirmationPopup()

const showEdit = ref(false);

const hasKnowledgeManagementRole = ref(false);

onMounted(async () => {
  hasKnowledgeManagementRole.value = await userInfo.hasUserRole(UserRoles.KnowledgeManagementRole);
})

function toggleEdit(){
  showEdit.value = !showEdit.value;
}

function formatWithSign(number: number) {
  return (number > 0 ? '+' : '') + number;
}
</script>

<template>
  <div v-if="showEdit" class="mb-2">
    <EditModifier :group-id="props.groupId" :modifier="props.modifier" @canceled="toggleEdit" />
  </div>
  <div v-else class="d-flex flex-column flex-md-row align-self-center justify-content-between m-2">
    <div class="align-self-center">{{ formatWithSign(props.modifier.modifier) }} {{ props.modifier?.statModifier.name }} <Tag v-if="props.modifier?.scaleWithLevel" severity="info"><span title="Scales with Level">Scales with Level</span></Tag></div>
    <div
      v-if="!showEdit && hasKnowledgeManagementRole && !props.isReadOnly"
      class="p-0 m-0 d-inline-flex align-items-start"
    >
      <Button class="mr-2" severity="danger" label="Delete" @click="popups.deleteConfirmation($event, props.groupId, props.modifier.id)" />
      <Button class="float-end" label="Edit" @click="toggleEdit" />
    </div>
  </div>
</template>
