<script setup lang="ts">

import Button from "primevue/button";
import {type PropType} from "vue";
import type {Power} from "@/components/expressions/powers/types";
import {makeIdSafe} from "@/utilities/stringUtilities";
import {characterPowersStore} from "@/components/characters/character/powers/stores/characterPowerStore.ts";
import AddCharacterPower from "@/components/characters/character/wizard/powers/supporting/AddCharacterPower.vue";

const powerData = characterPowersStore();
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

const toggleAdd = () => {
  powerData.activePowerId = props.power.id;
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
      <Button class="float-end" size="small" label="View" @click="toggleAdd"/>
    </div>
  </div>
  <Teleport v-if="powerData.activePowerId == props.power?.id" to="#item-modification-section">
    <AddCharacterPower :power="props.power" />
  </Teleport>
</template>

<style>
@media(max-width: 768px){
  .card-body-fix .p-card-body{
    padding: 0 !important;
  }
}
</style>
