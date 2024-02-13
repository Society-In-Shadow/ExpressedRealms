<script setup lang="ts">

  import Button from 'primevue/button';
  import axios from "axios";
  import { logOff } from "@/services/Authentication";
  import {userStore} from "@/stores/userStore";
  import { ref } from 'vue'
  let userInfo = userStore();

  let sentConfirmationEmail = ref(false);
  async function resendConfirmationEmail() {
    await axios.post("/api/auth/resendConfirmationEmail", { email: userInfo.userEmail })
        .then(() => {
          sentConfirmationEmail.value = true;
        });
  }
</script>

<template>
  <p>You have an unconfirmed email.  Please confirm your email at</p>
  <p>{{userInfo.userEmail}}</p>
  <p>If you do not have a confirmation email, you can resend it by clicking the button below.</p>
  <p v-show="sentConfirmationEmail">You have successfully send an reset password email to above email.</p>
  <Button data-cy="forgot-password" label="Resend Confirmation Link" @click="resendConfirmationEmail()" class="w-100 mb-2" />
  <p>Alternatively, you can log off and try another user.</p>
  <Button data-cy="logoff-button" label="Logoff" @click="logOff" class="w-100 mb-2" />
</template>

<style scoped>

</style>