<script setup lang="ts">

import {computed, ref} from "vue";
import { useRouter } from 'vue-router'
import TieredMenu from "primevue/tieredmenu";
import Avatar from "primevue/avatar"
import axios from "axios";
import {userStore} from "@/stores/userStore";
import md5 from "md5"
let userInfo = userStore();

const router = useRouter();

const menu = ref();
const items = ref([
  {
    label: 'Logoff',
    id: 'logoff',
    icon: 'pi pi-sign-out',
    command: () => {
      axios.post('api/auth/logoff').then(() => {
        router.push('login');
      });
    }
  },
]);

const toggle = (event) => {
  menu.value.toggle(event);
};


const gravatar = computed(() => {
  const hash = md5(userInfo.userEmail.trim().toLowerCase());
  return `https://www.gravatar.com/avatar/${hash}`;
});

</script>

<template>
  <a class="flex align-items-center p-3 cursor-pointer mb-2 gap-2" aria-haspopup="true" aria-controls="overlay_tmenu" @click="toggle">
    <Avatar :image="gravatar" shape="circle" size="large" />
    <div>{{ userInfo.userEmail }}</div>
    <i class="pi pi-caret-down text-lg" />
    <TieredMenu id="overlay_tmenu" ref="menu" :model="items" popup />
  </a>
</template>

<style scoped>

</style>