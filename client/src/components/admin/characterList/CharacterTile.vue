<script setup lang="ts">
import type {PropType} from "vue";
import {ref} from 'vue'
import Button from "primevue/button";
import Card from "primevue/card";
import type {PrimaryCharacter} from "@/components/admin/characterList/types.ts";
import {useRouter} from "vue-router";

const router = useRouter();
const showInfo = ref(false);

const props = defineProps({
  character: {
    type: Object as PropType<PrimaryCharacter>,
    required: true
  }
});

async function redirectToCharacterSheet(){
  await router.push({name: "characterSheet", params: {id: props.character.id}});
}

</script>

<template>
  <Card class="mb-3">
    <template #title>
      <div class="d-flex flex-row justify-content-between">
        <h2 class="m-0 p-0">{{ props.character?.name }} - <em>{{ props.character?.playerName }}</em></h2>
        <div class="flex flex-row">
          <Button :label="showInfo ? 'Cancel' : 'Quick Notes'" class="m-2" @click="showInfo = !showInfo" />
          <Button label="View Character Sheet" class="m-2" @click="redirectToCharacterSheet()" />
        </div>
      </div>
      <div>
        {{ props.character.expression }}
      </div>
    </template>
    <template #content>
      <div v-if="showInfo">
        <h3>Backstory</h3>
        {{ props.character.background ?? "No Background has been posted for this character." }}
      </div>
    </template>

  </Card>
</template>
