
<template>
  <Toast position="top-center" />
  <ConfirmDialog group="dialog" />
  <ConfirmPopup group="popup" />
  <Message icon="pi pi-exclamation-triangle" class="sticky-top ms-0 me-0 mt-2 mb-2 m-md-2" severity="warn">
    This site is currently in a BETA state, not all features or content is in place.
  </Message>
  <main-nav-bar />
  <router-view />
</template>

<script setup>
import { RouterView } from 'vue-router'
import MainNavBar from "@/components/navbar/MainNavBar.vue";
import Toast from 'primevue/toast';
import ConfirmDialog from "primevue/confirmdialog";
import ConfirmPopup from 'primevue/confirmpopup';
import Message from 'primevue/message';
</script>

<style>
  #baseTile > div > div{
    padding-top: 0 !important;
    padding-bottom: 0 !important;
  }
</style>
