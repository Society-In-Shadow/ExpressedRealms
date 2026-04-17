<script setup lang="ts">

import { computed, onMounted } from 'vue'
import MegaMenu from 'primevue/megamenu'
import AvatarDropdown from '@/components/navbar/AvatarDropdown.vue'
import { useRouter } from 'vue-router'

import ExpressionMenuItem from '@/components/navbar/navMenuItems/ExpressionMenuItem.vue'
import CharacterMenuItem from '@/components/navbar/navMenuItems/CharacterMenuItem.vue'
import RootNodeMenuItem from '@/components/navbar/navMenuItems/RootNodeMenuItem.vue'
import SimpleMenuItem from '@/components/navbar/navMenuItems/SimpleMenuItem.vue'
import { userStore } from '@/stores/userStore'
import { cmsStore } from '@/stores/cmsStore.ts'
import EventCheckinBanner from '@/components/conCheckin/EventCheckinBanner.vue'
import GoCheckinBanner from '@/components/conCheckin/GoCheckinBanner.vue'
import { populateAdminMenu } from '@/components/navbar/helpers/adminMenuGenerator.ts'
import { useQuery, useQueryCache } from '@pinia/colada'
import { characterListQuery } from '@/components/navbar/stores/navMenuStore.ts'
import { type Character, CharacterState } from '@/components/navbar/types.ts'
import { breakpointsBootstrapV5, useBreakpoints } from '@vueuse/core'
import { addAdminMenuItems, fillComputedMenu } from '@/components/navbar/helpers/navUtilities.ts'

const router = useRouter()
const cmsData = cmsStore()

const items = computed(() => [
  {
    root: true,
    label: 'Characters',
    items: charactersMenu.value,
  },
  {
    root: true,
    label: 'Rule Book',
    items: ruleBookMenu.value,
  },
  {
    root: true,
    label: 'Expressions',
    items: expressionMenu.value,
  },
  {
    root: true,
    label: 'World Background',
    items: worldBackgroundMenu.value,
  },
  {
    root: true,
    label: 'Stone Puller',
    icon: 'pi pi-file',
    command: () => router.push('/stonePuller'),
  },
  {
    root: true,
    label: 'Admin',
    icon: 'pi pi-admin',
    visible: () => (adminMenu.value?.length ?? 0) > 0,
    items: adminMenu.value,
  },
  {
    root: true,
    label: 'Code of Conduct',
    command: () => router.push('/codeofconduct'),
  },
])

async function loadList() {
  const userInfo = userStore()
  await userInfo.updateUserFeatureFlags()
  await cmsData.getCmsInformation()
}

onMounted(async () => {
  await populateCharacterMenu()
  await loadList()
})

const { data: characterData } = useQuery(characterListQuery)

const activeBreakpoint = useBreakpoints(breakpointsBootstrapV5)
const isMobile = activeBreakpoint.smaller('md')

const populateCharacterMenu = async () => {
  const queryCache = useQueryCache()
  await queryCache.refresh(queryCache.ensure(characterListQuery))
}

const charactersMenu = computed(() => {
  const list = characterData.value ?? []
  const characters: Array<Character> = [...list]
  if (characters.length > 0) {
    const insertSpot = isMobile.value
      ? characters.length
      : Math.max(Math.floor(characters.length / 2), 1)
    characters.splice(insertSpot, 0, { id: -1, name: 'View Characters', expression: '', state: CharacterState.Regular })
  }

  characters.push({ id: -2, name: 'Add Character', expression: '', state: CharacterState.Regular })
  return fillComputedMenu(characters, 'character')
})

const worldBackgroundMenu = computed(() => {
  return fillComputedMenu(cmsData.worldBackgroundItems ?? [], 'worldbackground')
})

const ruleBookMenu = computed(() => {
  return fillComputedMenu(cmsData.rulebookItems ?? [], 'rulebook')
})

const expressionMenu = computed(() => {
  return fillComputedMenu(cmsData.expressionItems ?? [], 'expressions')
})

const adminMenu = computed(() => {
  return addAdminMenuItems(populateAdminMenu())
})

</script>

<template>
  <EventCheckinBanner />
  <GoCheckinBanner />
  <MegaMenu :model="items" class="ms-0 me-0 mt-2 mb-2 m-md-2 d-print-none">
    <template #start>
      <RouterLink to="/">
        <img src="/favicon.png" alt="A white, black, blue, red, green, and transparent marbles organized in a pentagon pattern. The white stone is at the top and the transparent stone is in the center." height="50" width="50" class="m-2">
      </RouterLink>
    </template>
    <template #item="{ item }">
      <RootNodeMenuItem v-if="item.root" :item="item" />
      <SimpleMenuItem v-else-if="item.navMenuType == 'simple'" :item="item" />
      <CharacterMenuItem v-else-if="item.navMenuType == 'character'" :item="item.data" />
      <ExpressionMenuItem v-else :item="item.data" :nav-heading="item.navMenuType" />
    </template>
    <template #end>
      <avatar-dropdown />
    </template>
  </MegaMenu>
</template>

<style>

@media(min-width: 768px) {
  .p-megamenu-overlay {
    margin-top: 3em !important;
  }
}

.p-megamenu-submenu-label{
  padding: 0 !important
}
.p-megamenu-col-12{
  padding-top: 0 !important;
  padding-bottom: 0 !important;
}
.p-megamenu-submenu {
  padding-top: 0 !important;
  padding-bottom: 0 !important;
}
.p-megamenu-item {
  padding: 0 !important;
  margin: 0 !important;
}
</style>
