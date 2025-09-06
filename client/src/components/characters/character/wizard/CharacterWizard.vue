<script setup lang="ts">
import Button from 'primevue/button';
import Card from 'primevue/card';


import {ref} from "vue";
import KnowledgeStep from "@/components/characters/character/wizard/knowledges/KnowledgeStep.vue";

const sections = ref([
  { name: 'Getting Started'},
  { name: 'Expression' },
  { name: 'Stats' },
  { name: 'Knowledges' },
  { name: 'Powers' },
  { name: 'Skills' },
  { name: 'Proficiencies' },
]);


</script>

<template>
  <div class="row pt-3">
    <div class="col-2">
      <Card>
        <template #content>
          <div v-for="section in sections" class="text-right p-2">
            <Button class="w-100" :label="section.name"/>
          </div>
        </template>
      </Card>

    </div>
    <div class="col">
      <Card>
        <template #content>
          <KnowledgeStep />
        </template>
      </Card>
    </div>
    <div class="col">
      <Card>
        <template #content>
          <div id="item-modification-section"></div>
        </template>
      </Card>
    </div>
  </div>

</template>

<style scoped>

</style>