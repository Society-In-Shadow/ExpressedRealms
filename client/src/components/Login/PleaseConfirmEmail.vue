<script setup lang="ts">

  import Button from 'primevue/button';
  import axios from "axios";
  import { logOff } from "@/services/Authentication";
  import {userStore} from "@/stores/userStore";
  import { ref } from 'vue'
  let userInfo = userStore();

  let sentConfirmationEmail = ref(false);
  async function resendConfirmationEmail() {
    await axios.post("/auth/resendConfirmationEmail", { email: userInfo.userEmail })
        .then(() => {
          sentConfirmationEmail.value = true;
        });
  }
</script>

<template>
  <p>We've detected that you have not confirmed your email associated with this account.</p>
  <p>Please do so by clicking the link in your confirmation email. If you do not have an email, you can resend it by clicking the button below.</p>
  <p v-show="sentConfirmationEmail">
    You have successfully send an reset password email.
  </p>
  <Button data-cy="forgot-password" label="Resend Confirmation Link" class="w-100 mb-2" @click="resendConfirmationEmail()" />
  <p>Alternatively, you can log off and try another user.</p>
  <Button data-cy="logoff-button" label="Logoff" class="w-100 mb-2" @click="logOff" />
</template>
