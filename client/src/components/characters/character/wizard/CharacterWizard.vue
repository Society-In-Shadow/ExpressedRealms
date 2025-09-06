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
    <div class="col col-md-2 custom-toc">
      <Card>
        <template #content>
          <div v-for="section in sections" class="text-right p-2">
            <Button class="w-100" :label="section.name"/>
          </div>
        </template>
      </Card>

    </div>
    <div class="col custom-toc">
      <Card>
        <template #content>
          <KnowledgeStep />
        </template>
      </Card>
    </div>
    <div class="col custom-toc">
      <Card>
        <template #content>
          <div id="item-modification-section"></div>
        </template>
      </Card>
    </div>
  </div>

</template>

<style scoped>
@media(min-width: 768px){
  .custom-toc {
    max-height: calc(100vh - 1rem);
    overflow-y: auto;
    height:100%;
    min-width: 18em;
  }
}

@media(max-width: 768px){
  .custom-card > .p-card-body{
    padding: 0.75rem !important;
  }

  .custom-toc > .p-card-body{
    padding-left: 1rem !important;
    padding-right: 1rem !important;
  }

  .custom-card .p-tabpanels{
    padding: 0.5rem !important;
  }
}
</style>