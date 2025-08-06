<script setup lang="ts">

import FormTextAreaWrapper from "@/FormWrappers/FormTextAreaWrapper.vue";
import Button from "primevue/button";
import {getValidationInstance} from "@/components/characters/character/knowledges/validations/knowledgeValidations";
import {characterKnowledgeStore} from "@/components/characters/character/knowledges/stores/characterKnowledgeStore";
import KnowledgeLevelDetail from "@/components/characters/character/knowledges/KnowledgeLevelDetail.vue";
import {useRoute} from "vue-router";
import {inject, onBeforeMount, ref} from "vue";
import FormListboxWrapper from "@/FormWrappers/FormListboxWrapper.vue";
import type {CharacterKnowledge, KnowledgeOptions} from "@/components/characters/character/knowledges/types";
import Message from "primevue/message";

const store = characterKnowledgeStore();
const form = getValidationInstance();
const route = useRoute();

const dialogRef = inject('dialogRef');

const knowledge = ref<CharacterKnowledge>(dialogRef.value.data.knowledge);
const isReadOnly = ref(dialogRef.value.data.isReadOnly);
const isUnknownKnowledge = ref(knowledge.value.knowledge.type === 'Unknown');
const closeDialog = () => {
  dialogRef.value.close();
}

onBeforeMount(async () => {
  await store.getKnowledgeLevels(route.params.id);
  store.knowledgeLevels.forEach(function(level:KnowledgeOptions) {
    const xpCost = isUnknownKnowledge.value ? level.totalUnknownXpCost : level.totalGeneralXpCost;
    const isLowerLevel = level.id > knowledge.value.levelId
    level.disabled = xpCost > store.currentExperience && isLowerLevel;
  });
  form.setValues(knowledge.value);
})

const onSubmit = form.handleSubmit(async (values) => {
  await store.editKnowledge(values, route.params.id, knowledge.value.mappingId);
  closeDialog()
});

function getCurrentXpLevel(levelId: number){
  let level = store.knowledgeLevels.filter(function(level:KnowledgeOptions) {
    return level.id === levelId;
  })[0]
  
  if(isUnknownKnowledge.value){
    return level.totalUnknownXpCost;
  }
  return level.totalGeneralXpCost;
}

</script>

<template>
  <h1 class="pt-0 mt-0">
    {{ knowledge.knowledge.name }}
  </h1>
  <h3>{{ knowledge.knowledge.type }}</h3>
  <p>{{ knowledge.knowledge.description }}</p>
  <h3 class="text-right">
    Available Experience: {{ store.currentExperience }}
  </h3>
  <form @submit="onSubmit">
    <FormListboxWrapper v-model="form.knowledgeLevel" :options="store.knowledgeLevels" option-value="id" option-disabled="disabled">
      <template #option="slotProps">
        <KnowledgeLevelDetail
          :is-loading="store.isLoadingLevels" :selected-item="slotProps.option"
          :current-xp-level="getCurrentXpLevel(knowledge.levelId)" :is-unknown-knowledge="isUnknownKnowledge"
          :is-read-only="isReadOnly"
        />
      </template>
    </FormListboxWrapper>

    <Message v-if="form.knowledgeLevel.field == 8" severity="warn">
      <p>
        Gaining the seventh level of knowledge also requires the completion of a quest of some kind. The quest can be as
        straightforward as finding lost or unknown relics that relate to the subject or as complicated as a life-long
        journey to discover new theories and paradigms of the knowledge. In either case, the quest should have some
        bearing on the field of the knowledge.
      </p>
      <p>
        Selecting this will require interaction with a GO to get the quest approved.  Use the notes field below to 
        keep track of your ideas, and anything you may have discussed with your GO.
      </p>
    </Message>
    
    <FormTextAreaWrapper v-model="form.notes" />

    <div class="m-3 text-right">
      <Button label="Cancel" class="m-2" type="reset" @click="closeDialog()" />
      <Button label="Update" class="m-2" type="submit" />
    </div>
  </form>
</template>
