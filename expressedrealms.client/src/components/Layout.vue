
<template>

  <main-nav-bar></main-nav-bar>
  
  <router-view />

</template>

<script setup>
import { RouterView } from 'vue-router'
import MainNavBar from "@/components/navbar/MainNavBar.vue";

</script>

<style scoped>

</style>