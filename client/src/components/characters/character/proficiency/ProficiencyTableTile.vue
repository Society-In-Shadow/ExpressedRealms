<script setup lang="ts">

import {computed, onMounted, ref} from "vue";
import {useRoute} from 'vue-router'
import {proficiencyStore} from "@/components/characters/character/proficiency/stores/proficiencyStore";
import Panel from "primevue/panel";
import Accordion from 'primevue/accordion';
import ProficiencyAccordionPanel from "@/components/characters/character/proficiency/ProficiencyAccordionPanel.vue";

const route = useRoute()

const openItems = ref([]);

const profStore = proficiencyStore();

const types = computed(() => [
  { name: "Offensive Proficiencies", items: profStore.offensive },
  { name: "Defensive Proficiencies", items: profStore.defensive },
]);

onMounted(() =>{
  profStore.getUpdateProficiencies(route.params.id);
});

</script>

<template>
  <div class="d-inline-flex flex-wrap justify-content-center column-gap-3 row-gap-1 w-100">
    <Panel v-for="type in types" :key="type.name" class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" style="width: 25em">
      <template #header>
        <h3 class="pb-0 mb-0 mt-0 pt-0">
          {{ type.name }}
        </h3>
      </template>
      <Accordion :value="openItems" multiple :lazy="true" expand-icon="pi pi-info-circle" collapse-icon="pi pi-times-circle">
        <ProficiencyAccordionPanel v-for="proficiency in type.items" :key="proficiency.id" :value="proficiency.id" :proficiency="proficiency"/>
      </Accordion>
    </Panel>
  </div>
</template>

<style>
@media(min-width: 768px){
  .max-width {
    width: 100%;
    max-width: 75em
  }
}

.p-panel-header{
  background: var(--p-panel-background) !important;
  border-bottom: 0px !important;
  padding: 1.5em 1.5em 0em !important;
}
</style>
