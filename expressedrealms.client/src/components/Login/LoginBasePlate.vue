<script setup lang="ts">

import Card from 'primevue/card';

</script>

<template>
  <Card style="border-radius: 1em; max-width:40em;">
    <template #content>
        <div class="row">
          <div class="col d-flex justify-content-center align-items-center">
              <img src="/public/favicon.png" alt="Image" height="200"/>
          </div>
          <div class="col-sm-6 col-xs-12">
            <router-view />
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