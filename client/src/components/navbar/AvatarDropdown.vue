<script setup lang="ts">

import { computed, ref } from 'vue'
import TieredMenu from 'primevue/tieredmenu'
import Avatar from 'primevue/avatar'
import md5 from 'md5'
import { logOff } from '@/services/Authentication'
import { useRouter } from 'vue-router'
import { userInfoQuery } from '@/auth/authStore.ts'
import { useQueryWithLoading } from '@/utilities/queryOverride.ts'
import NavSearch from '@/components/navbar/search/NavSearch.vue'

const Router = useRouter()
const { data } = useQueryWithLoading(userInfoQuery)
const menu = ref()
const items = ref([
  {
    label: 'My Profile',
    id: 'myProfile',
    icon: 'pi pi-user-edit',
    command: () => {
      Router.push('/userProfile')
    },
  },
  {
    label: 'Characters',
    id: 'characters',
    icon: 'pi pi-users',
    command: () => {
      Router.push('/characters')
    },
  },
  {
    label: 'Logoff',
    id: 'logoff',
    icon: 'pi pi-sign-out',
    command: () => { logOff(Router) },
  },
])

const toggle = (event) => {
  menu.value.toggle(event)
}

const gravatar = computed(() => {
  const hash = md5(data!.value.userInfo.email.trim().toLowerCase())
  return `https://www.gravatar.com/avatar/${hash}`
})

</script>

<template>
  <div class="d-flex flex-row gap-2">
    <div class="flex align-items-center cursor-pointer">
      <NavSearch />
    </div>

    <a href="https://discord.gg/NSv3GxSAj7" target="_blank" class="flex align-items-center cursor-pointer  no-underline">
      <Avatar class="pi pi-discord avatar-size" shape="circle" />
    </a>
    <a
      v-if="data!.userInfo !== null" class="flex align-items-center cursor-pointer" aria-haspopup="true" aria-controls="overlay_tmenu"
      @click="toggle"
    >
      <Avatar :image="gravatar" shape="circle" class="avatar-size" style="max-width: 2em; max-height: 2em;" />
      <TieredMenu id="overlay_tmenu" ref="menu" :model="items" popup />
    </a>
    <RouterLink v-else to="/login" class="flex align-items-center cursor-pointer">
      <Avatar class="pi pi-sign-in" shape="circle" />
      <div class="hideIfSmall">
        Login
      </div>
    </RouterLink>
  </div>
</template>
