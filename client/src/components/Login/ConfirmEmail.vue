<script setup lang="ts">

  import Button from 'primevue/button';
  import axios from "axios";
  import Router from "@/router";
  import {useRoute} from "vue-router";
  import { ref } from 'vue'
  import {userStore} from "@/stores/userStore";
  import { isLoggedIn, logOff } from "@/services/Authentication";

  let userInfo = userStore();

  const route = useRoute();

  let userIsLoggedIn = isLoggedIn();
  let hasError = ref(false);
  axios.get('/api/auth/confirmEmail',{
    params: {
      userId: route.query.userId,
      code: route.query.code,
      changedEmail: route.query.changedEmail
    }
  }).then(() => {
    userInfo.hasConfirmedEmail = true;
    if(route.query.changedEmail)
      userInfo.userEmail = route.query.changedEmail;
    Router.push('login?confirmedEmail=1');
  }).catch(() => {
    hasError.value = true;
  });
  
  let sentConfirmationEmail = ref(false);
  async function resendConfirmationEmail() {
    await axios.post("/api/auth/resendConfirmationEmail", { email: userInfo.userEmail })
        .then(() => {
          sentConfirmationEmail.value = true;
        });
  }
  
</script>

<template>
  <p v-if="!hasError">
    Confirming your email, please wait... We will automatically redirect you.
  </p>
  <div v-else>
    <div v-if="!userIsLoggedIn">
      <p>An error occured while confirming your email. Please try clicking the link in the email again, or try logging in to resend it.</p>
      <Button data-cy="back-button" label="Back To Login" class="w-100 mb-2" @click="Router.push('/login')" />
    </div>
    <div v-else>
      <p>An error occured while confirming your email. Please try clicking the link in the email again, or pressing the button below to send a new one.</p>
      <p v-show="sentConfirmationEmail" data-cy="resend-confirmation-message">
        You have successfully sent an email confirmation email to your email.
      </p>
      <Button data-cy="resend-confirmation-button" label="Resend Confirmation Link" class="w-100 mb-2" @click="resendConfirmationEmail()" />
      <p>Alternatively, you can log off and try another user.</p>
      <Button data-cy="logoff-button" label="Logoff" class="w-100 mb-2" @click="logOff" />
    </div>
  </div>
</template>
