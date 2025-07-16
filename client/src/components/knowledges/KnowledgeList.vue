<script setup lang="ts">

import {knowledgeStore} from "@/components/knowledges/stores/knowledgeStore";
import {onMounted, ref} from "vue";
import KnowledgeItem from "@/components/knowledges/KnowledgeItem.vue";
import {UserRoles, userStore} from "@/stores/userStore";
import Button from "primevue/button";
import AddKnowledge from "@/components/knowledges/AddKnowledge.vue";

const store = knowledgeStore();
const userInfo = userStore();

onMounted(async () => {
  await store.getKnowledges()
})

const showAdd = ref(false);

const toggleAdd = () =>{
  showAdd.value = !showAdd.value;
}

const props = defineProps({
  isReadOnly: {
    type: Boolean,
    required: true
  }
});

</script>

<template>
  <div v-for="knowledge in store.knowledges" :key="knowledge.id">
    <KnowledgeItem :knowledge="knowledge" :is-read-only="props.isReadOnly" />
  </div>
  
  <AddKnowledge v-if="showAdd && userInfo.hasUserRole(UserRoles.KnowledgeManagementRole) && !props.isReadOnly" @canceled="toggleAdd" />
  <Button
    v-if="!showAdd && userInfo.hasUserRole(UserRoles.KnowledgeManagementRole) && !props.isReadOnly" class="w-100 m-2"
    label="Add Knowledge" @click="toggleAdd"
  />
</template>
