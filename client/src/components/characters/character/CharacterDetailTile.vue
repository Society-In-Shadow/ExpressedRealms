<script setup lang="ts">

import Card from "primevue/card";
import {onMounted, ref} from "vue";
import { useRoute } from 'vue-router'
const route = useRoute()
import {characterStore} from "@/components/characters/character/stores/characterStore";
import Button from "primevue/button";
import EditCharacterDetails from "@/components/characters/character/EditCharacterDetails.vue";
const characterInfo = characterStore();

onMounted(async () =>{
  await characterInfo.getCharacterDetails(Number(route.params.id))
      .then(() => {
        name.value = characterInfo.name;
        expression.value = characterInfo.expression;
        faction.value = characterInfo.faction;
      });
});

const name = ref("");
const faction = ref("");
const expression = ref("");
const showEdit = ref(false);

function toggleEdit() {
  showEdit.value = !showEdit.value;
}

</script>

<template>
  <Card v-if="!showEdit" class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" style="max-width: 30em">
    <template #content>
        <h1 class="mt-0 pt-0">{{name}}</h1>
        <div>{{expression}}</div>
        <div>{{faction?.name ?? 'No Faction'}}</div>
        <Button class="float-end" label="Edit" @click="toggleEdit" />
    </template>
  </Card>
  <EditCharacterDetails v-else @close-dialog="toggleEdit" />
</template>
