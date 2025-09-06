<script setup lang="ts">
import FormTextAreaWrapper from "@/FormWrappers/FormTextAreaWrapper.vue";
import Button from "primevue/button";
import {getValidationInstance} from "@/components/characters/character/knowledges/validations/knowledgeValidations";
import {characterKnowledgeStore} from "@/components/characters/character/knowledges/stores/characterKnowledgeStore";
import {useRoute} from "vue-router";
import {onBeforeMount, type PropType, ref} from "vue";
import FormListboxWrapper from "@/FormWrappers/FormListboxWrapper.vue";
import type {CharacterKnowledge, KnowledgeOptions} from "@/components/characters/character/knowledges/types";
import Message from "primevue/message";
import {addKnowledgeDialog} from "@/components/characters/character/knowledges/services/dialogs.ts";
import KnowledgeLevelDetail from "@/components/characters/character/wizard/knowledges/KnowledgeLevelDetail.vue";

const dialogService = addKnowledgeDialog();
const store = characterKnowledgeStore();
const form = getValidationInstance();
const route = useRoute();

const props = defineProps({
  knowledge: {
    type: Object as PropType<CharacterKnowledge>,
    required: true,
  },
  isReadOnly:{
    type: Boolean,
    required: false
  }
});

const isUnknownKnowledge = ref(props.knowledge.type === 'Unknown');

onBeforeMount(async () => {
  await store.getKnowledgeLevels(route.params.id);
  store.knowledgeLevels.forEach(function(level:KnowledgeOptions) {
    const xpCost = isUnknownKnowledge.value ? level.totalUnknownXpCost : level.totalGeneralXpCost;
    const isLowerLevel = level.id > props.knowledge.levelId
    level.disabled = xpCost > store.currentExperience && isLowerLevel;
  });
  form.setValues(props.knowledge);
})

const onSubmit = form.handleSubmit(async (values) => {
  await store.editKnowledge(values, route.params.id, props.knowledge.mappingId);
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
    {{ props.knowledge.knowledge.name }}
  </h1>
  <h3>{{ props.knowledge.knowledge.type }}</h3>
  <p>{{ props.knowledge.knowledge.description }}</p>
  <h3 class="text-right">
    Available Experience: {{ store.currentExperience }}
  </h3>
  <form @submit="onSubmit">

<!--    <DataTable v-model:selection="form.knowledgeLevel.field" :value="store.knowledgeLevels" selectionMode="single" dataKey="id">
      <Column field="name" header="Name"></Column>
      <Column field="level" header="Level"></Column>
      <Column field="totalGeneralXpCost" header="XP">
        <template #body="slotProps">
          {{slotProps.totalGeneralXpCost > getCurrentXpLevel(knowledge.levelId) ? "-" : "+"}} {{ Math.abs(slotProps.totalGeneralXpCost - getCurrentXpLevel(knowledge.levelId)) }}
        </template>
      </Column>
      <Column field="stoneModifier" header="Stones"></Column>
      <Column field="specializations" header="Specials"></Column>
    </DataTable>-->
    
    <FormListboxWrapper v-model="form.knowledgeLevel" :options="store.knowledgeLevels" option-value="id" option-disabled="disabled" scroll-height="10000px">
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
      <Button label="Update" class="m-2" type="submit" />
    </div>
  </form>

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

      <div class="p-0 m-0 d-flex justify-content-between">
        <Button label="Delete" severity="danger" @click="popupService.deleteSpecializationConfirmation($event, knowledge.mappingId, special.id)" />
        <Button label="Edit" @click="dialogService.showEditSpecialization(knowledge, special)" />
      </div>
    </div>
  </div>
  <div class="text-right mt-2">
    <Button v-if="knowledge.specializationCount > knowledge.specializations.length" class="btn btn-primary text-right" label="Add Specialization" @click="dialogService.showAddSpecialization(knowledge)" />
  </div>
</template>
