<script setup lang="ts">

import {computed, onMounted, ref} from "vue";
import { useRoute } from 'vue-router'
import {proficiencyStore} from "@/components/characters/character/proficiency/stores/proficiencyStore";
import Panel from "primevue/panel";

const route = useRoute()

import Accordion from 'primevue/accordion';
import AccordionPanel from 'primevue/accordionpanel';
import AccordionHeader from 'primevue/accordionheader';
import AccordionContent from 'primevue/accordioncontent';

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
        <h3 class="pb-0 mb-0 mt-0 pt-0">{{ type.name }}</h3>
      </template>
      <Accordion :value="openItems" multiple :lazy="true" expand-icon="pi pi-info-circle" collapse-icon="pi pi-times-circle">
        <AccordionPanel v-for="proficiency in type.items" :key="proficiency.id" :value="proficiency.id">
          <AccordionHeader>
            <div class="d-flex justify-content-between w-100 pr-3">
              <div>{{ proficiency.name }}</div>
              <div class="text-right">
                {{ proficiency.value }}
              </div>
            </div>
          </AccordionHeader>
          <AccordionContent>
            <div class="p-datatable p-component p-datatable-striped">
              <div class="p-datatable-table-container">
                <table class="w-100 p-datatable-table">
                  <!-- Table header -->
                  <thead class="p-datatable-thead">
                    <tr>
                      <th class="p-datatable-header-cell">
                        Source
                      </th>
                      <th class="p-datatable-header-cell">
                        Details
                      </th>
                      <th class="p-datatable-header-cell">
                        Bonus
                      </th>
                    </tr>
                  </thead>
                  <tbody class="p-datatable-tbody">
                    <tr v-for="(modifier, index) in proficiency.appliedModifiers" :key="index" :class="index % 2 === 0 ? 'p-row-even' : 'p-row-odd'">
                      <td>
                        {{ modifier.name }}
                      </td>
                      <td>
                        {{ modifier.message }}
                      </td>
                      <td class="text-right">
                        {{ modifier.value >= 0 ? '+' : '' }}{{ modifier.value }}
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
          </AccordionContent>
        </AccordionPanel>
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
