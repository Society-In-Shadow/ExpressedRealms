<script setup lang="ts">

import { makeIdSafe } from '@/utilities/stringUtilities'
import Skeleton from 'primevue/skeleton'
import { scrollToSection } from '@/components/expressions/expressionUtilities'
import { blessingsStore } from '@/components/blessings/stores/blessingsStore.ts'

const store = blessingsStore()

const props = defineProps({
  showSkeleton: {
    type: Boolean,
    required: true,
  },
})

</script>

<template>
  <div v-for="type in store.types" :key="type.name">
    <Skeleton v-if="props.showSkeleton" id="toc-skeleton" class="mb-2" height="1.5em" />
    <a v-else class="p-1 tocItem" :href="'#' + makeIdSafe(type.name)" @click.prevent="scrollToSection(type.name)">{{ type.name }}</a>
    <div v-for="path in type.subCategories" :key="path.name" class="ps-4">
      <Skeleton v-if="props.showSkeleton" id="sub-category-skeleton" class="mb-2" height="1.5em" />
      <a v-else class="p-1 tocItem" :href="'#' + makeIdSafe(path.name)" @click.prevent="scrollToSection(path.name)">{{ path.name }}</a>
      <div v-for="power in path.blessings" :key="power.id" class="ps-4">
        <Skeleton v-if="props.showSkeleton" id="blessings-skeleton" class="mb-2" height="1.5em" />
        <a v-else class="p-1 tocItem" :href="'#' + makeIdSafe(power.name)" @click.prevent="scrollToSection(power.name)">{{ power.name }}</a>
      </div>
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
