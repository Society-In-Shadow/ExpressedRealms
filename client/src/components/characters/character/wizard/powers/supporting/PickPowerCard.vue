<script setup lang="ts">

import Button from "primevue/button";
import {type PropType} from "vue";
import type {Power} from "@/components/expressions/powers/types";
import {makeIdSafe} from "@/utilities/stringUtilities";
import AddCharacterPower from "@/components/characters/character/wizard/powers/supporting/AddCharacterPower.vue";
import type {WizardContent} from "@/components/characters/character/wizard/types.ts";
import {wizardContentStore} from "@/components/characters/character/wizard/stores/wizardContentStore.ts";


const props = defineProps({
  power: {
    type: Object as PropType<Power>,
    required: true,
  },
  showPickButton: {
    type: Boolean,
    required: false
  }
});

const wizardContentInfo = wizardContentStore();
const updateWizardContent = () => {
  wizardContentInfo.updateContent(
      {
        headerName: 'Power',
        component: AddCharacterPower,
        props: { power: props.power}
      } as WizardContent
  )
}

</script>

<template>
  <div :id="makeIdSafe(props.power.name)" class="d-flex flex-column flex-md-row align-self-center justify-content-between pl-3 ">
    <div>
      <h3 class="p-0 m-0">
        {{ props.power.name }}
      </h3>
      <div class="p-0 m-0">
        {{ props.power.powerLevel.name }}
      </div>
    </div>
    <div class="p-0 m-2 d-inline-flex align-items-start align-items-center">
      <Button class="float-end" size="small" label="View" @click="updateWizardContent"/>
    </div>
  </div>
</template>

<style>
@media(max-width: 768px){
  .card-body-fix .p-card-body{
    padding: 0 !important;
  }
}
</style>
