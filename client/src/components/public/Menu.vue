<script setup lang="ts">

import MegaMenu from "primevue/megamenu";
import Avatar from "primevue/avatar";
import {useRouter} from "vue-router";
import {ref} from "vue";


const router = useRouter();

const items = ref([
  { root: true, label: 'Home', route: '', command: () => router.push("/") },
  { root: true, label: 'About', route: 'about', command: () => router.push("/about") },
  { root: true, label: 'Expressions', route: 'expressions', command: () => router.push("/expressions") },
  { root: true, label: 'Contact Us', route: 'contact-us', command: () => router.push("/contact-us") },
  { root: true, label: 'Upcoming Events', route: 'upcoming-events', command: () => router.push("/upcoming-events") },
]);
</script>

<template>
  <MegaMenu :model="items" class="ms-0 me-0 mt-2 mb-2 m-md-2">
    <template #start>
      <img src="/favicon.png" alt="A white, black, blue, red, green, and transparent marbles organized in a pentagon pattern. The white stone is at the top and the transparent stone is in the center." height="50" width="50" class="m-2">
    </template>
    <template #item="{ item }">
      <a v-if="item.root" class="flex items-center cursor-pointer px-4 py-2 overflow-hidden relative font-semibold text-lg uppercase" style="border-radius: 2rem" :class="{ 'selected-item': router.currentRoute.value.path === '/' + item.route }">
        <span>{{ item.label }}</span>
      </a>
      <a v-else-if="!item.image" class="flex items-center p-4 cursor-pointer mb-2 gap-3">
            <span class="inline-flex items-center justify-center rounded-full bg-primary text-primary-contrast w-12 h-12">
                <i :class="[item.icon, 'text-lg']"></i>
            </span>
        <span class="inline-flex flex-col gap-1">
                <span class="font-bold text-lg">{{ item.label }}</span>
                <span class="whitespace-nowrap">{{ item.subtext }}</span>
            </span>
      </a>
      <div v-else class="flex flex-col items-start gap-4 p-2">
        <img alt="megamenu-demo" :src="item.image" class="w-full" />
        <span>{{ item.subtext }}</span>
        <Button :label="item.label" outlined />
      </div>
    </template>
    <template #end>
      <a class="flex align-items-center p-3 cursor-pointer mb-2 gap-2 no-underline text-100" href="https://discord.gg/NSv3GxSAj7" target="_blank">
        <Avatar class="pi pi-discord" shape="circle" size="large" />
        <div class="hideIfSmall">Discord</div>
      </a>
      <a class="flex align-items-center p-3 cursor-pointer mb-2 gap-2 no-underline text-100" href="/login" target="_blank">
        <Avatar class="pi pi-user" shape="circle" size="large" />
        <div class="hideIfSmall">Login</div>
      </a>
    </template>
  </MegaMenu>
</template>

<style scoped>
@media(max-width: 576px){
  .hideIfSmall{
    display: none;
  }
}

.selected-item {
  background: var(--p-form-field-disabled-background);
}
</style>