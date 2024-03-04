<script setup lang="ts">

import ChangeEmail from "@/components/profile/ChangeEmail.vue";
import ChangePassword from "@/components/profile/ChangePassword.vue";

</script>

<template>

  <div class="flex flex-xs-column flex-sm-column flex-lg-row flex-md-row gap-3 m-3 ">
    <ChangeEmail></ChangeEmail>
    <ChangePassword></ChangePassword>
  </div>

</template>

<style scoped>

@media (max-width: 576px) {
  .flex-xs-column {
    flex-direction: column !important;
  }
}

</style>