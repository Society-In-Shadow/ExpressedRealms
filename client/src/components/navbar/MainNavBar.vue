<script setup lang="ts">

import { onMounted, ref, watch } from 'vue'
import MegaMenu from 'primevue/megamenu'
import AvatarDropdown from '@/components/navbar/AvatarDropdown.vue'
import { useRouter } from 'vue-router'
import axios from 'axios'

import ExpressionMenuItem from '@/components/navbar/navMenuItems/ExpressionMenuItem.vue'
import CharacterMenuItem from '@/components/navbar/navMenuItems/CharacterMenuItem.vue'
import RootNodeMenuItem from '@/components/navbar/navMenuItems/RootNodeMenuItem.vue'
import SimpleMenuItem from '@/components/navbar/navMenuItems/SimpleMenuItem.vue'
import { userStore } from '@/stores/userStore'
import { cmsStore } from '@/stores/cmsStore.ts'
import { storeToRefs } from 'pinia'

const userInfo = userStore()
const Router = useRouter()
const router = useRouter()
const cmsData = cmsStore()

const items = ref([
  {
    root: true,
    label: 'Characters',
    icon: 'pi pi-file',
    subtext: 'Characters',
    command: () => router.push('/characters'),
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
    label: 'Admin',
    icon: 'pi pi-admin',
    visible: () => userInfo.userRoles.includes('UserManagementRole') || userInfo.userRoles.includes('ManagePlayerCharacterList'),
    items: [[{
      items: [
        {
          navMenuType: 'simple',
          label: 'Users',
          navMenuIcon: 'groups',
          pushComponentRouteName: 'viewPlayers',
          description: 'Manage all users in the system.',
          visible: () => userInfo.userRoles.includes('UserManagementRole'),
        },
        {
          navMenuType: 'simple',
          label: 'Events',
          navMenuIcon: 'calendar_month',
          pushComponentRouteName: 'adminEventList',
          description: 'Manage all events in the system.',
          visible: () => userInfo.userRoles.includes('ManageEvents'),
        },
      ],
    },
    ],
    [
      {
        items: [
          {
            navMenuType: 'simple',
            label: 'Role Management',
            description: 'Manage Roles that users can have.',
            navMenuIcon: 'group',
            pushComponentRouteName: 'adminRoleList',
            visible: () => userInfo.userRoles.includes('UserManagementRole'),
          },
          {
            navMenuType: 'simple',
            label: 'Character Management',
            description: 'Manage any primary characters across all players.',
            navMenuIcon: 'patient_list',
            pushComponentRouteName: 'adminCharacterList',
            visible: () => userInfo.userRoles.includes('ManagePlayerCharacterList'),
          },
        ],
      },
    ]],
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
    expression: expression,
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

  function MapCharacterData(character) {
    return {
      id: character.id,
      name: character.name,
      icon: 'pi pi-cloud',
      background: character.background,
      expression: character.expression,
      navMenuType: 'character',
      command: () => {
        Router.push('/characters/' + character.id)
      },
    }
  }

  await cmsData.getCmsInformation()
  fillMenu(cmsData.worldBackgroundItems, 'World Background', 'worldbackground')
  fillMenu(cmsData.rulebookItems, 'Rule Book', 'rulebook')
  fillMenu(cmsData.expressionItems, 'Expressions', 'expressions')

  axios.get('/navMenu/characters')
    .then((response) => {
      const characters = response.data

      const column1 = characters.slice(0, Math.ceil(characters.length / 2))
      const column2 = characters.slice(Math.ceil(characters.length / 2), characters.length)

      const expressionMenu = items.value.find(item => item.label === 'Characters')?.items

      expressionMenu.length = 0

      if (expressionMenu !== undefined) {
        expressionMenu.push([{
          items: column1.map(MapCharacterData),
        }])
        expressionMenu.push([{
          items: column2.map(MapCharacterData),
        }])
      }
    })
}

onMounted(async () => {
  await loadList()
})

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
  <MegaMenu :model="items" class="ms-0 me-0 mt-2 mb-2 m-md-2 d-print-none">
    <template #start>
      <RouterLink to="/">
        <img src="/favicon.png" alt="A white, black, blue, red, green, and transparent marbles organized in a pentagon pattern. The white stone is at the top and the transparent stone is in the center." height="50" width="50" class="m-2">
      </RouterLink>
    </template>
    <template #item="{ item }">
      <RootNodeMenuItem v-if="item.root" :item="item" />
      <SimpleMenuItem v-else-if="item.navMenuType == 'simple'" :item="item" />
      <CharacterMenuItem v-else-if="item.navMenuType == 'character'" :item="item" />
      <ExpressionMenuItem v-else :item="item.expression" :nav-heading="item.navMenuType" />
    </template>
    <template #end>
      <avatar-dropdown />
    </template>
  </MegaMenu>
</template>
