<script setup lang="ts">

import {onMounted, ref, watch} from 'vue';
import axios from "axios";
import PlayerTile from "@/components/players/Tiles/PlayerTile.vue";
import type {PlayerListItem} from "@/components/players/Objects/Player";
import InputText from "primevue/inputtext";

let players = ref<Array<PlayerListItem>>([]);
const filteredPlayers = ref<Array<PlayerListItem>>([]);
const searchQuery = ref<string>("");

function fetchData() {
  axios.get('/admin/users')
      .then((response) => {
        players.value = response.data.users;
        filteredPlayers.value = response.data.users
      });
}

onMounted(() =>{
  fetchData();
})

function filterPlayers(query: string) {
  const lowercasedQuery = query.toLowerCase().trim();

  if (!lowercasedQuery) {
    // Reset showing all players if the query is empty
    filteredPlayers.value = players.value;
  } else {
    // Filter players by username or email
    filteredPlayers.value = players.value.filter((player) =>
        player.username.toLowerCase().includes(lowercasedQuery) ||
        player.email.toLowerCase().includes(lowercasedQuery)
    );
  }
}

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
  filterPlayers(query);
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
    <div v-if="filteredPlayers.length === 0" class="m-3">
      No users with that name or email address
    </div>
    
    <div v-for="player in filteredPlayers" :key="player.id">
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
