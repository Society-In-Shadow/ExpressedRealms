<script setup lang="ts">

import Button from "primevue/button";
import Card from "primevue/card";
</script>

<template>
  <Card class="mb-3 characterTile">
    <template #content>
      <div style="text-align: center;" class="align-self-center">
        <Button data-cy="character-delete-button" label="Add" type="submit" />
      </div>
    </template>
  </Card>
</template>

<style scoped>

</style>