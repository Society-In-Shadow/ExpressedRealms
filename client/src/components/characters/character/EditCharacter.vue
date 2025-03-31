<script setup lang="ts">

import Card from "primevue/card";
import { ref } from "vue";
import SmallStatDisplay from "@/components/characters/character/SmallStatDisplay.vue";
import Breadcrumb from 'primevue/breadcrumb';
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";
import SkillTile from "@/components/characters/character/skills/SkillTile.vue";
import EditCharacterDetails from "@/components/characters/character/EditCharacterDetails.vue";

import {characterStore} from "@/components/characters/character/stores/characterStore";
const characterInfo = characterStore();

const items = ref([
  { label: characterInfo.name },
]);
const home = ref({
  icon: 'pi pi-home',
  route: '/characters'
});

</script>

<template>
  <SkeletonWrapper :show-skeleton="characterInfo.isLoading" width="1em" height="1em">
    <Breadcrumb :home="home" :model="items" class="m-3">
      <template #item="{ item, props }">
        <router-link v-if="item.route" v-slot="{ href, navigate }" :to="item.route" custom>
          <a :href="href" v-bind="props.action" @click="navigate">
            <span class="pi pi-home text-color" />
            <span class="text-primary font-semibold">{{ item.label }}</span>
          </a>
        </router-link>
        <a v-else :href="item.url" :target="item.target" v-bind="props.action">
          <span class="text-color">{{ characterInfo.name }}</span>
        </a>
      </template>
    </Breadcrumb>
  </SkeletonWrapper>
  <div class="flex flex-xs-column flex-sm-column flex-lg-row flex-md-row gap-3 m-1 m-sm-3 m-md-3 m-lg-3 m-xl-3 flex-wrap">
    <EditCharacterDetails />
    <Card class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch">
      <template #content>
        <SmallStatDisplay />
      </template>
    </Card>

    <SkillTile />
  </div>
</template>

<style scoped>
@media (max-width: 576px) {
  .flex-xs-column {
    flex-direction: column !important;
  }
}
</style>
