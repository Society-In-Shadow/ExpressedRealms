
<template>
  <Toast position="top-center" />
  <ConfirmDialog group="dialog" />
  <ConfirmPopup group="popup" />
  <main-nav-bar />
  
  <router-view />
</template>

<script setup>
import { RouterView } from 'vue-router'
import MainNavBar from "@/components/navbar/MainNavBar.vue";
import Toast from 'primevue/toast';
import ConfirmDialog from "primevue/confirmdialog";
import ConfirmPopup from 'primevue/confirmpopup';
</script>

<style>
  #baseTile > div > div{
    padding-top: 0 !important;
    padding-bottom: 0 !important;
  }
</style>
