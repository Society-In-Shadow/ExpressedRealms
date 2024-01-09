<script setup lang="ts">

import { ref } from "vue";
import { useRouter } from 'vue-router'
import TieredMenu from "primevue/tieredmenu";
import Avatar from "primevue/avatar"
import axios from "axios";

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

</script>

<template>
  <a class="flex align-items-center p-3 cursor-pointer mb-2 gap-2" @click="toggle" aria-haspopup="true" aria-controls="overlay_tmenu">
    <Avatar image="https://primefaces.org/cdn/primevue/images/avatar/amyelsner.png" shape="circle" />
    <div>The User</div>
    <i class="pi pi-caret-down text-lg"></i>
    <TieredMenu ref="menu" id="overlay_tmenu" size="large" :model="items" popup />
  </a>
</template>

<style scoped>

</style>