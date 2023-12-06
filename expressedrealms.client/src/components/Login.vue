<script setup lang="ts">

import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import Card from 'primevue/card';

import { ref } from 'vue'

const email = ref("");
const password = ref("");

</script>

<template>
  <Card style="border-radius: 1em; max-width:40em;">
    <template #content>
        <div class="row">
          <div class="col d-flex justify-content-center align-items-center">
              <img src="/public/favicon.png" alt="Image" height="200"/>
          </div>
          <div class="col">
            <div class="row">
              <div class="col">
                <div>
                  <label for="email1" class="block text-900 font-medium mb-2">Email</label>
                  <InputText id="email1" type="text" v-model="email" class="w-100 mb-3" />
                </div>
                <div>
                  <label for="password1" class="block text-900 font-medium mb-2">Password</label>
                  <InputText id="password1" type="password" v-model="password" class="w-100 mb-3" />
                </div>
              </div>
            </div>
            <div class="row">
              <div class="col">
                <Button label="Sign In" class="w-100 mb-2"></Button>
                <Button label="Create Account" class="w-100 mb-2"></Button>
                <Button label="Forgot Password?" class="w-100"></Button>
              </div>
            </div>
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