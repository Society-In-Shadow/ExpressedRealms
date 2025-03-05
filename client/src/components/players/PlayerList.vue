<script setup lang="ts">

import {onMounted, ref, watch} from 'vue';
import PlayerTile from "@/components/players/Tiles/PlayerTile.vue";
import InputText from "primevue/inputtext";
import {playerList} from "@/components/players/Stores/PlayerListStore";
const playerListStore = playerList();

const searchQuery = ref<string>("");

onMounted(async () =>{
  await playerListStore.fetchPlayers();
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
  playerListStore.filterPlayers(query);
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
          Players
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
    <div v-if="playerListStore.filteredPlayers.length === 0" class="m-3">
      No users with that name or email address
    </div>
    
    <div v-for="player in playerListStore.filteredPlayers" :key="player.id">
      <PlayerTile :player-info="player" />
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
