<script setup lang="ts">

import {type PropType} from "vue";
import type {Knowledge} from "@/components/knowledges/types";
import Button from "primevue/button";
import {addKnowledgeDialog} from "@/components/characters/character/knowledges/services/dialogs";

const addDialog = addKnowledgeDialog();

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

</script>

<template>
  <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
    <div>
      <h1 class="p-0 m-0">
        {{ props.knowledge.name }}
      </h1>
      <div class="p-0 m-0">
        {{ props.knowledge.typeName }}
      </div>
    </div>
    <div
      v-if="!props.isReadOnly"
      class="p-0 m-0 d-inline-flex align-items-start"
    >
      <Button class="float-end" label="Pick" @click="addDialog.showAddCharacter(props.knowledge)" />
    </div>
  </div>
  <p>{{ props.knowledge.description }}</p>
</template>
