<script setup lang="ts">

import Button from "primevue/button";
import Card from "primevue/card";
import axios from "axios";

const emit = defineEmits<{
  delete: [id: number]
}>()

const props = defineProps({
  characterName: {
    type: String,
    required: true,
  },
  backgroundStory: {
    type: String
  },
  characterId: {
    type: Number,
    required: true
  }
});

async function deleteCharacter() {
  
  await axios.delete(`/api/characters/${props.characterId}`)
      .then(() => {
        emit('delete', props.characterId);
      });
}

</script>

<template>
  <Card class="mb-3 characterTile">
    <template #title>
      {{characterName}}
    </template>
    <template #content>
      <div>{{backgroundStory}}</div>
      <Button data-cy="character-edit-button" label="Edit" class="m-1" />
      <Button data-cy="character-delete-button" label="Delete" class="m-1" @click="deleteCharacter" />
    </template>
  </Card>
</template>

<style scoped>

</style>