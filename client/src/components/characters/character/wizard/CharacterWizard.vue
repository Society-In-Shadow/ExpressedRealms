<script setup lang="ts">
import Button from 'primevue/button';
import Card from 'primevue/card';
import {computed, defineAsyncComponent, h, onBeforeMount, ref, watch} from "vue";
import KnowledgeStep from "@/components/characters/character/wizard/knowledges/KnowledgeStep.vue";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import PowerStep from "@/components/characters/character/wizard/powers/PowerStep.vue";
import StatStep from "@/components/characters/character/wizard/stats/StatStep.vue";
import SkillStep from "@/components/characters/character/wizard/skills/SkillStep.vue";
import ProficiencyTableTile from "@/components/characters/character/proficiency/ProficiencyTableTile.vue";
import {useRoute, useRouter} from "vue-router";
import DataTable from "primevue/datatable";
import SecondaryProficiencies from "@/components/characters/character/wizard/proficiencies/SecondaryProficiencies.vue";
import EditCharacterDetails from "@/components/characters/character/wizard/basicInfo/EditCharacterDetails.vue";
import AddCharacter from "@/components/characters/character/wizard/basicInfo/AddCharacter.vue";
import OverallExperience from "@/components/characters/character/OverallExperience.vue";
import BlessingStep from "@/components/characters/character/wizard/blessings/BlessingStep.vue";
import {FeatureFlags, userStore} from "@/stores/userStore.ts";
import WizardContent from "@/components/characters/character/wizard/WizardContent.vue";

const xpData = experienceStore();
const route = useRoute()
const router = useRouter();
const userInfo = userStore();
const isAdd = computed(() =>route.name == 'addWizard');

const sections = ref([
  { name: 'Getting Started', isDisabled: false, component: createPlaceholderView('Getting Started', 'Getting Started content coming soon...') },
  { name: 'Stats', isDisabled: isAdd, component: defineAsyncComponent(async () => StatStep) },
  { name: 'Knowledges', isDisabled: isAdd, component: defineAsyncComponent(async () => KnowledgeStep)},
  { name: 'Powers', isDisabled: isAdd, component: defineAsyncComponent(async () => PowerStep) },
  { name: 'Skills', isDisabled: isAdd, component: defineAsyncComponent(async () => SkillStep) },
  { name: 'Proficiencies', isDisabled: isAdd, component: defineAsyncComponent(async () => ProficiencyTableTile) },
  { name: 'Experience Breakdown', isDisabled: isAdd, component: defineAsyncComponent(async () => OverallExperience) },
]);

onBeforeMount(async () => {
  await fetchData()
})

async function fetchData() {
  if(sections.value[1].name == 'Basic Info') sections.value.splice(1, 1);
  if(isAdd.value){
    sections.value.splice(1, 0,   { name: 'Basic Info', isDisabled: false, component: defineAsyncComponent(async () => AddCharacter) },);
  }else{
    sections.value.splice(1, 0,   { name: 'Basic Info', isDisabled: false, component: defineAsyncComponent(async () => EditCharacterDetails) },);
    await xpData.updateExperience(route.params.id);
  }
  
  if(await userInfo.hasFeatureFlag(FeatureFlags.ManageCharacterBlessings)){
    sections.value.splice(sections.value.length - 1, 0, { name: 'Advantages / Disadvantages', isDisabled: isAdd, component: defineAsyncComponent(async () => BlessingStep) })
  }
}

watch(
    () => route.path,
    async (newPath, oldPath) => {
      if (newPath !== oldPath) {
        await fetchData()
      }
    }
)

function createPlaceholderView(name: string, text: string) : Promise<any> {
  return defineAsyncComponent(async () => ({
    name: 'PlaceholderView',
    setup() {
      return () => h('div', { class: 'p-3' }, text);
    },
  }));
}

const selectedSection = ref<string>('Basic Info');
const currentView = computed(() => sections.value.filter(x => x.name == selectedSection.value)[0].component);
const selectSection = (name: string) => {
  selectedSection.value = name;
};

const redirectToEdit = () => {
  router.push({name: 'characterSheet', params: {id: route.params.id}})
}
</script>

<template>
  <div class="d-none">
    <DataTable />
  </div>
  <div class="d-flex justify-content-end" v-if="!isAdd">
    <SecondaryProficiencies></SecondaryProficiencies>
  </div>
  <div class="row pt-3">
    <div class="col col-md-2 custom-toc">
      <Card>
        <template #content>
          <div v-for="section in sections" class="text-right p-2">
            <Button class="w-100" :label="section.name"
                    :outlined="selectedSection !== section.name"
                    @click="selectSection(section.name)"
                    :disabled="section.isDisabled"
            />
          </div>
          <div class="p-2" v-if="!isAdd">
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
    <div class="col custom-toc col-12 col-md">
      <WizardContent></WizardContent>
      <div id="item-modification-section"></div>
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