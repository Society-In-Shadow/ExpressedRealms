<script setup lang="ts">

import {computed, onMounted, ref} from "vue";
import {blessingsStore} from "@/components/blessings/stores/blessingsStore";
import {UserRoles, userStore} from "@/stores/userStore.ts";
import {makeIdSafe} from "@/utilities/stringUtilities.ts";
import SelectBlessingItem from "@/components/characters/character/wizard/blessings/supports/SelectBlessingItem.vue";
import {
  characterBlessingsStore
} from "@/components/characters/character/wizard/blessings/stores/characterBlessingStore.ts";
import {useRoute} from "vue-router";
import type {SubCategory} from "@/components/blessings/types.ts";

const route = useRoute();
const store = blessingsStore();
const characterBlessingData = characterBlessingsStore();
const userInfo = userStore();


const props = defineProps({
  isReadOnly: {
    type: Boolean,
    required: true
  }
});

const showEdit = ref(false);
const showAdd = ref(false);

onMounted(async () => {
  await store.getBlessings()
  showEdit.value = await userInfo.hasUserRole(UserRoles.BlessingsManagementRole);
  await characterBlessingData.getCharacterBlessings(route.params.id)
})

const filteredTypes = computed(() => {
  const selectedIds = characterBlessingData.selectedBlessingIds;
  
  return store.types.map(type => {
    
    const subCategories = type.subCategories.map(subCategory => {
      return {
        name: subCategory.name,
        blessings: subCategory.blessings.filter(x => !selectedIds.includes(x.id))
      } as SubCategory
    })
    
    return {
      ...type,
      subCategories: subCategories
    };
  })
  
})

</script>

<template>

  <div v-for="type in characterBlessingData.types">
    <h1>Selected {{type.name}}</h1>
    <div v-for="trait in type.subCategories">
      <h3 class="ml-3">{{trait.name}}</h3>
      <div v-for="blessing in trait.blessings">
        <h4 class="ml-5">{{blessing.name}}</h4>
      </div>
    </div>
  </div>

  <div v-for="type in filteredTypes" :key="type.name">
    <h1 :id="makeIdSafe(type.name)">{{ type.name }}</h1>
    <div v-for="subCategory in type.subCategories" :key="subCategory.name">
      <h2 class="pl-3 pb-3" :id="makeIdSafe(subCategory.name)">{{ subCategory.name }}</h2>
      <div v-for="blessing in subCategory.blessings" :key="blessing.id">
        <SelectBlessingItem :blessing="blessing" :is-read-only="props.isReadOnly" />
      </div>
    </div>
  </div>
</template>
