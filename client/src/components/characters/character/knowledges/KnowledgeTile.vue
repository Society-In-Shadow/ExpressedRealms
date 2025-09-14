<script setup lang="ts">

import {onBeforeMount, ref} from "vue";
import {characterKnowledgeStore} from "@/components/characters/character/knowledges/stores/characterKnowledgeStore";
import {useRoute} from "vue-router";
import Tag from "primevue/tag";
import AccordionPanel from "primevue/accordionpanel";
import Accordion from "primevue/accordion";
import AccordionContent from "primevue/accordioncontent";
import AccordionHeader from "primevue/accordionheader";

const characterKnowledgeData = characterKnowledgeStore();
const route = useRoute();

onBeforeMount(async () => {
  await characterKnowledgeData.getCharacterKnowledges(route.params.id)
})

const openKnowledgeItems = ref([]);

</script>

<template>
  <div style="max-width: 650px; margin: 0 auto;">

    <Accordion :value="openKnowledgeItems" multiple :lazy="true" expand-icon="pi pi-info-circle" collapse-icon="pi pi-times-circle">
      <AccordionPanel v-for="knowledge in characterKnowledgeData.knowledges" :key="knowledge.name" :value="knowledge.mappingId">
        <AccordionHeader>
          <div class="d-flex flex-column flex-grow-1 pr-3">
            <div class="d-flex flex-fill align-content-between d-block">
              <div class="flex-grow-1 font-bold text-900">
                {{ knowledge.knowledge.name }} - <em>{{ knowledge.knowledge.type }}</em>
              </div>
              <div>
                {{ knowledge.levelName }} ({{ knowledge.level }})
              </div>
            </div>
            <div class="d-flex d-block mt-1">
              <div class="flex-grow-1">
                <Tag v-if="knowledge.specializations.length == 0" value="No Specializations" />
                <Tag v-for="special in knowledge.specializations" v-else class="mr-1" :value="special.name" />
              </div>
              <div>Stones: +{{ knowledge.stoneModifier }}</div>
            </div>
          </div>
        </AccordionHeader>
        <AccordionContent>
          <p class="m-0">
            {{ knowledge.knowledge.description }}
          </p>
        
          <h3 v-if="knowledge.notes" class="mt-3">
            Notes
          </h3>
          <p v-if="knowledge.notes">
            {{ knowledge.notes }}
          </p>

          <hr v-if="knowledge.specializations.length > 0" class="mt-2 mb-2">
          <h1 v-if="knowledge.specializations.length > 0" class="mt-3">
            Specializations
          </h1>
          <div v-if="knowledge.specializations.length > 0">
            <div v-for="special in knowledge.specializations" :key="special.id">
              <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
                <div>
                  <h2 class="m-0 p-0">
                    {{ special.name }}
                  </h2>
                </div>
              </div>

              <p>{{ special.description }}</p>
              <h4 v-if="special.notes">
                Notes
              </h4>
              <p v-if="special.notes">
                {{ special.notes }}
              </p>
            </div>
          </div>
        </AccordionContent>
      </AccordionPanel>
    </Accordion>
  </div>
</template>
