<script setup lang="ts">

import axios from 'axios'
import Panel from 'primevue/panel'
import {onMounted, ref, type Ref} from 'vue'
import {useRoute} from 'vue-router'
import Accordion from 'primevue/accordion'
import AccordionPanel from 'primevue/accordionpanel'
import AccordionHeader from 'primevue/accordionheader'
import AccordionContent from 'primevue/accordioncontent'

import type {
  CharacterSkillsResponse,
} from '@/components/characters/character/skills/interfaces/CharacterSkillsResponse'
import SkillDetail from '@/components/characters/character/skills/SkillDetail.vue'

const route = useRoute()

const offensiveSkills: Ref<Array<CharacterSkillsResponse>> = ref([])
const defensiveSkills: Ref<Array<CharacterSkillsResponse>> = ref([])
const openItems = ref([])

const skillTypes = ref([
  { name: 'Offensive Skills', skills: offensiveSkills },
  { name: 'Defensive Skills', skills: defensiveSkills },
])

onMounted(async () => {
  getEditOptions()
})

function getEditOptions() {
  axios.get(`characters/${route.params.id}/skills`)
    .then((response) => {
      offensiveSkills.value = response.data.filter((x: CharacterSkillsResponse) => x.skillSubTypeId === 1)
      defensiveSkills.value = response.data.filter((x: CharacterSkillsResponse) => x.skillSubTypeId === 2)
    })
}

</script>

<template>
  <div class="d-inline-flex flex-wrap justify-content-center column-gap-3 row-gap-1 w-100">
    <Panel v-for="skillType in skillTypes" class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" style="width: 25em">
      <template #header>
        <div class="row">
          <h3 class="col pb-0 mb-0 mt-0 pt-0">
            {{ skillType.name }}
          </h3>
        </div>
      </template>
      <Accordion :value="openItems" multiple :lazy="true" expand-icon="pi pi-info-circle" collapse-icon="pi pi-times-circle">
        <AccordionPanel v-for="skill in skillType.skills" :key="skill.name" :value="skill.skillTypeId">
          <AccordionHeader>
            <div class="d-flex justify-content-between w-100 pr-3 flex-column flex-md-row">
              <div>{{ skill.name }}</div>
              <div class="md:text-right mt-md-0 mt-2">
                {{ skill.levelName }} ({{ skill.levelNumber }})
              </div>
            </div>
          </AccordionHeader>
          <AccordionContent>
            <p class="m-0">
              {{ skill.description }}
            </p>
            <SkillDetail :skill-type-id="skill.skillTypeId" :selected-level-id="skill.levelId" />
          </AccordionContent>
        </AccordionPanel>
      </Accordion>
    </Panel>
  </div>
</template>

<style>
 .p-panel-header{
   background: var(--p-panel-background) !important;
   border-bottom: 0px !important;
   padding: 1.5em 1.5em 0em !important;
 }
</style>
