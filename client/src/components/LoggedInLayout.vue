<template>
  <Toast position="top-center" />
  <ConfirmDialog group="dialog" />
  <ConfirmPopup group="popup" />
  <DynamicDialog />
  <main-nav-bar class="d-print-none" />
  <div class="main-container">
    <router-view />
  </div>
</template>

<script setup>
import { RouterView } from 'vue-router'
import MainNavBar from '@/components/navbar/MainNavBar.vue'
import Toast from 'primevue/toast'
import ConfirmDialog from 'primevue/confirmdialog'
import ConfirmPopup from 'primevue/confirmpopup'
import DynamicDialog from 'primevue/dynamicdialog'
</script>

<style>
  #baseTile > div > div{
    padding-top: 0 !important;
    padding-bottom: 0 !important;
  }

  .main-container{
    margin: 0 auto;
    max-width: 1300px;
  }

</style>
