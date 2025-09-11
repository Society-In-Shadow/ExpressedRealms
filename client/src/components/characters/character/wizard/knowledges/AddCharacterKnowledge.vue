<script setup lang="ts">

import FormTextAreaWrapper from "@/FormWrappers/FormTextAreaWrapper.vue";
import Button from "primevue/button";
import {getValidationInstance} from "@/components/characters/character/knowledges/validations/knowledgeValidations";
import {characterKnowledgeStore} from "@/components/characters/character/knowledges/stores/characterKnowledgeStore";
import {useRoute} from "vue-router";
import {onBeforeMount, type PropType, ref} from "vue";
import type {KnowledgeOptions} from "@/components/characters/character/knowledges/types";
import Message from "primevue/message";
import type {Knowledge} from "@/components/knowledges/types.ts";
import Column from "primevue/column";
import DataTable from "primevue/datatable";

const store = characterKnowledgeStore();
const form = getValidationInstance();
const route = useRoute();


const props = defineProps({
  knowledge: {
    type: Object as PropType<Knowledge>,
    required: true,
  },
  isReadOnly:{
    type: Boolean,
    required: false
  }
});

const isUnknownKnowledge = ref(props.knowledge.typeName === 'Unknown');


onBeforeMount(async () => {
  await store.getKnowledgeLevels(route.params.id);
  store.knowledgeLevels.forEach(function(level:KnowledgeOptions) {
    const xpCost = isUnknownKnowledge.value ? level.totalUnknownXpCost : level.totalGeneralXpCost;
    level.disabled = xpCost > store.currentExperience;
  });
})

const onSubmit = form.handleSubmit(async (values) => {
  await store.addKnowledge(values, route.params.id, props.knowledge.id);
});

</script>

<template>
  <h1 class="pt-0 mt-0">
    {{ props.knowledge.name }}
  </h1>
  <h3>{{ props.knowledge.typeName }}</h3>
  <p>{{ props.knowledge.description }}</p>
  <h3 class="text-right">
    Available Experience: {{ store.currentExperience }}
  </h3>
  <form @submit="onSubmit">
    <DataTable v-model:selection="form.knowledgeLevel2.field.value" selection-mode="single" :value="store.knowledgeLevels" dataKey="id">
      <Column selection-mode="single"  headerStyle="width: 3rem"></Column>
      <Column field="name" header="Name"></Column>
      <Column field="totalGeneralXpCost" header="XP" header-class="text-center" body-class="text-center" >
        <template #body="slotProps">
          -{{ slotProps.data.totalGeneralXpCost }}
        </template>
      </Column>
      <Column field="stoneModifier" header="Stones" header-class="text-center" body-class="text-center" ></Column>
      <Column field="specializationCount" header="Specials" header-class="text-center" body-class="text-center" ></Column>
    </DataTable>

    <Message v-if="form.knowledgeLevel2.field.value  && form.knowledgeLevel2.field.value.id == 8" severity="warn" class="mt-4">
      <p>
        Gaining the doctorate of knowledge also requires the completion of a quest of some kind. The quest can be as
        straightforward as finding lost or unknown relics that relate to the subject or as complicated as a life-long
        journey to discover new theories and paradigms of the knowledge. In either case, the quest should have some
        bearing on the field of the knowledge.
      </p>
      <p>
        Selecting this will require interaction with a GO to get the quest approved.  Use the notes field below to
        keep track of your ideas, and anything you may have discussed with your GO.
      </p>
    </Message>

    <div class="mt-4">
      <FormTextAreaWrapper v-model="form.notes"/>
    </div>

    <div class="m-3 text-right">
      <Button label="Add" class="m-2" type="submit" />
    </div>
  </form>
</template>

<style>
:deep(th.text-center .p-datatable-column-header-content) {
  justify-content: center;
}

</style>