<script setup lang="ts">

import Button from "primevue/button";
import Card from "primevue/card";
import axios from "axios";
import Router from "@/router";

const emit = defineEmits<{
  delete: [id: number]
}>()

const props = defineProps({
  characterName: {
    type: String,
    required: true,
  },
  backgroundStory: {
    type: String,
    default: ''
  },
  characterId: {
    type: Number,
    required: true
  },
  expression: {
    type: String,
    required: true
  }
});

async function deleteCharacter() {
  await axios.delete(`/api/characters/${props.characterId}`)
      .then(() => {
        emit('delete', props.characterId);
      });
}

function editCharacter() {
  Router.push(`/characters/${props.characterId}`)
}

</script>

<template>
  <Card class="mb-3 characterTile">
    <template #title>
      {{ characterName }}
    </template>
    <template #content>
      <em class="mb-3">{{ expression }}</em>
      <div class="mt-3 text-sm">
        {{ backgroundStory }}
      </div>
      <Button data-cy="character-edit-button" label="Edit" class="m-1" @click="editCharacter" />
      <Button data-cy="character-delete-button" label="Delete" class="m-1" @click="deleteCharacter" />
    </template>
  </Card>
</template>

<style scoped>
  .characterTile >>> .p-card-content{
    padding: 0;
  }
</style>