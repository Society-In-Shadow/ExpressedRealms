<script setup lang="ts">

import Drawer from "primevue/drawer";
import Card from "primevue/card";
import {breakpointsBootstrapV5, useBreakpoints} from "@vueuse/core";
import {wizardContentStore} from "@/components/characters/character/wizard/stores/wizardContentStore.ts";

const wizardContentInfo = wizardContentStore();
const activeBreakpoint = useBreakpoints(breakpointsBootstrapV5);

const isMobile = activeBreakpoint.smaller('md');

</script>

<template>
  <Drawer v-model:visible="wizardContentInfo.showContent" header="Drawer" v-if="isMobile" position="full" >
    <component :is="wizardContentInfo.contentComponent.component" v-bind="wizardContentInfo.contentComponent.props"/>
  </Drawer>
  <Card v-else>
    <template #content>
      <div v-if="!wizardContentInfo.contentComponent">
        Choose an item to get started!
      </div>
      <component v-else :is="wizardContentInfo.contentComponent.component" v-bind="wizardContentInfo.contentComponent.props" />
    </template>
  </Card>
</template>

<style scoped>

</style>