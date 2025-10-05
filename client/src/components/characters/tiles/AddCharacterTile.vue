<script setup lang="ts">

import Button from "primevue/button";
import {useRouter} from "vue-router";

const router = useRouter();

async function redirectToAdd(){
  router.push('/characters/wizard')
}

</script>

<template>
  <div class="d-flex justify-content-end">
    <Button  data-cy="character-delete-button" label="Add Character" @click="redirectToAdd" />
  </div>
</template>
