<script setup lang="ts">

import {onMounted, type PropType, ref} from "vue";
import type {Blessing} from "@/components/blessings/types";
import {UserRoles, userStore} from "@/stores/userStore.ts";
import Button from "primevue/button";
import AddCharacterBlessing from "@/components/characters/character/wizard/blessings/supports/AddCharacterBlessing.vue";
import {wizardContentStore} from "@/components/characters/character/wizard/stores/wizardContentStore.ts";
import type {WizardContent} from "@/components/characters/character/wizard/types.ts";

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

const hasBlessingRole = ref(false);

onMounted(async () => {
  hasBlessingRole.value = await userInfo.hasUserRole(UserRoles.BlessingsManagementRole);
})

const wizardContentInfo = wizardContentStore();
const updateWizardContent = () => {
  wizardContentInfo.updateContent(
      {
        headerName: 'Add Blessing',
        component: AddCharacterBlessing,
        props: { blessing: props.blessing }
      } as WizardContent
  )
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
      <Button class="float-end" size="small" label="View" @click="updateWizardContent" />
    </div>
  </div>
</template>
