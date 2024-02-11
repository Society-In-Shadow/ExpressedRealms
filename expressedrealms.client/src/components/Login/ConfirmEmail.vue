<script setup lang="ts">

  import axios from "axios";
  import Router from "@/router";
  import {useRoute} from "vue-router";
  import {userStore} from "@/stores/userStore";
  let userInfo = userStore();

  const route = useRoute();

  axios.get('/api/auth/confirmEmail',{
        params: {
          userId: route.query.userId,
          code: route.query.code
        }
      }).then(() => {
        userInfo.hasConfirmedEmail = true;
        Router.push('login?confirmedEmail=1');
      });
  
</script>

<template>
  <p>Confirming your email, please wait... We will automatically redirect you.</p>
</template>

<style scoped>

</style>