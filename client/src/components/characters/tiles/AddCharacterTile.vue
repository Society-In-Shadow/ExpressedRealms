<script setup lang="ts">

import Button from "primevue/button";
import Card from "primevue/card";
import {useRouter} from "vue-router";

const router = useRouter();

async function redirectToAdd(){
  router.push('/characters/wizard')
}

</script>

<template>
  <Card class="mb-3 characterTile">
    <template #content>
      <div style="text-align: center;" class="align-self-center">
        <Button data-cy="character-delete-button" label="Add Character" @click="redirectToAdd" />
      </div>
    </template>
  </Card>
</template>
