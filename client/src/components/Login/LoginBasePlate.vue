<script setup lang="ts">

import Card from 'primevue/card'
</script>

<template>
  <div class="d-flex justify-content-center align-items-center boxCenterHelper">
    <Card id="loginBox">
      <template #content>
        <div class="row">
          <div class="col-sm-6 col-xs-12 d-flex justify-content-center align-items-center">
            <img src="/favicon.png" alt="A white, black, blue, red, green, and transparent marbles organized in a pentagon pattern. The white stone is at the top and the transparent stone is in the center." height="200">
          </div>
          <div class="col-sm-6 col-xs-12">
            <router-view />
          </div>
        </div>
      </template>
    </Card>
  </div>
</template>

<style scoped>

#loginBox {
  border-radius: 1em;
}

@media (min-width: 540px) and (min-height: 450px) {
  .boxCenterHelper{
    height: 98vh;
  }
}
</style>
