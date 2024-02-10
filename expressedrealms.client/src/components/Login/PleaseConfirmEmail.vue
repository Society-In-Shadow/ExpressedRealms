<script setup lang="ts">

  import axios from "axios";
  import {userStore} from "@/stores/userStore";
  let userInfo = userStore();

  let sentConfirmationEmail = false;
  await axios.post("/auth/resendConfirmationEmail", { email: userInfo.userEmail })
      .then(() => { 
        sentConfirmationEmail = true;
      })
  
</script>

<template>
  <p>You have an unconfirmed email.  Please confirm your email at {{userInfo.userEmail}}.</p>
  <p>If you do not have a confirmation email, you can resend it by clicking the button below.</p>
  <p v-show="sentConfirmationEmail">You have successfully send an reset password email to above email.</p>
  <router-link to="ForgotPassword">
    <Button data-cy="forgot-password" label="Forgot Password?" class="w-100 mb-2" />
  </router-link>
</template>

<style scoped>

</style>