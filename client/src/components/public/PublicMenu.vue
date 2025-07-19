<script setup lang="ts">

import MegaMenu from "primevue/megamenu";
import {useRouter} from "vue-router";
import {ref} from "vue";
import AvatarDropdown from "@/components/navbar/AvatarDropdown.vue";

const router = useRouter();

const items = ref([
  { root: true, label: 'Home', route: '', command: () => router.push("/") },
  { root: true, label: 'About', route: 'about', command: () => router.push("/about") },
  { root: true, label: 'Expressions', route: 'expressions', command: () => router.push("/expressions") },
  { root: true, label: 'Contact Us', route: 'contact-us', command: () => router.push("/contact-us") },
  { root: true, label: 'Upcoming Events', route: 'upcoming-events', command: () => router.push("/upcoming-events") },
  { root: true, label: 'Code of Conduct', route: 'code-of-conduct', command: () => router.push("/code-of-conduct") },
]);
</script>

<template>
  <MegaMenu :model="items" class="ms-0 me-0 mt-2 mb-2 m-md-2">
    <template #start>
      <RouterLink to="/">
        <img src="/favicon.png" alt="A white, black, blue, red, green, and transparent marbles organized in a pentagon pattern. The white stone is at the top and the transparent stone is in the center." height="50" width="50" class="m-2">
      </RouterLink>
    </template>
    <template #item="{ item }">
      <a v-if="item.root" class="flex items-center cursor-pointer px-4 py-2 overflow-hidden relative font-semibold text-lg uppercase" :class="{ 'selected-item': router.currentRoute.value.path === '/' + item.route }">
        <span>{{ item.label }}</span>
      </a>
      <a v-else class="flex flex-shrink-1 align-items-center p-3 cursor-pointer mb-2 gap-2">
        {{ item.label }}
      </a>
    </template>
    <template #end>
      <avatar-dropdown />
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
  border-radius: 2rem;
}
</style>
