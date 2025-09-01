
<script setup lang="ts">
import {onMounted} from 'vue';
import CharacterTile from "@/components/characters/tiles/CharacterTile.vue";
import AddCharacterTile from "@/components/characters/tiles/AddCharacterTile.vue";
import {charactersStore} from "@/components/characters/stores/charactersStore.ts";

const charactersData = charactersStore();

onMounted(async () => {
  await charactersData.getCharacters();
})
</script>

<template>
  <div class="flex flex-wrap justify-content-center m-3 column-gap-3">
    <CharacterTile
      v-for="character in charactersData.characters" 
      :key="character.id"
      :character-id="Number(character.id)" 
      :character-name="character.name" 
      :background-story="character.background"
      :expression="character.expression"
    />
    <AddCharacterTile />
  </div>
</template>

<style scoped>

.characterTile {
  width: 15rem;
}

@media(max-width: 576px){
  .characterTile {
    width: 100%
  }
}

</style>
