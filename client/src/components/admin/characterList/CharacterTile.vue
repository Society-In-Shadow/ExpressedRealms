<script setup lang="ts">
import type {PropType} from "vue";
import {ref} from 'vue'
import Button from "primevue/button";
import Card from "primevue/card";
import type {PrimaryCharacter} from "@/components/admin/characterList/types.ts";
import {useRouter} from "vue-router";
import {adminCharacterDialogs} from "@/components/admin/characterList/services/dialogs.ts";

const router = useRouter();
const showInfo = ref(false);
const dialogs = adminCharacterDialogs();

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
        <div>
          <h2 class="m-0 p-0">{{ props.character?.name }}</h2>
          <em class="small">{{ props.character?.playerName }}</em>
          <div>
            {{ props.character.expression }}
          </div>
        </div>
        <div>
          <Button :label="showInfo ? 'Cancel' : 'Quick Notes'" class="m-2" @click="showInfo = !showInfo" />
          <Button label="View Character Sheet" class="m-2" @click="redirectToCharacterSheet()" />
          <Button label="Update XP" class="m-2" @click="dialogs.showUpdateXp(props.character.id, props.character.assignedXp)" />
        </div>
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
