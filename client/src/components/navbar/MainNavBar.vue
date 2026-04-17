<script setup lang="ts">

import { onMounted, ref, watch } from 'vue'
import MegaMenu from 'primevue/megamenu'
import AvatarDropdown from '@/components/navbar/AvatarDropdown.vue'
import { useRouter } from 'vue-router'

import ExpressionMenuItem from '@/components/navbar/navMenuItems/ExpressionMenuItem.vue'
import CharacterMenuItem from '@/components/navbar/navMenuItems/CharacterMenuItem.vue'
import RootNodeMenuItem from '@/components/navbar/navMenuItems/RootNodeMenuItem.vue'
import SimpleMenuItem from '@/components/navbar/navMenuItems/SimpleMenuItem.vue'
import { userStore } from '@/stores/userStore'
import { cmsStore } from '@/stores/cmsStore.ts'
import { storeToRefs } from 'pinia'
import EventCheckinBanner from '@/components/conCheckin/EventCheckinBanner.vue'
import GoCheckinBanner from '@/components/conCheckin/GoCheckinBanner.vue'
import { addRootMenuAndChildren } from '@/components/navbar/helpers/navUtilities.ts'
import { populateAdminMenu } from '@/components/navbar/helpers/adminMenuGenerator.ts'
import { useQuery, useQueryCache } from '@pinia/colada'
import { characterListQuery } from '@/components/navbar/stores/archetypeStore.ts'

const router = useRouter()
const cmsData = cmsStore()

const items = ref([
  {
    root: true,
    label: 'Characters',
    icon: 'pi pi-file',
    subtext: 'Characters',
    items: [],
  },
  {
    root: true,
    label: 'Rule Book',
    items: [],
  },
  {
    root: true,
    label: 'Expressions',
    items: [],
  },
  {
    root: true,
    label: 'World Background',
    items: [],
  },
  {
    root: true,
    label: 'Stone Puller',
    icon: 'pi pi-file',
    subtext: 'Stone Puller',
    command: () => router.push('/stonePuller'),
  },
  {
    root: true,
    label: 'Code of Conduct',
    route: 'code-of-conduct',
    command: () => router.push('/codeofconduct'),
  },
])

function MapData(expression, navMenuHeading: string) {
  return {
    navMenuType: navMenuHeading,
    data: expression,
  }
}

function fillMenu(menuItems: any[], menuLabel: string, navMenuHeading: string) {
  const column1 = menuItems.slice(0, Math.ceil(menuItems.length / 2))
  const column2 = menuItems.slice(Math.ceil(menuItems.length / 2), menuItems.length)

  const expressionMenu = items.value.find(item => item.label === menuLabel)?.items

  expressionMenu.length = 0

  if (expressionMenu !== undefined) {
    expressionMenu.push([{
      items: column1.map(x => MapData(x, navMenuHeading)),
    }])
    expressionMenu.push([{
      items: column2.map(x => MapData(x, navMenuHeading)),
    }])
  }
}

async function loadList() {
  const userInfo = userStore()
  await userInfo.updateUserFeatureFlags()

  const adminMenu = addRootMenuAndChildren('Admin', 'pi pi-admin', populateAdminMenu())
  if (adminMenu) {
    items.value.splice(-1, 0, adminMenu)
  }

  await cmsData.getCmsInformation()
  fillMenu(cmsData.worldBackgroundItems, 'World Background', 'worldbackground')
  fillMenu(cmsData.rulebookItems, 'Rule Book', 'rulebook')
  fillMenu(cmsData.expressionItems, 'Expressions', 'expressions')
}

onMounted(async () => {
  await populateCharacterMenu()
  await loadList()
})

const populateCharacterMenu = async () => {
  const queryCache = useQueryCache()
  await queryCache.refresh(queryCache.ensure(characterListQuery))
  const { data } = useQuery(characterListQuery)
  fillMenu(data.value!, 'Characters', 'character')
}

const { worldBackgroundItems, rulebookItems, expressionItems } = storeToRefs(cmsData)

watch(worldBackgroundItems, (newValue) => {
  fillMenu(newValue, 'World Background', 'worldbackground')
})
watch(rulebookItems, (newValue) => {
  fillMenu(newValue, 'Rule Book', 'rulebook')
})
watch(expressionItems, (newValue) => {
  fillMenu(newValue, 'Expressions', 'expressions')
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
