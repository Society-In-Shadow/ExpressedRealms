<script setup lang="ts">

import {computed, onMounted, ref} from "vue";
import {blessingsStore} from "@/components/blessings/stores/blessingsStore";
import BlessingItem from "@/components/blessings/BlessingItem.vue";
import {UserRoles, userStore} from "@/stores/userStore.ts";
import AddBlessing from "@/components/blessings/AddBlessing.vue";
import Button from "primevue/button";

const store = blessingsStore();
const userInfo = userStore();


const props = defineProps({
  isReadOnly: {
    type: Boolean,
    required: true
  }
});

const showEdit = ref(false);
const showAdd = ref(false);

const toggleAdd = () =>{
  showAdd.value = !showAdd.value;
}

onMounted(async () => {
  await store.getBlessings()
  showEdit.value = userInfo.hasUserRole(UserRoles.BlessingsManagementRole);
})

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
  <div v-if="showEdit" class="text-right">
    <Button label="Add Advantage" @click="toggleAdd" />
    <AddBlessing v-if="showAdd" @canceled="toggleAdd" />
  </div>
  <div>
    <h1>Advantage</h1>
    <div v-for="blessing in sortedAdvantages" :key="blessing.id">
      <BlessingItem :blessing="blessing" :is-read-only="props.isReadOnly" />
    </div>
  </div>

  <div>
    <h1>Disadvantage</h1>
    <div v-for="blessing in sortedDisadvantages" :key="blessing.id">
      <BlessingItem :blessing="blessing" :is-read-only="props.isReadOnly" />
    </div>
  </div>

  <div>
    <h1>Mixed Blessings</h1>
    <div v-for="blessing in store.mixedBlessings" :key="blessing.id">
      <BlessingItem :blessing="blessing" :is-read-only="props.isReadOnly" />
    </div>
  </div>
</template>
