<script setup lang="ts">

import {onBeforeMount, ref} from "vue";
import {useRoute} from "vue-router";
import {
  characterBlessingsStore
} from "@/components/characters/character/wizard/blessings/stores/characterBlessingStore.ts";
import BlessingAccordion from "@/components/characters/character/blessings/BlessingAccordion.vue";
import type {CharacterBlessingTypes} from "@/components/characters/character/wizard/blessings/types.ts";

const characterBlessingData = characterBlessingsStore();
const route = useRoute();
const types = ref<Array<CharacterBlessingTypes>>([])

onBeforeMount(async () => {
  await characterBlessingData.getCharacterBlessings(route.params.id);

  const typeNames = [...new Set(characterBlessingData.blessings.map(i => i.type))];
  typeNames.forEach(type => {
    types.value.push({
      name: type,
      blessings: characterBlessingData.blessings.filter(i => i.type == type)
    })
  })
})


</script>

<template>
  <div style="max-width: 650px; margin: 0 auto;">
    <div v-for="type in types">
      <h1>{{type.name}}s</h1>
      <BlessingAccordion :blessings="type.blessings" />
    </div>
  </div>
</template>
