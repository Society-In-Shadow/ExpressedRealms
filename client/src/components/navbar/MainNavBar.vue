<script setup lang="ts">

import {onMounted, ref} from "vue";
import MegaMenu from "primevue/megamenu";
import AvatarDropdown from "@/components/navbar/AvatarDropdown.vue";
import {useRouter} from "vue-router";
import axios from "axios";
import Router from "@/router";
import ExpressionMenuItem from "@/components/navbar/navMenuItems/ExpressionMenuItem.vue";
import CharacterMenuItem from "@/components/navbar/navMenuItems/CharacterMenuItem.vue";
import RootNodeMenuItem from "@/components/navbar/navMenuItems/RootNodeMenuItem.vue";

const router = useRouter();

const items = ref([
  { root: true, label: 'Characters', icon: 'pi pi-file', subtext: 'Characters', command: () => router.push("/characters"),  items: [] },
  { root: true, label: 'Expressions', items: [] },
  { root: true, label: 'Stone Puller', icon: 'pi pi-file', subtext: 'Stone Puller', command: () => router.push("/stonePuller") },
]);

onMounted(() => {
  function MapData(expression) {
    return {
          label: expression.name,
          icon: 'pi pi-cloud',
          subtext: expression.shortDescription,
          navMenuType: "expression",
          command: () => {
            Router.push("/expressions/" + expression.name.toLowerCase());
          }
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
        
        const column1 = expressions.slice(0, Math.ceil(expressions.length / 2));
        const column2 = expressions.slice(Math.ceil(expressions.length / 2), expressions.length);
        
        const expressionMenu = items.value.find(item => item.label === 'Expressions')?.items;
        
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

        if(expressionMenu !== undefined){
          expressionMenu.push([{
            items: column1.map(MapCharacterData)
          }]);
          expressionMenu.push([{
            items: column2.map(MapCharacterData)
          }]);

        }

      })
});

</script>

<template>
  <MegaMenu :model="items" class="m-lg-3 m-md-3 m-sm-1 m-1 pb-1 pt-1">
    <template #start>
      <img src="/public/favicon.png" height="50" width="50" class="m-2">
    </template>
    <template #item="{ item }">
      <RootNodeMenuItem v-if="item.root" :item="item" />
      <CharacterMenuItem v-else-if="item.navMenuType == 'character'" :item="item" />
      <ExpressionMenuItem v-else :item="item" />
    </template>
    <template #end>
      <avatar-dropdown />
    </template>
  </MegaMenu>
</template>
