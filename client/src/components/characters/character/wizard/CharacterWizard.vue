<script setup lang="ts">
import Button from 'primevue/button';
import Card from 'primevue/card';
import {computed, defineAsyncComponent, h, onMounted, ref} from "vue";
import KnowledgeStep from "@/components/characters/character/wizard/knowledges/KnowledgeStep.vue";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import PowerStep from "@/components/characters/character/wizard/powers/PowerStep.vue";
import StatStep from "@/components/characters/character/wizard/stats/StatStep.vue";
import SkillStep from "@/components/characters/character/wizard/skills/SkillStep.vue";
import {useRoute, useRouter} from "vue-router";
import DataTable from "primevue/datatable";

const xpData = experienceStore();
const route = useRoute()
const router = useRouter();

const sections = ref([
  { name: 'Getting Started', component: createPlaceholderView('Getting Started', 'Getting Started content coming soon...') },
  { name: 'Expression', component: createPlaceholderView('Expression', 'Expression content coming soon...') },
  { name: 'Stats', component: defineAsyncComponent(async () => StatStep) },
  { name: 'Knowledges', component: defineAsyncComponent(async () => KnowledgeStep)},
  { name: 'Powers', component: defineAsyncComponent(async () => PowerStep) },
  { name: 'Skills', component: defineAsyncComponent(async () => SkillStep) },
  { name: 'Proficiencies', component: createPlaceholderView('Proficiencies', 'Proficiencies content coming soon...') },
]);

onMounted(() => {
  xpData.updateExperience(route.params.id);
})

function createPlaceholderView(name: string, text: string) : Promise<any> {
  return defineAsyncComponent(async () => ({
    name: 'PlaceholderView',
    setup() {
      return () => h('div', { class: 'p-3' }, text);
    },
  }));
}

const selectedSection = ref<string>('Knowledges');
const currentView = computed(() => sections.value.filter(x => x.name == selectedSection.value)[0].component);
const selectSection = (name: string) => {
  selectedSection.value = name;
};

const redirectToEdit = () => {
  router.push({name: 'editCharacter', params: {id: route.params.id}})
}
</script>

<template>
  <div class="d-none">
    <DataTable />
  </div>
  <div class="row">
    
  </div>
  <div class="row pt-3">
    <div class="col col-md-2 custom-toc">
      <Card>
        <template #content>
          <div v-for="section in sections" class="text-right p-2">
            <Button class="w-100" :label="section.name"
                    :outlined="selectedSection !== section.name"
                    @click="selectSection(section.name)"
            />
          </div>
          <div class="p-2">
            <Button class="w-100" label="Character Sheet" :outlined="true" icon="pi pi-arrow-left"
                    @click="redirectToEdit"
            />
          </div>
        </template>
      </Card>
    </div>
    <div class="col custom-toc">
      <Card>
        <template #content>
          <!-- Dynamically load the chosen section in the middle column -->
          <component :is="currentView"/>
        </template>
      </Card>
    </div>
    <div class="col custom-toc">
      <Card>
        <template #content>
          <div id="item-modification-section"></div>
        </template>
      </Card>
    </div>
  </div>

</template>

<style>
@media(min-width: 768px){
  .custom-toc {
    max-height: calc(100vh - 1rem);
    overflow-y: auto;
    height:100%;
    min-width: 18em;
  }
}

@media(max-width: 768px){
  .custom-toc > .p-card-body{
    padding-left: 1rem !important;
    padding-right: 1rem !important;
  }
}

.main-container{
  max-width: 1500px !important;
}
</style>