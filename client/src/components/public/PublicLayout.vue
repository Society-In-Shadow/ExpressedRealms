<script setup lang="ts">

import Message from 'primevue/message';
import {RouterView} from "vue-router";
import Menu from "@/components/public/Menu.vue";
import Card from 'primevue/card';

</script>

<template>
  <div class="d-flex flex-column min-vh-100">
    <Message icon="pi pi-exclamation-triangle" class="sticky-top ms-0 me-0 mt-2 mb-2 m-md-2" severity="warn">
      This site is currently in a BETA state, not all features or content is in place.
    </Message>
    <Menu />
    <div class="container-fluid constrain-width flex-grow-1">
      <router-view />
    </div>
    <Card class="mt-auto">
      <template #content>
        <div class="d-flex justify-content-between align-items-center">
          <div>Society in Shadows © {{new Date().getFullYear()}}</div>
          <div>
            <RouterLink to="/terms-of-service" class="p-3">Terms of Service</RouterLink>
            <RouterLink to="/privacy-policy" class="p-3">Privacy Policy</RouterLink>
            <RouterLink to="/code-of-conduct" class="p-3">Code of Conduct</RouterLink>
          </div>
        </div>
      </template>
    </Card>
  </div>
</template>

<style scoped>

 .constrain-width {
   max-width: 1000px;
 }

</style>
