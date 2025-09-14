<script setup lang="ts">

import {type PropType} from "vue";
import type {Knowledge} from "@/components/knowledges/types";
import Button from "primevue/button";
import AddCharacterKnowledge from "@/components/characters/character/wizard/knowledges/AddCharacterKnowledge.vue";
import {characterKnowledgeStore} from "@/components/characters/character/knowledges/stores/characterKnowledgeStore.ts";

const characterKnowledgeData = characterKnowledgeStore();

const props = defineProps({
  knowledge: {
    type: Object as PropType<Knowledge>,
    required: true,
  },
  isReadOnly: {
    type: Boolean,
    required: true
  }
});

const toggleAdd = () => {
  characterKnowledgeData.activeKnowledgeId = props.knowledge.id;
}

</script>

<template>
  <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
    <div>
      <h2 class="p-0 m-0">
        {{ props.knowledge.name }}
      </h2>
    </div>
    <div v-if="!props.isReadOnly" class="p-0 m-2 d-inline-flex align-items-start align-items-center">
      <Button class="float-end" size="small" label="View" @click="toggleAdd" />
    </div>
  </div>
  <Teleport v-if="characterKnowledgeData.activeKnowledgeId == props.knowledge?.id" to="#item-modification-section">
    <AddCharacterKnowledge :knowledge="props.knowledge" />
  </Teleport>
</template>
