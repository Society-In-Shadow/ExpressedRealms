<script setup lang="ts">

import Card from 'primevue/card';
import Login from './Login.vue'

</script>

<template>
  <Card style="border-radius: 1em; max-width:40em;">
    <template #content>
        <div class="row">
          <div class="col d-flex justify-content-center align-items-center">
              <img src="/public/favicon.png" alt="Image" height="200"/>
          </div>
          <div class="col">
            <Login></Login>
          </div>
        </div>
    </template>
  </Card>
</template>

<style>

body {
  display: flex;
  place-items: center;
  background-color: dimgray;
}

</style>