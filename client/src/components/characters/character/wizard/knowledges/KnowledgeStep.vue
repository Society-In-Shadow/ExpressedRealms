<script setup lang="ts">

import Button from "primevue/button";
import {computed, onBeforeMount, ref} from "vue";
import {knowledgeStore} from "@/components/knowledges/stores/knowledgeStore";
import {characterKnowledgeStore} from "@/components/characters/character/knowledges/stores/characterKnowledgeStore";
import {useRoute} from "vue-router";
import Tag from "primevue/tag";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import SelectKnowledgeItem from "@/components/characters/character/wizard/knowledges/SelectKnowledgeItem.vue";
import EditCharacterKnowledge from "@/components/characters/character/wizard/knowledges/EditCharacterKnowledge.vue";

const knowledgeData = knowledgeStore();
const characterKnowledgeData = characterKnowledgeStore();
const route = useRoute();
const experienceInfo = experienceStore();

onBeforeMount(async () => {
  await knowledgeData.getKnowledges();
  await characterKnowledgeData.getCharacterKnowledges(route.params.id)
  if(characterKnowledgeData.knowledges.length === 0)
  {
    noKnowledges.value = true;
  }
})

const noKnowledges = ref(false);
const openKnowledgeItems = ref([]);


const filteredKnowledges = computed(() => {
  const existingKnowledgeIds = characterKnowledgeData.knowledges.map(ck => ck.knowledge.id);
  return knowledgeData.knowledges.filter(knowledge =>
      !existingKnowledgeIds.includes(knowledge.id)
  );

})

const toggleEdit = (knowledgeId: number) => {
  characterKnowledgeData.activeKnowledgeId = knowledgeId;
}

</script>

<template>
  <div style="max-width: 650px; margin: 0 auto;">
    <div class="text-right pb-3" v-if="experienceInfo.showAllExperience">{{ experienceInfo.experienceBreakdown.knowledgeXp}} Total XP - {{experienceInfo.experienceBreakdown.setupKnowledgeXp}} Creation XP = {{experienceInfo.experienceBreakdown.knowledgeXp - experienceInfo.experienceBreakdown.setupKnowledgeXp}} XP</div>
    <h1 v-if="characterKnowledgeData.knowledges.length > 0">Selected Knowledges</h1>
    <div v-for="knowledge in characterKnowledgeData.knowledges" :key="knowledge.id">
      <div class="d-flex flex-column flex-md-row align-self-center justify-content-between pt-2 pb-2">
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
        <div>
          <Button label="View" @click="toggleEdit(knowledge.knowledge.id)" />
          <Teleport v-if="characterKnowledgeData.activeKnowledgeId == knowledge.knowledge.id" to="#item-modification-section">
            <EditCharacterKnowledge :knowledge="knowledge" />
          </Teleport>
        </div>
      </div>
    </div>
    
    <div class="mb-2">
      <hr v-if="characterKnowledgeData.knowledges.length !== 0">
      <h1>Choose Knowledges</h1>
      <div v-if="characterKnowledgeData.knowledges.length === 0">
        <p>No Knowledges detected, please pick one below.</p>
      </div>
      <div v-for="knowledge in filteredKnowledges" :key="knowledge.id" class="pt-1 pb-1">
        <SelectKnowledgeItem :is-read-only="false" :knowledge="knowledge" />
      </div>
    </div>
  </div>
</template>
