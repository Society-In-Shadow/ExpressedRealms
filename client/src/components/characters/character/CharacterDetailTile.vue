<script setup lang="ts">

import Card from "primevue/card";
import {onBeforeMount, ref} from "vue";
import {useRoute, useRouter} from 'vue-router'
import {characterStore} from "@/components/characters/character/stores/characterStore";
import Button from "primevue/button";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import {FeatureFlags, userStore} from "@/stores/userStore.ts";
import Tag from "primevue/tag";

const route = useRoute()
const router = useRouter();
const characterInfo = characterStore();
const experienceInfo = experienceStore();
const userInfo = userStore();

const showFactionInfo = ref(false);
onBeforeMount(async () =>{
  await characterInfo.getCharacterDetails(Number(route.params.id))
      .then(() => {
        name.value = characterInfo.name;
        expression.value = characterInfo.expression;
        faction.value = characterInfo.faction;
        isPrimaryCharacter.value = characterInfo.isPrimaryCharacter;
      });
  showFactionInfo.value = await userInfo.hasFeatureFlag(FeatureFlags.ShowFactionDropdown);
  await experienceInfo.updateExperience(route.params.id);
});

const name = ref("");
const faction = ref("");
const expression = ref("");
const isPrimaryCharacter = ref(false);

const redirectToEdit = () => {
  router.push({name: 'characterWizard', params: {id: route.params.id}})
}

</script>

<template>
  <Card class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch">
    <template #content>
      <div class="d-flex flex-row justify-content-between" >
        <div>
          <div class="d-flex flex-row align-items-center gap-3">
            <div><h1 class="mt-0 pt-0 pb-0 mb-0">{{ name }}</h1></div>
            <div><Tag v-if="isPrimaryCharacter" value="Primary" severity="info" /></div>
          </div>
          <div v-if="!experienceInfo.isLoading"><em><span>XL: {{experienceInfo.getCharacterLevel()}}</span> - {{ expression }}</em></div>
          <div v-if="showFactionInfo"><em>{{ faction?.name ?? 'No Faction' }}</em></div>
        </div>
        <div class="d-flex flex-column gap-3" style="font-size: 2.5em">
          <Button class="float-end" label="Edit" @click="redirectToEdit" />
        </div>
      </div>
    </template>
  </Card>
</template>
