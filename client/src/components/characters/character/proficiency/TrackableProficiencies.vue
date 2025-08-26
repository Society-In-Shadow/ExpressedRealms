<script setup lang="ts">

import Card from "primevue/card";
import Accordion from 'primevue/accordion';
import ProficiencyAccordionPanel from "@/components/characters/character/proficiency/ProficiencyAccordionPanel.vue";
import {onMounted, ref} from "vue";
import {useRoute} from 'vue-router'
import {proficiencyStore} from "@/components/characters/character/proficiency/stores/proficiencyStore";


const route = useRoute()
const profStore = proficiencyStore();

const props = defineProps({
  showTitle: {
    type: Boolean,
    required: false,
    default: true
  }
})

onMounted(() =>{
  profStore.getUpdateProficiencies(route.params.id);
});

const openItems = ref([]);
</script>

<template>
  <Card class="p-1 p-md-3">
    <template #header >
      <h1 class="text-center" v-if="showTitle">Secondary Statistics</h1>
    </template>
    <template #content>
      <Accordion :value="openItems" multiple :lazy="true" expand-icon="pi pi-info-circle" collapse-icon="pi pi-times-circle">
        <ProficiencyAccordionPanel v-for="proficiency in profStore.secondary" :key="proficiency.id" :value="proficiency.id" :proficiency="proficiency"/>
      </Accordion>
    </template>
  </Card>
</template>
