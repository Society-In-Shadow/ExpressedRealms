<template>
  <div class="flex flex-wrap justify-content-center m-3 column-gap-3">
    <CharacterTile
      v-for="character in characters" 
      :key="character.id"
      :character-id="character.id" 
      :character-name="character.name" 
      :background-story="character.background"
      :expression="character.expression"
      @delete="deleteCharacter"
    />
    <AddCharacterTile />
  </div>
</template>

<script setup lang="ts">
    import { onMounted, ref } from 'vue';
    import axios from "axios";
    import CharacterTile from "@/components/characters/tiles/CharacterTile.vue";
    import AddCharacterTile from "@/components/characters/tiles/AddCharacterTile.vue";

    let characters = ref([]);
    
    function fetchData() {
      axios.get('/api/characters')
          .then((json) => {
            characters.value = json.data;
          });
    }
    
    function deleteCharacter(id){
      var characterIndex = characters.value.map(i => i.id).indexOf(id)
      ~characterIndex && characters.value.splice(characterIndex, 1);
    }
    
    onMounted(() =>{
      fetchData();
    })
</script>

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
