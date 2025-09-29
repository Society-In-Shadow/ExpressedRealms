<script setup lang="ts">

import {computed, type PropType, ref, watch} from "vue";
import axios from "axios";
import FormDropdownWrapper from "@/FormWrappers/FormDropdownWrapper.vue";
import type {FormField} from "@/FormWrappers/Interfaces/FormField.ts";
import type {ProgressionPath} from "@/components/characters/character/wizard/basicInfo/types.ts";
import {characterStore} from "@/components/characters/character/stores/characterStore.ts";
import Message from "primevue/message";


const emit = defineEmits<{
  change: []
}>();

const props = defineProps({
  expressionTypeId: {
    type: Number,
    required: true,
  },
  primaryProgression: {
    type: Object as PropType<FormField>,
    required: true,
  },
  secondaryProgression: {
    type: Object as PropType<FormField>,
    required: true,
  }
});

const characterInfo = characterStore();

const adeptId = 3;
const shammasId = 4;
const sorcererId = 8;
const vampyreId = 9;

const progressionOptions = ref<ProgressionPath[]>([]);
const isLoading = ref(true);
const primaryExpressions = [adeptId, shammasId, sorcererId, vampyreId]

watch(() => props.expressionTypeId, async () => {
  isLoading.value = true;
  const results = await axios.get(`/characters/progressionOptions/${props.expressionTypeId}`);
  progressionOptions.value = results.data;
  isLoading.value = false;
  props.primaryProgression.field.value = progressionOptions.value.find(x => x.id == characterInfo.primaryProgressionId) as ProgressionPath;
  props.secondaryProgression.field.value = progressionOptions.value.find(x => x.id == characterInfo.secondaryProgressionId) as ProgressionPath;
}, {immediate: true, deep: true})

const showPrimary = computed(() => primaryExpressions.includes(props.expressionTypeId))
const showSecondary = computed(() => {
  return props.expressionTypeId == sorcererId && props.primaryProgression?.field.value != null}
);

const secondaryProgressionOptions = computed<ProgressionPath[]>(() => {
  if(props.primaryProgression.field.value == null && props.expressionTypeId == sorcererId) {
    return [];
  }
  
  const fireOpposites = ["Water", "Fire"];
  if(fireOpposites.includes(props.primaryProgression?.field?.value.name)){
     return progressionOptions.value.filter(x => !fireOpposites.includes(x.name));
  }
  
  const earthOpposites = ["Earth", "Air"];
  if(earthOpposites.includes(props.primaryProgression?.field?.value.name)){
    return progressionOptions.value.filter(x => !earthOpposites.includes(x.name));
  }
});

const primaryLabel = computed(() => {
  if(props.expressionTypeId == adeptId)
  {
     return "Orientation";
  }
  if(props.expressionTypeId == shammasId)
  {
     return "Breed";
  }
  if(props.expressionTypeId == sorcererId)
  {
     return "Primary Elemental";
  }
  if(props.expressionTypeId == vampyreId) {
    return "Vampyric Orientation";
  }
  
})

</script>

<template>
  <FormDropdownWrapper v-if="showPrimary && !isLoading" v-model="props.primaryProgression" option-label="name" :options="progressionOptions" :label-override="primaryLabel" @change="emit('change')" />

  <FormDropdownWrapper v-if="showSecondary && !isLoading" v-model="props.secondaryProgression" option-label="name" :options="secondaryProgressionOptions" label-override="Secondary Elemental" @change="emit('change')"/>
  <div v-if="showSecondary">
    <Message severity="info" :closable="false">
      The Secondary element is made to complement the Primary, and the character may only gain up to Intermediate 
      Powers within this element. The Secondary element cannot be the opposite of their Primary. For example, a 
      Sorcerer could have a primary mastery of Earth and a secondary mastery in Fire or Water, but not Air.
    </Message>
  </div>
</template>
