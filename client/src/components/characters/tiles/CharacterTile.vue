<script setup lang="ts">

import Button from "primevue/button";
import Card from "primevue/card";
import {useRouter} from "vue-router";
import {overallCharacterConfirmationService} from "@/components/characters/services/confirmationService.ts";

const Router = useRouter();
const popupService = overallCharacterConfirmationService();


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
      <Button data-cy="character-delete-button" label="Delete" class="m-1" @click="popupService.deleteConfirmation($event, props.characterId)" />
    </template>
  </Card>
</template>

<style scoped>
  .characterTile >>> .p-card-content{
    padding: 0;
  }
</style>
