<script setup lang="ts">

import {computed, onMounted} from "vue";
import {blessingsStore} from "@/components/blessings/stores/blessingsStore";
import BlessingItem from "@/components/blessings/BlessingItem.vue";

const store = blessingsStore();

onMounted(async () => {
  await store.getBlessings()
})

const props = defineProps({
  isReadOnly: {
    type: Boolean,
    required: true
  }
});

const sortedAdvantages = computed(() => {
  return [...store.advantages].sort((a, b) => {
    // First sort by subCategory
    const subCategoryA = a.subCategory?.toLowerCase() || '';
    const subCategoryB = b.subCategory?.toLowerCase() || '';
    const subCategoryComparison = subCategoryA.localeCompare(subCategoryB);

    // If subCategories are the same, sort by name
    if (subCategoryComparison === 0) {
      const nameA = a.name?.toLowerCase() || '';
      const nameB = b.name?.toLowerCase() || '';
      return nameA.localeCompare(nameB);
    }

    return subCategoryComparison;

  });
});

const sortedDisadvantages = computed(() => {
  return [...store.disadvantages].sort((a, b) => {
    // First sort by subCategory
    const subCategoryA = a.subCategory?.toLowerCase() || '';
    const subCategoryB = b.subCategory?.toLowerCase() || '';
    const subCategoryComparison = subCategoryA.localeCompare(subCategoryB);

    // If subCategories are the same, sort by name
    if (subCategoryComparison === 0) {
      const nameA = a.name?.toLowerCase() || '';
      const nameB = b.name?.toLowerCase() || '';
      return nameA.localeCompare(nameB);
    }

    return subCategoryComparison;
  });
});

</script>

<template>
  
  <div>
    <h1>Advantage</h1>
    <div v-for="blessing in sortedAdvantages" v-bind:key="blessing.id">
      <BlessingItem :blessing="blessing" :is-read-only="props.isReadOnly" />
    </div>
  </div>

  <div>
    <h1>Disadvantage</h1>
    <div v-for="blessing in sortedDisadvantages" v-bind:key="blessing.id">
      <BlessingItem :blessing="blessing" :is-read-only="props.isReadOnly" />
    </div>
  </div>

  <div>
    <h1>Mixed Blessings</h1>
    <div v-for="blessing in store.mixedBlessings" v-bind:key="blessing.id">
      <BlessingItem :blessing="blessing" :is-read-only="props.isReadOnly" />
    </div>
  </div>
  
</template>
