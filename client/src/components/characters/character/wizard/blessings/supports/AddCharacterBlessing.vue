<script setup lang="ts">

import FormTextAreaWrapper from "@/FormWrappers/FormTextAreaWrapper.vue";
import Button from "primevue/button";
import {useRoute} from "vue-router";
import {type PropType, ref, watch} from "vue";
import type {Blessing, BlessingLevel} from "@/components/blessings/types.ts";
import RadioButton from "primevue/radiobutton";
import {
  getValidationInstance
} from "@/components/characters/character/wizard/blessings/validators/blessingValidations.ts";
import {
  characterBlessingsStore
} from "@/components/characters/character/wizard/blessings/stores/characterBlessingStore.ts";
import {
  experienceStore,
  type XpSectionType,
  XpSectionTypes,
} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import Message from "primevue/message";

const store = characterBlessingsStore();
const form = getValidationInstance();
const route = useRoute();
const experienceInfo = experienceStore();
const availableXp = ref(0);

const props = defineProps({
  blessing: {
    type: Object as PropType<Blessing>,
    required: true,
  }
});

watch(() => props.blessing, async () => {
  let sectionType: XpSectionType = props.blessing.type.toLowerCase() == 'disadvantage' ? XpSectionTypes.disadvantage : XpSectionTypes.advantage;
  let xpInfo = experienceInfo.getExperienceInfoForSection(sectionType);
  availableXp.value = xpInfo.characterCreateMax - xpInfo.total;
}, {immediate: true})

const onSubmit = form.handleSubmit(async (values) => {
  await store.addBlessing(values, route.params.id, props.blessing.id);
});

function disableOption(level:BlessingLevel){
  if(props.blessing.type.toLowerCase() == 'disadvantage'){
    return level.xpGain > availableXp.value;
  }
  return level.xpCost > availableXp.value;
}

</script>

<template>
  <h1 class="pt-0 mt-0">
    {{ props.blessing.name }}
  </h1>
  
  <div v-html="props.blessing?.description"></div>
  <h3 class="d-flex justify-content-end">
    <span>Available Experience: {{ availableXp }}</span>
  </h3>
  <div v-if="availableXp == 0">
    <Message severity="warn" class="mt-4">You do not have enough experience to add this to your character.</Message>
  </div>
  <form @submit="onSubmit">
    <div v-for="level in props.blessing.levels" :key="level.id" class="mt-3">
      <div class="d-flex flex-column flex-md-row align-self-center">
        <RadioButton v-model="form.blessingLevel.field" :inputId="level.id.toString()" :value="level" class="mr-4" :disabled="disableOption(level)" />
        <label :for="level.id.toString()">{{ level.name }} – {{ level.description }}</label>
      </div>
    </div>
    
    <div class="mt-4">
      <FormTextAreaWrapper v-model="form.notes" :disabled="availableXp == 0" />
    </div>

    <div class="m-3 text-right" v-if="availableXp != 0">
      <Button label="Add" class="m-2" type="submit" />
    </div>
  </form>
</template>

<style>
:deep(th.text-center .p-datatable-column-header-content) {
  justify-content: center;
}

</style>