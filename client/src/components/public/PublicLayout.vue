<script setup lang="ts">

import Message from 'primevue/message';
import {RouterView} from "vue-router";
import Menu from "@/components/public/Menu.vue";

</script>

<template>
  <Message icon="pi pi-exclamation-triangle" class="sticky-top ms-0 me-0 mt-2 mb-2 m-md-2" severity="warn">
    This site is currently in a BETA state, not all features or content is in place.
  </Message>
  <Menu />
  <div class="container-fluid constrain-width ">
    <router-view />
  </div>

</template>

<style scoped>

 .constrain-width {
   max-width: 1000px;
 }

</style>
