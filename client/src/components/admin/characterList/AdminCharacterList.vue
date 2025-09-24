<script setup lang="ts">

import {onMounted, ref, watch} from 'vue';
import InputText from "primevue/inputtext";
import {adminCharacterListStore} from "@/components/admin/characterList/stores/characterListStore.ts";
import CharacterTile from "@/components/admin/characterList/CharacterTile.vue";

const characterListInfo = adminCharacterListStore();

const searchQuery = ref<string>("");

onMounted(async () =>{
  await characterListInfo.fetchCharacters();
})

// Debounce function
function debounce(fn: Function, delay: number) {
  let timeout: number | undefined;
  return (...args: any[]) => {
    clearTimeout(timeout);
    timeout = setTimeout(() => {
      fn(...args);
    }, delay);
  };
}

// Debounced filter function
const debouncedFilterPlayers = debounce((query: string) => {
  characterListInfo.filterCharacters(query);
}, 250);

// Watch for changes to the search query and trigger the debounced filter function
watch(searchQuery, (newQuery) => {
  debouncedFilterPlayers(newQuery);
});

</script>

<template>
  <div class="container">
    <div class="row">
      <div class="col">
        <h1 class="m-3">
          Characters
        </h1>
      </div>
      <div class="col">
        <InputText
          v-model="searchQuery"
          placeholder="Search..."
          class="float-end m-3"
        />
      </div>
    </div>
    <div v-if="characterListInfo.filteredCharacters.length === 0" class="m-3">
      No characters with that name or email address
    </div>
    <div v-for="character in characterListInfo.filteredCharacters" :key="character.id">
      <CharacterTile :character="character" />
    </div>
  </div>
</template>

<style scoped>
  .container {
    width: 100%;
    margin-right: auto;
    margin-left: auto;
    max-width:1000px
  }
</style>
