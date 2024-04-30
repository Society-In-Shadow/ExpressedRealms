<script setup lang="ts">

import ChangeEmail from "@/components/profile/ChangeEmail.vue";
import ChangePassword from "@/components/profile/ChangePassword.vue";
import EditUserProfile from "@/components/profile/EditUserProfile.vue";

</script>

<template>
  <div class="flex flex-xs-column flex-sm-column flex-lg-row flex-md-row gap-3 m-3 ">
    <EditUserProfile class="align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" />
    <ChangeEmail class="align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" />
    <ChangePassword class="align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" />
  </div>
</template>

<style scoped>

@media (max-width: 576px) {
  .flex-xs-column {
    flex-direction: column !important;
  }
}

</style>
