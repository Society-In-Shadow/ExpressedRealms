<script setup lang="ts">

import Card from "primevue/card";
import { onMounted } from "vue";
import { useRoute } from 'vue-router'
import {proficiencyStore} from "@/components/characters/character/proficiency/stores/proficiencyStore";
import InputNumber from 'primevue/inputnumber';
const route = useRoute()
const profStore = proficiencyStore();

onMounted(() =>{
  profStore.getUpdateProficiencies(route.params.id);
});

</script>

<template>
  <Card class="custom-tile p-1 p-md-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch w-100 max-width">
    <template #content>
      <div class="d-inline-flex flex-wrap gap-1 justify-content-center">
        <div v-for="proficiency in profStore.secondary" :key="proficiency.id" :value="proficiency.id" class="vitality-tile p-2 m-md-2 p-md-3">
          <div class="pb-2">
            {{ proficiency.name }}
          </div>
          <InputNumber
            v-model="proficiency.value" :suffix="' / ' + proficiency.maxValue" 
            :min="0" :max="proficiency.maxValue" 
            show-buttons button-layout="horizontal"
            fluid style="width: 10em" class="text-center"
          >
            <template #incrementbuttonicon>
              <span class="pi pi-plus" />
            </template>
            <template #decrementbuttonicon>
              <span class="pi pi-minus" />
            </template>
          </InputNumber>
        </div>
      </div>
    </template>
  </Card>
</template>

<style>
@media(min-width: 768px){
  .max-width {
    width: 100%;
    max-width: 75em
  }
  .custom-tile .p-card-body {
    padding-left: inherit !important;
    padding-right: inherit !important;
  }
}

.vitality-tile{
  border: 1px solid var(--p-fieldset-border-color);
  border-radius: var(--p-fieldset-border-radius);
}

.custom-tile .p-card-body{
  padding-left: 0 !important;
  padding-right: 0 !important;
}

 .p-inputnumber-input{
   text-align: center;
 }

</style>
