<script setup lang="ts">

import { onMounted, ref } from 'vue';
import axios from "axios";
import PlayerTile from "@/components/players/Tiles/PlayerTile.vue";
import type {PlayerListItem} from "@/components/players/Objects/Player";

let players = ref<Array<PlayerListItem>>([]);

function fetchData() {
  axios.get('/admin/users')
      .then((json) => {
        players.value = json.data.users;
      });
}

onMounted(() =>{
  fetchData();
})

</script>

<template>
  <div v-for="player in players" class="container" v-bind:key="player.id">
    <PlayerTile :player-info="player" />
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
