<script setup lang="ts">

import Card from 'primevue/card';
import Login from './Login.vue';
import CreateAccount from './CreateAccount.vue';
import ResetPassword from './ResetPassword.vue';

import { ref } from 'vue';

const currentAction = ref("Login");

function transitionScreen(screenName){
   currentAction.value = screenName;
}
</script>

<template>
  <Card style="border-radius: 1em; max-width:40em;">
    <template #content>
        <div class="row">
          <div class="col d-flex justify-content-center align-items-center">
              <img src="/public/favicon.png" alt="Image" height="200"/>
          </div>
          <div class="col-sm-6 col-xs-12">
            <slot></slot>
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