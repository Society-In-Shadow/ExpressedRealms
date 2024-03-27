<script setup lang="ts">

import {onMounted, ref} from "vue";
import MegaMenu from "primevue/megamenu";
import Button from "primevue/button"
import AvatarDropdown from "@/components/navbar/AvatarDropdown.vue";
import {useRouter} from "vue-router";
import axios from "axios";
import Router from "@/router";

const router = useRouter();

const items = ref([
  {
    label: 'Company',
    root: true,
    items: [
      [
        {
          items: [
            { label: 'Weather', icon: 'pi pi-cloud', subtext: 'Random Weather Forecast', command: () => router.push("/weatherforecast") },
            { label: 'Characters', icon: 'pi pi-users', subtext: 'Protected Endpoint Test', command: () => router.push("/characters") },
            { label: 'Case Studies', icon: 'pi pi-file', subtext: 'Subtext of item 1' }
          ]
        }
      ],
      [
        {
          items: [
            { label: 'Adepts', icon: 'pi pi-shield', subtext: 'Incredible martial artists. Masters of the mind. Enlightened healers. Stalwart defenders.' },
            { label: 'Faq', icon: 'pi pi-question', subtext: 'Subtext of item 3' },
            { label: 'Library', icon: 'pi pi-search', subtext: 'Subtext of item 4' }
          ]
        }
      ],
      [
        {
          items: [
            { label: 'Community', icon: 'pi pi-comments', subtext: 'Subtext of item 5' },
            { label: 'Rewards', icon: 'pi pi-star', subtext: 'Subtext of item 6' },
            { label: 'Investors', icon: 'pi pi-globe', subtext: 'Subtext of item 7' }
          ]
        }
      ],
      [
        {
          items: [{ image: 'https://primefaces.org/cdn/primevue/images/uikit/uikit-system.png', label: 'GET STARTED', subtext: 'Build spectacular apps in no time.' }]
        }
      ]
    ]
  },
  {
    label: 'Expressions',
    root: true,
    items: []
  }
]);

onMounted(() => {
  function MapData(expression) {
    return {
          label: expression.name,
          icon: 'pi pi-cloud',
          subtext: expression.shortDescription,
          command: () => {
            Router.push("/expressions/" + expression.name.toLowerCase());
          }
    };
  }

  axios.get("/api/navMenu/expressions")
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
});


</script>

<template>
  <MegaMenu :model="items" class="m-lg-3 m-md-3 m-sm-1 m-1 pb-1 pt-1">
    <template #start>
      <img src="/public/favicon.png" height="50" width="50" class="m-2">
    </template>
    <template #item="{ item }">
      <a v-if="item.root" v-ripple class="flex align-items-center cursor-pointer px-3 py-2 overflow-hidden relative font-semibold text-lg uppercase">
        <span>{{ item.label }}</span>
      </a>
      <a v-else-if="!item.image" class="flex flex-shrink-1 align-items-center p-3 cursor-pointer mb-2 gap-2">
        <span class="inline-flex flex-none align-items-center justify-content-center border-circle bg-primary w-3rem h-3rem">
          <i :class="[item.icon, 'text-lg']" />
        </span>
        <span class="inline-flex flex-column gap-1">
          <span class="font-medium text-lg text-900">{{ item.label }}</span>
          <span class="">{{ item.subtext }}</span>
        </span>
      </a>
      <div v-else class="flex flex-column align-items-start gap-3">
        <img alt="megamenu-demo" :src="item.image" class="w-full">
        <span>{{ item.subtext }}</span>
        <Button :label="item.label" outlined />
      </div>
    </template>
    <template #end>
      <avatar-dropdown />
    </template>
  </MegaMenu>
</template>

<style scoped>

</style>