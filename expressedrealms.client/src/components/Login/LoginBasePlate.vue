<script setup lang="ts">

import Card from 'primevue/card';

</script>

<template>
  <Card id="loginBox" class="centered">
    <template #content>
      <div class="row">
        <div class="col-sm-6 col-xs-12 d-flex justify-content-center align-items-center">
          <img src="/public/favicon.png" alt="Image" height="200">
        </div>
        <div class="col-sm-6 col-xs-12">
          <router-view />
        </div>
      </div>
    </template>
  </Card>
</template>

<style scoped>

#loginBox {
  border-radius: 1em; 
}

.centered {
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}

</style>