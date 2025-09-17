<script setup lang="ts">

import FormTextAreaWrapper from "@/FormWrappers/FormTextAreaWrapper.vue";
import Button from "primevue/button";
import {useRoute} from "vue-router";
import {onBeforeMount, type PropType, ref, watch} from "vue";
import type {Blessing, BlessingLevel} from "@/components/blessings/types.ts";
import RadioButton from "primevue/radiobutton";
import {
  getValidationInstance
} from "@/components/characters/character/wizard/blessings/validators/blessingValidations.ts";
import {
  characterBlessingsStore
} from "@/components/characters/character/wizard/blessings/stores/characterBlessingStore.ts";
import {confirmationPopup} from "@/components/characters/character/wizard/blessings/services/confirmationService.ts";
import {
  experienceStore,
  type XpSectionType,
  XpSectionTypes
} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import Message from "primevue/message";

const store = characterBlessingsStore();
const form = getValidationInstance();
const route = useRoute();
const popupService = confirmationPopup(route.params.id);
const experienceInfo = experienceStore();

const props = defineProps({
  blessing: {
    type: Object as PropType<Blessing>,
    required: true,
  }
});

const mappingId = ref(0);
const currentLevel = ref<BlessingLevel>({});
const availableXp = ref(0);

onBeforeMount(async () => {
  await loadData();
});

watch(() => props.blessing, async () => {
  await loadData();
})

const loadData = async () => {
  const currentBlessing = store.blessings.filter(x => x.blessingId == props.blessing?.id)[0];
  currentLevel.value = props.blessing.levels.filter(x => x.id == currentBlessing.blessingLevelId)[0];
  mappingId.value = currentBlessing.id;
  form.setValues(currentBlessing, currentLevel.value);
  let sectionType: XpSectionType = props.blessing.type.toLowerCase() == 'disadvantage' ? XpSectionTypes.disadvantage : XpSectionTypes.advantage;
  let xpInfo = experienceInfo.getExperienceInfoForSection(sectionType);
  let currentLevelXp = props.blessing.type.toLowerCase() == 'disadvantage' ? currentLevel.value.xpGain : currentLevel.value.xpCost ;
  availableXp.value = xpInfo.characterCreateMax - xpInfo.total + currentLevelXp;
}

const onSubmit = form.handleSubmit(async (values) => {
  const currentBlessing = store.blessings.filter(x => x.blessingId == props.blessing?.id)[0];
  await store.updateBlessing(values, route.params.id, currentBlessing.id);
});

function disableOption(level:BlessingLevel){
  if(props.blessing.type.toLowerCase() == 'disadvantage'){
    return level.xpGain > availableXp.value;
  }
  return level.xpCost > availableXp.value;
}

</script>

<template>

  <form @submit="onSubmit">

    <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
      <div>
        <h2 class="p-0 m-0">
          {{ props.blessing.name }}
        </h2>
      </div>
      <div class="p-0 m-2 d-inline-flex align-items-start align-items-center gap-2">
        <Button label="Delete" size="small" severity="danger" @click="popupService.deleteConfirmation($event, mappingId )" />
        <Button label="Update" size="small" type="submit" />
      </div>
    </div>

    <div v-html="props.blessing?.description"></div>
    <h3 class="d-flex justify-content-end">
      <span>Available Experience: {{availableXp}}</span>
    </h3>
    <div v-if="availableXp == 0">
      <Message severity="warn" class="mt-4">You do not have enough experience to modify this.</Message>
    </div>
    <div v-for="level in props.blessing.levels" :key="level.id" class="mt-3">
      <div class="d-flex flex-column flex-md-row align-self-center">
        <RadioButton v-model="form.blessingLevel.field" :inputId="level.id.toString()" :value="level" class="mr-4" :disabled="disableOption(level)"/>
        <label :for="level.id.toString()">{{ level.name }} – {{ level.description }}</label>
      </div>
    </div>
    
    <div class="mt-4">
      <FormTextAreaWrapper v-model="form.notes"/>
    </div>
  </form>
</template>

<style>
:deep(th.text-center .p-datatable-column-header-content) {
  justify-content: center;
}

</style>