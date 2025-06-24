<script setup lang="ts">

import {makeIdSafe} from "@/utilities/stringUtilities";
import Skeleton from 'primevue/skeleton';
import {scrollToSection} from "@/components/expressions/expressionUtilities";
import {powerPathStore} from "@/components/expressions/powerPaths/stores/powerPathStore";

const powerPaths = powerPathStore();

const props = defineProps({
  showSkeleton:{
    type: Boolean,
    required: true
  }
});

</script>

<template>
  <div v-for="path in powerPaths.powerPaths">
    <Skeleton v-if="props.showSkeleton" id="toc-skeleton" class="mb-2" height="1.5em" />
    <a v-else class="p-1 tocItem" :href="'#' + makeIdSafe(path.name)" @click.prevent="scrollToSection(path.name)">{{ path.name }}</a>
    <div class="ps-4" v-for="power in path.powers">
      <Skeleton v-if="props.showSkeleton" id="toc-skeleton" class="mb-2" height="1.5em" />
      <a v-else class="p-1 tocItem" :href="'#' + makeIdSafe(power.name)" @click.prevent="scrollToSection(power.name)">{{ power.name }}</a>
    </div>
  </div>
</template>


<style scoped>

.tocItem{
  text-decoration: none;
  display: block;
  color: inherit;
}

.tocItem:hover {
  background: var(--p-form-field-disabled-background);
  cursor: pointer;
}

</style>
