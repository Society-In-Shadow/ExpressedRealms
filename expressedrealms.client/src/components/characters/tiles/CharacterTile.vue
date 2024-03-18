<script setup lang="ts">

import Button from "primevue/button";
import Card from "primevue/card";

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
</script>

<template>
  <Card class="mb-3 characterTile">
    <template #title>
      {{characterName}}
    </template>
    <template #content>
      <div>{{backgroundStory}}</div>
      <Button data-cy="character-edit-button" label="Edit" class="m-1" type="submit" />
      <Button data-cy="character-delete-button" label="Delete" class="m-1" type="submit" />
    </template>
  </Card>
</template>

<style scoped>

</style>