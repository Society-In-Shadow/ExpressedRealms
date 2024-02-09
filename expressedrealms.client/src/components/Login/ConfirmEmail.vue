<script setup lang="ts">

  import axios from "axios";
  import Router from "@/router";
  import {useRoute} from "vue-router";

  const route = useRoute();
  let confirmStatus = "confirming";

  axios.post('/api/auth/confirmEmail',
      {
        userId: route.query.userId,
        code: route.query.code
      }).then(() => {
        confirmStatus = "confirmed"
        Router.push('login?confirmedEmail=1');
      })
      .catch(() => {
        confirmStatus = "error";
      });
  
</script>

<template>
  <p v-if="confirmStatus == 'confirming'">Confirming your email, please wait...</p>
  <p v-else-if="confirmStatus == 'confirmed'">Successfully confirmed your email!  Redirecting you to the login page!</p>
  <p v-else-if="confirmStatus == 'error'">There was an issue confirming your email, please try again.</p>
</template>

<style scoped>

</style>