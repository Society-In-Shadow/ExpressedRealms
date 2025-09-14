<script setup lang="ts">

import Button from "primevue/button";
import {onBeforeMount, ref, watch} from "vue";
import {knowledgeStore} from "@/components/knowledges/stores/knowledgeStore";
import {characterKnowledgeStore} from "@/components/characters/character/knowledges/stores/characterKnowledgeStore";
import {useRoute} from "vue-router";
import Tag from "primevue/tag";
import SelectKnowledgeItem from "@/components/characters/character/wizard/knowledges/SelectKnowledgeItem.vue";
import EditCharacterKnowledge from "@/components/characters/character/wizard/knowledges/EditCharacterKnowledge.vue";
import ShowXPCosts from "@/components/characters/character/wizard/ShowXPCosts.vue";
import type {CharacterKnowledgeGroup, Knowledge, KnowledgeGroup} from "@/components/knowledges/types.ts";
import type {CharacterKnowledge} from "@/components/characters/character/knowledges/types.ts";
import type {WizardContent} from "@/components/characters/character/wizard/types.ts";
import {wizardContentStore} from "@/components/characters/character/wizard/stores/wizardContentStore.ts";

const wizardContentInfo = wizardContentStore();
const knowledgeData = knowledgeStore();
const characterKnowledgeData = characterKnowledgeStore();
const route = useRoute();
const availableKnowledgeGroups = ref<Array<KnowledgeGroup>>([]);
const selectedKnowledgeGroups = ref<CharacterKnowledgeGroup[]>([]);

onBeforeMount(async () => {
  await knowledgeData.getKnowledges();
  await characterKnowledgeData.getCharacterKnowledges(route.params.id)
})

watch(() => characterKnowledgeData.knowledges, () => {
  populateAvailableKnowledges();
  populateSelectedKnowledges()
});

function populateAvailableKnowledges(){

  const existingKnowledgeIds = characterKnowledgeData.knowledges.map(ck => ck.knowledge.id);
  let selectedKnowledges =  knowledgeData.knowledges.filter(knowledge =>
      !existingKnowledgeIds.includes(knowledge.id)
  ).sort((x, y) => x.name.localeCompare(y.name)) as Array<Knowledge>;
  availableKnowledgeGroups.value = [];
  const typeNames = [...new Set(selectedKnowledges.map(i => i.typeName))].sort((x, y) => x.localeCompare(y));
  typeNames.forEach(type => {
    availableKnowledgeGroups.value.push({
      name: type,
      knowledges: selectedKnowledges.filter(i => i.typeName == type)
    } as KnowledgeGroup)
  })
}

function populateSelectedKnowledges(){

  let selectedKnowledges =  characterKnowledgeData.knowledges
      .sort((x, y) => x.knowledge.name.localeCompare(y.knowledge.name)) as Array<CharacterKnowledge>;

  selectedKnowledgeGroups.value = [];
  const typeNames = [...new Set(selectedKnowledges.map(i => i.knowledge.type))].sort((x, y) => x.localeCompare(y));
  typeNames.forEach(type => {
    selectedKnowledgeGroups.value.push({
      name: type,
      knowledges: selectedKnowledges.filter(i => i.knowledge.type == type)
    } as CharacterKnowledgeGroup)
  })
}


const toggleEdit = (knowledge) => {
  wizardContentInfo.updateContent(
      {
        headerName: 'Knowledge',
        component: EditCharacterKnowledge,
        props: { knowledge: knowledge}
      } as WizardContent
  )
}

</script>

<template>
  <div style="max-width: 650px; margin: 0 auto;">
    <div v-if="characterKnowledgeData.knowledges.length === 0">
      <p>No Knowledges detected, please pick one below.</p>
    </div>
    <h1 v-if="characterKnowledgeData.knowledges.length > 0">Selected Knowledges</h1>
    <div v-for="knowledgeGroup in selectedKnowledgeGroups" :key="knowledgeGroup.name" class="mb-2">
      <h2 class="pb-0 mb-0">{{ knowledgeGroup.name }}</h2>
      <div class="pl-3" v-for="knowledge in knowledgeGroup.knowledges" :key="knowledge.id">
        <div class="d-flex flex-column flex-md-row align-self-center justify-content-between pt-2 pb-2">
          <div class="d-flex flex-column flex-grow-1 pr-3">
            <div class="d-flex flex-fill align-content-between d-block">
              <div class="flex-grow-1 font-bold text-900">
                {{ knowledge.knowledge.name }}
              </div>
              <div>
                {{ knowledge.levelName }} ({{ knowledge.level }})
              </div>
            </div>
            <div class="d-flex d-block mt-1">
              <div class="flex-grow-1">
                <Tag severity="info" v-if="knowledge.specializations.length == 0" value="No Specializations" />
                <Tag severity="info" v-for="special in knowledge.specializations" v-else class="mr-1" :value="special.name" />
              </div>
              <div>Stones: +{{ knowledge.stoneModifier }}</div>
            </div>
          </div>
          <div>
            <Button label="View" size="small" @click="toggleEdit(knowledge)" />
          </div>
        </div>
      </div>
    </div>
    
    <div class="mb-2">
      <hr v-if="characterKnowledgeData.knowledges.length !== 0">
      <h1 class="pb-0 mb-0">Choose Knowledges</h1>
      <ShowXPCosts xp-name-tag="Knowledge XP" />
      <div v-for="knowledgeGroup in availableKnowledgeGroups" :key="knowledgeGroup.name" class="mb-2">
        <h2 class="pb-0 mb-3">{{ knowledgeGroup.name }}</h2>
        <div v-for="knowledge in knowledgeGroup.knowledges" :key="knowledge.id" class="pl-3 pt-1 pb-1">
          <SelectKnowledgeItem :is-read-only="false" :knowledge="knowledge" />
        </div>
      </div>

    </div>
  </div>
</template>
