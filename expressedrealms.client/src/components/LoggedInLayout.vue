
<template>
  <main-nav-bar />
  
  <Card class="m-3 p-0" id="baseTile">
    <template #content>
      <router-view />
    </template>
  </Card>
</template>

<script setup>
import { RouterView } from 'vue-router'
import MainNavBar from "@/components/navbar/MainNavBar.vue";
import Card from 'primevue/card';
</script>

<style>
  #baseTile > div > div{
    padding-top: 0 !important;
    padding-bottom: 0 !important;
  }
</style>