<script setup lang="ts">

import ChangeEmail from "@/components/profile/ChangeEmail.vue";
import ChangePassword from "@/components/profile/ChangePassword.vue";

</script>

<template>
  <div class="flex flex-xs-column flex-sm-column flex-lg-row flex-md-row gap-3 m-3 ">
    <ChangeEmail class="align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" />
    <ChangePassword />
  </div>
</template>

<style scoped>

@media (max-width: 576px) {
  .flex-xs-column {
    flex-direction: column !important;
  }
}

</style>