<script setup lang="ts">

import { RouterView } from 'vue-router'
import Card from 'primevue/card'
import PublicMenu from '@/components/public/PublicMenu.vue'

import DynamicDialog from 'primevue/dynamicdialog'
</script>

<template>
  <div class="d-flex flex-column min-vh-98">
    <DynamicDialog />
    <PublicMenu />
    <div class="container-fluid constrain-width flex-grow-1 mb-3 mt-3">
      <router-view />
    </div>
    <Card class="mt-auto m-0 p-0">
      <template #content>
        <div class="d-flex flex-column flex-md-row justify-content-between align-items-center">
          <div>© {{ new Date().getFullYear() }} Expressed Realms: Society in Shadows LARP. All rights reserved.</div>
          <div class="d-flex flex-column flex-md-row text-center">
            <RouterLink to="/terms-of-service" class="p-md-3">
              Terms of Service
            </RouterLink>
            <RouterLink to="/privacy-policy" class="p-md-3">
              Privacy Policy
            </RouterLink>
            <RouterLink to="/code-of-conduct" class="p-md-3">
              Code of Conduct
            </RouterLink>
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

 .min-vh-98 {
   min-height: 98vh;
 }

</style>
