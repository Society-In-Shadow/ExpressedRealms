<script setup lang="ts">

import {computed, ref} from "vue";
import TieredMenu from "primevue/tieredmenu";
import Avatar from "primevue/avatar"
import {userStore} from "@/stores/userStore";
import md5 from "md5"
import { logOff } from "@/services/Authentication";
import {useRouter} from "vue-router";
let userInfo = userStore();
const Router = useRouter();

const menu = ref();
const items = ref([
  {
    label: 'My Profile',
    id: 'myProfile',
    icon: 'pi pi-user-edit',
    command: () => {
      Router.push('/userProfile');
    }
  },
  {
    label: 'Logoff',
    id: 'logoff',
    icon: 'pi pi-sign-out',
    command: () => { logOff(); }
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
  <a class="flex align-items-center p-3 cursor-pointer mb-2 gap-2 no-underline text-100" href="https://discord.gg/NSv3GxSAj7" target="_blank">
    <Avatar class="pi pi-discord" shape="circle" size="large" />
    <div class="hideIfSmall">Discord</div>
  </a>
  <a class="flex align-items-center p-3 cursor-pointer mb-2 gap-2" aria-haspopup="true" aria-controls="overlay_tmenu" @click="toggle">
    <Avatar :image="gravatar" shape="circle" size="large" />
    <div class="hideIfSmall">{{ userInfo.name }}</div>
    <i class="pi pi-caret-down text-lg" />
    <TieredMenu id="overlay_tmenu" ref="menu" :model="items" popup />
  </a>
</template>

<style scoped>
  @media(max-width: 576px){
    .hideIfSmall{
      display: none;
    }
  }
</style>
