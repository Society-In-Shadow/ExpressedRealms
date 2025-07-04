<script setup lang="ts">

import {powersStore} from "@/components/expressions/powers/stores/powersStore";
import {computed, onBeforeMount, ref} from "vue";
import type {
  RawPowerPrerequisite,
  PowerPrerequisiteOptions
} from "@/components/expressions/powers/types";
import FormMultiSelectWrapper from "@/FormWrappers/FormMultiSelectWrapper.vue";
import FormDropdownWrapper from "@/FormWrappers/FormDropdownWrapper.vue";
import {getValidationInstance} from "@/components/expressions/powers/Validations/PrerequisiteValidations";
import Button from "primevue/button";
import axios from "axios";
import toaster from "@/services/Toasters";
import type {ListItem} from "@/types/ListItem";
import {
  powerPrerequisiteConfirmationPopups
} from "@/components/expressions/powers/services/powerPrerequisiteConfirmationPopupService";

let powers = powersStore();
let form = getValidationInstance();

const props = defineProps({
  powerId: {
    type: Number,
  },
  powerPathId:{
    type: Number,
    required: true
  }
});

let availablePowers = ref<PowerPrerequisiteOptions>({});
let prerequisite = ref<RawPowerPrerequisite | null>(null);
let popups = powerPrerequisiteConfirmationPopups(props.powerPathId);

const showPrerequisite = ref(false);
const hasPrerequisite = computed(() => {
  return prerequisite.value != null;
});
function togglePrerequisite(){
  showPrerequisite.value = !showPrerequisite.value;
}

onBeforeMount(async () =>  {
  let prerequisiteOptions = await powers.getPrerequisitePowerOptions(props.powerPathId);

  prerequisiteOptions.prerequisitePowers = prerequisiteOptions.prerequisitePowers.filter((x: ListItem) => x.id != props.powerId);
  
  availablePowers.value = prerequisiteOptions;
  await loadPrerequisite();
})

async function loadPrerequisite(){
  if(props.powerId == null) return;
  let value = await powers.getPrerequisitePowers(props.powerId);
  
  if(!value) return;
  
  prerequisite.value = value;
  form.setValues({
    requiredAmount: availablePowers.value.requiredAmount.filter((x: ListItem) => prerequisite.value.requiredAmount == x.id)[0] as ListItem,
    powers: availablePowers.value.prerequisitePowers.filter((x: ListItem) => prerequisite.value.powerIds.includes(x.id)) as ListItem[]
  })
}

const addUpdatePrerequisite = async function(powerId: number){
  if(hasPrerequisite.value){
    await onEdit();
    return;
  }
  await onAdd(powerId);
}

defineExpose({
  addUpdatePrerequisite
})

const onAdd = async function (powerId: number) {
  const submitHandler = form.handleSubmit(async (values) => {
    await axios.post(`/powers/${powerId}/prerequisite`, {
      requiredAmount: values.requiredAmount.id,
      powerIds: values.powers.map((item: { id: string | number }) => item.id)
    })
        .then(async () => {
          await loadPrerequisite();
          await powers.updatePowersByPathId(props.powerPathId);
          toaster.success("Successfully Added Prerequisite!");
        });
  })
  
  await submitHandler();
}

const onEdit = form.handleSubmit(async (values) => {
  if(!prerequisite.value) return;
  
  await axios.put(`/powers/${props.powerId}/prerequisite/${prerequisite.value.id}`, {
    id: prerequisite.value.id,
    requiredAmount: values.requiredAmount.id,
    powerIds: values.powers.map((item: { id: string | number }) => item.id)
  })
      .then(async () => {
        await powers.updatePowersByPathId(props.powerPathId);
        toaster.success("Successfully Updated Prerequisite!");
      });
});

async function deletePrerequisite(event: MouseEvent){
  await popups.deleteConfirmation(event, props.powerId, prerequisite.value.id);
  prerequisite.value = null;
  form.customResetForm();
}

</script>

<template>
  <div v-if="showPrerequisite || hasPrerequisite" class="row">
    <div class="col">
      <FormDropdownWrapper
        v-model="form.requiredAmount"
        :options="availablePowers.requiredAmount"
        option-label="name"
      />
    </div>
    <div class="col">
      <FormMultiSelectWrapper
        v-model="form.powers"
        :options="availablePowers.prerequisitePowers"
        option-label="name"
      />
    </div>
  </div>
  <div v-if="hasPrerequisite">
    <Button label="Remove Prerequisite" severity="danger" class="mr-2" @click="deletePrerequisite($event)" />
  </div>
  <div v-else>
    <Button v-if="!showPrerequisite" label="Add Prerequisite" @click="togglePrerequisite" />
    <Button v-if="showPrerequisite" label="Cancel Prerequisite" class="ml-2" @click="togglePrerequisite" />
  </div>
</template>
