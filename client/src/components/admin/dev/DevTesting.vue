<script setup>

import Button from 'primevue/button'
import axios from 'axios'
import toaster from '@/services/Toasters.ts'
import { UserPermissions } from '@/types/UserPermissions.ts'

const testEmail = async () => {
  await axios.post(`/dev/sendTestEmail`)
    .then(async () => {
      toaster.success('Successfully Sent Email to Logged In User!')
    })
}

const getFeatureFlag = async () => {
  await axios.get(`/dev/getFeatureFlag`)
    .then(async (response) => {
      toaster.success(`Successfully got Test Release Flag Status (${response.data})!`)
    })
}

const sendDiscordMessage = async () => {
  await axios.post(`/dev/sendDiscordTestMessage`)
    .then(async () => {
      toaster.success(`Successfully sent test message to Dev Testing Channel!`)
    })
}

const testRedis = async () => {
  await axios.post(`/dev/testRedis`)
    .then(async (response) => {
      toaster.success(`Successfully tested Redis (${response.data})!`)
    })
}

</script>

<template>
  <h1>Developer Tools</h1>
  <p>A small set of tools to test various features and services</p>
  <div class="d-flex flex-column gap-3 ">
    <Button v-permission="UserPermissions.DevDebug.SendTestEmail" label="Test Email" @click="testEmail" />
    <Button v-permission="UserPermissions.DevDebug.GetFeatureFlag" label="Test Feature Flags" @click="getFeatureFlag" />
    <Button v-permission="UserPermissions.DevDebug.SendDiscordMessage" label="Test Sending Discord Message" @click="sendDiscordMessage" />
    <Button v-permission="UserPermissions.DevDebug.TestRedis" label="Test Redis" @click="testRedis" />
  </div>
</template>
