<script setup lang="ts">

import Card from "primevue/card";
import Popover from "primevue/popover";
import {onBeforeMount, ref} from "vue";
import {useRoute, useRouter} from 'vue-router'
import {characterStore} from "@/components/characters/character/stores/characterStore";
import Button from "primevue/button";
import EditCharacterDetails from "@/components/characters/character/EditCharacterDetails.vue";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import OverallExperience from "@/components/characters/character/OverallExperience.vue";
import {characterPopupDialogs} from "@/components/characters/character/services/dialogs.ts";
import {FeatureFlags, userStore} from "@/stores/userStore.ts";

const route = useRoute()
const router = useRouter();
const characterInfo = characterStore();
const experienceInfo = experienceStore();
const dialog = characterPopupDialogs()
const userInfo = userStore();

const showFactionInfo = ref(false);
onBeforeMount(async () =>{
  await characterInfo.getCharacterDetails(Number(route.params.id))
      .then(() => {
        name.value = characterInfo.name;
        expression.value = characterInfo.expression;
        faction.value = characterInfo.faction;
      });
  showFactionInfo.value = await userInfo.hasFeatureFlag(FeatureFlags.ShowFactionDropdown);
  showWizardButton.value = await userInfo.hasFeatureFlag(FeatureFlags.ShowCharacterWizard);
  await experienceInfo.updateExperience(route.params.id);
});


const showWizardButton = ref(false);

const name = ref("");
const faction = ref("");
const expression = ref("");
const showEdit = ref(false);

function toggleEdit() {
  showEdit.value = !showEdit.value;
}


const op = ref();

const togglePopup = (event) => {
  op.value.toggle(event);
}

const redirectToEdit = () => {
  router.push({name: 'characterWizard', params: {id: route.params.id}})
}

</script>

<template>
  <Card v-if="!showEdit" class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch">
    <template #content>
      <div class="d-flex flex-row justify-content-between" >
        <div>
          <h1 class="mt-0 pt-0 pb-0 mb-0">{{ name }}</h1>
          <div v-if="!experienceInfo.isLoading"><em><span>XL: {{experienceInfo.getCharacterLevel()}}</span> - {{ expression }}</em></div>
          <div v-if="showFactionInfo"><em>{{ faction?.name ?? 'No Faction' }}</em></div>
        </div>
        <div class="d-flex flex-column gap-3" style="font-size: 2.5em">
          <Button v-if="!showWizardButton" type="button" @click="dialog.showExperience()" icon="pi pi-info-circle" icon-pos="right" size="large" :label="`XP: ${experienceInfo.experienceBreakdown.total - experienceInfo.experienceBreakdown.setupTotal}`" />
          <Button v-if="!showWizardButton" class="float-end" label="Edit" @click="toggleEdit" />
          <Button v-if="showWizardButton" class="float-end" label="Edit" @click="redirectToEdit" />
        </div>
      </div>
    </template>
  </Card>
  <EditCharacterDetails v-else @close-dialog="toggleEdit" />
  <Popover ref="op" :dismissable="true" >
    <OverallExperience/>
  </Popover>
</template>
