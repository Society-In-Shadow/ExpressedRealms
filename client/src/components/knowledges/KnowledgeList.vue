<script setup lang="ts">

import {knowledgeStore} from "@/components/knowledges/stores/knowledgeStore";
import {onMounted} from "vue";
import KnowledgeItem from "@/components/knowledges/KnowledgeItem.vue";

const store = knowledgeStore();

onMounted(async () => {
  await store.getKnowledges()
})

</script>

<template>

  <div v-for="knowledge in store.knowledges" :key="knowledge.id">
    <KnowledgeItem :knowledge="knowledge" />
  </div>
  
</template>
