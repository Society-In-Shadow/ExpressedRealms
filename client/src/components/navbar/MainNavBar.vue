<script setup lang="ts">

import {onMounted, ref} from "vue";
import MegaMenu from "primevue/megamenu";
import AvatarDropdown from "@/components/navbar/AvatarDropdown.vue";
import {useRouter} from "vue-router";
import axios from "axios";

import ExpressionMenuItem from "@/components/navbar/navMenuItems/ExpressionMenuItem.vue";
import CharacterMenuItem from "@/components/navbar/navMenuItems/CharacterMenuItem.vue";
import RootNodeMenuItem from "@/components/navbar/navMenuItems/RootNodeMenuItem.vue";
import EditExpression from "@/components/expressions/EditExpression.vue";
import Dialog from 'primevue/dialog';
import AddExpression from "@/components/expressions/AddExpression.vue";
import {FeatureFlags, userStore} from "@/stores/userStore";
import GeneralContentItem from "@/components/navbar/navMenuItems/GeneralContentItem.vue";

const userInfo = userStore();
const Router = useRouter();
let showExpressionEdit = false;
const router = useRouter();

const items = ref([
  { 
    root: true, 
    label: 'Characters', 
    icon: 'pi pi-file', 
    subtext: 'Characters', 
    command: () => router.push("/characters"),  
    items: [] 
  },
  {
    root: true,
    label: 'Rule Book',
    icon: 'pi pi-file',
    subtext: 'Rule Book',
    items: [
      [
        {
          items: [
            {
              navMenuType: "cms",
              label: 'Quick Start Guide',
              icon: 'user_attributes',
              command: () => router.push("/characterQuickStart")
            },
            {
              navMenuType: "cms",
              label: 'Character Setup',
              icon: 'article_person',
              command: () => router.push("/characterSetup")
            },
            {
              navMenuType: "cms",
              label: 'Knowledges',
              icon: 'cognition_2',
              command: () => router.push("/knowledges")
            }
          ]
        }
      ],
      [
        { 
          items: [
            {
              navMenuType: "cms",
              label: 'Advantages / Disadvantages / Mixed Blessings',
              icon: 'chess_bishop_2',
              command: () => router.push("/blessings")
            },
            {
              navMenuType: "cms",
              label: 'Combat',
              icon: 'swords',
              command: () => router.push("/combat")
            },
            {
              navMenuType: "cms",
              label: 'Equipment',
              icon: 'backpack',
              command: () => router.push("/equipment"),
              visible: () => userInfo.userFeatureFlags.includes(FeatureFlags.ShowInventoryPage)
            }
          ]
        }

      ]
    ]
  },
  { 
    root: true, 
    label: 'Expressions', 
    items: [] 
  },
  {
    root: true,
    label: 'World Background',
    icon: 'pi pi-file',
    subtext: '',
    items: [
      [
        {
          items: [
            {
              navMenuType: "cms",
              label: 'Treasured Tales',
              icon: 'auto_stories',
              command: () => router.push("/treasuredtales")
            },
            {
              navMenuType: "cms",
              label: 'Adversaries',
              icon: 'skull_list',
              command: () => router.push("/adversaries")
            }
          ]
        }
      ],
      [
        { 
          items: [
/*            {
              navMenuType: "cms",
              label: 'Factions',
              icon: 'safety_divider',
              command: () => router.push("/factions"),
            },*/
            {
              navMenuType: "cms",
              label: 'The Society',
              icon: 'hub',
              command: () => router.push("/society")
            }
          ]
        }

      ]
    ]
  },
  { 
    root: true, 
    label: 'Stone Puller', 
    icon: 'pi pi-file', 
    subtext: 'Stone Puller', 
    command: () => router.push("/stonePuller") 
  },
  { 
    root: true, 
    label: 'Admin', 
    icon: 'pi pi-admin', 
    subtext: 'See User List', 
    command: () => router.push("/admin/players"), 
    visible: () => userInfo.userRoles.includes("UserManagementRole") },
  { 
    root: true, 
    label: 'Code of Conduct', 
    route: 'code-of-conduct', 
    command: () => router.push("/codeofconduct") 
  },
]);

async function loadList(){

  const userInfo = userStore();
  await userInfo.updateUserFeatureFlags()
  function MapData(expression) {
    return {
      navMenuType: "expression",
      expression: expression,
    };
  }

  function MapCharacterData(character) {
    return {
      id: character.id,
      name: character.name,
      icon: 'pi pi-cloud',
      background: character.background,
      expression: character.expression,
      navMenuType: "character",
      command: () => {
        Router.push("/characters/" + character.id);
      }
    };
  }

  axios.get("/navMenu/expressions")
      .then(response => {
        const expressions = response.data;

        showExpressionEdit = expressions.canEdit;
        const menuItems = expressions.menuItems;

        const column1 = menuItems.slice(0, Math.ceil(menuItems.length / 2));
        const column2 = menuItems.slice(Math.ceil(menuItems.length / 2), menuItems.length);

        const expressionMenu = items.value.find(item => item.label === 'Expressions')?.items;

        expressionMenu.length = 0;
        
        if(expressionMenu !== undefined){
          expressionMenu.push([{
            items: column1.map(MapData)
          }]);
          expressionMenu.push([{
            items: column2.map(MapData)
          }]);

        }

      })

  axios.get("/navMenu/characters")
      .then(response => {
        const characters = response.data;

        const column1 = characters.slice(0, Math.ceil(characters.length / 2));
        const column2 = characters.slice(Math.ceil(characters.length / 2), characters.length);

        const expressionMenu = items.value.find(item => item.label === 'Characters')?.items;

        expressionMenu.length = 0;
        
        if(expressionMenu !== undefined){
          expressionMenu.push([{
            items: column1.map(MapCharacterData)
          }]);
          expressionMenu.push([{
            items: column2.map(MapCharacterData)
          }]);

        }

      })

}

onMounted(async () => {
  await loadList();
});

let editVisible = ref(false);
let expressionId = ref();
function showEditExpressionPopup(id){
  editVisible.value = true;
  expressionId.value = id;
}
let newVisible = ref(false);
function showCreateExpressionPopup(){
  newVisible.value = true;
}

</script>

<template>
  <Dialog v-model:visible="editVisible" modal header="Edit Expression">
    <EditExpression :expression-id="expressionId" @refresh-list="loadList" />
  </Dialog>
  <Dialog v-model:visible="newVisible" modal header="Add Expression">
    <AddExpression @refresh-list="loadList" @close-dialog="newVisible = false" />
  </Dialog>
  <MegaMenu :model="items" class="ms-0 me-0 mt-2 mb-2 m-md-2 d-print-none">
    <template #start>
      <RouterLink to="/">
        <img src="/favicon.png" alt="A white, black, blue, red, green, and transparent marbles organized in a pentagon pattern. The white stone is at the top and the transparent stone is in the center." height="50" width="50" class="m-2">
      </RouterLink>
    </template>
    <template #item="{ item }">
      <RootNodeMenuItem v-if="item.root" :item="item" />
      <CharacterMenuItem v-else-if="item.navMenuType == 'character'" :item="item" />
      <GeneralContentItem v-else-if="item.navMenuType == 'cms'" :item="item" />
      <ExpressionMenuItem
        v-else :item="item.expression" :show-edit="showExpressionEdit" @show-edit-popup="showEditExpressionPopup" @show-create-popup="showCreateExpressionPopup"
        @refresh-list="loadList"
      />
    </template>
    <template #end>
      <avatar-dropdown />
    </template>
  </MegaMenu>
</template>
