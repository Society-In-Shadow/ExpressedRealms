<script setup lang="ts">

  import Button from 'primevue/button';
  import axios from "axios";
  import { useRouter, useRoute } from "vue-router";
  import { ref } from 'vue'
  import {userStore} from "@/stores/userStore";
  import { isLoggedIn, logOff } from "@/services/Authentication";

  let userInfo = userStore();

  const Router = useRouter();
  const route = useRoute();

  let hasError = ref(false);
  let isSuccessful = ref(false);
  let isLoading = ref(true );
  axios.get('/auth/confirmEmail',{
    params: {
      userId: route.query.userId,
      code: route.query.code,
      changedEmail: route.query.changedEmail
    }
  }).then(() => {
    userInfo.hasConfirmedEmail = true;
    if(route.query.changedEmail)
      userInfo.userEmail = route.query.changedEmail;
    isSuccessful.value = true;
    isLoading.value = false;
  }).catch(() => {
    hasError.value = true;
    isLoading.value = false;
  });
  
  let sentConfirmationEmail = ref(false);
  async function resendConfirmationEmail() {
    await axios.post("/auth/resendConfirmationEmail", { email: userInfo.userEmail })
        .then(() => {
          sentConfirmationEmail.value = true;
        });
  }
  
</script>

<template>
  <p v-if="isLoading" class="text-center">
    Confirming your email, please wait...
  </p>
  <div v-if="isSuccessful">
    <p>Your email has been confirmed.</p>
    <div v-if="userInfo.isLoggedIn() && !userInfo.isPlayerSetup">
      <Button data-cy="resend-confirmation-button" label="Continue Setup Process" class="w-100 mb-2" @click="Router.push('/setupProfile')" />
    </div>
    <div v-else>
      <Button data-cy="back-button" label="Back To Login" class="w-100 mb-2" @click="Router.push('/login')" />
    </div>
  </div>
  <div v-else>
    <div v-if="userInfo.isLoggedIn()">
      <p>An error occured while confirming your email. Please try clicking the link in the email again, or pressing the button below to send a new one.</p>
      <p v-show="sentConfirmationEmail" data-cy="resend-confirmation-message">
        You have successfully sent an email confirmation email to your email.
      </p>
      <Button data-cy="resend-confirmation-button" label="Resend Confirmation Link" class="w-100 mb-2" @click="resendConfirmationEmail()" />
      <p>Alternatively, you can log off and try another user.</p>
      <Button data-cy="logoff-button" label="Logoff" class="w-100 mb-2" @click="logOff" />
    </div>
    <div v-else>
      <p>An error occured while confirming your email. Please try clicking the link in the email again, or try logging in to resend it.</p>
      <Button data-cy="back-button" label="Back To Login" class="w-100 mb-2" @click="Router.push('/login')" />
    </div>
  </div>
</template>
