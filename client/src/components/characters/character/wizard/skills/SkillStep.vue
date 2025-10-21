<script setup lang="ts">

import Panel from 'primevue/panel'
import {computed, onMounted} from 'vue'
import {useRoute} from 'vue-router'
import type {
  CharacterSkillsResponse,
} from '@/components/characters/character/skills/interfaces/CharacterSkillsResponse'
import {skillStore} from '@/components/characters/character/skills/Stores/skillStore'
import SkeletonWrapper from '@/FormWrappers/SkeletonWrapper.vue'
import Column from 'primevue/column'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import EditSkillDetail from '@/components/characters/character/wizard/skills/supporting/EditSkillDetail.vue'
import ShowXPCosts from '@/components/characters/character/wizard/ShowXPCosts.vue'
import {wizardContentStore} from '@/components/characters/character/wizard/stores/wizardContentStore.ts'
import type {WizardContent} from '@/components/characters/character/wizard/types.ts'
import {breakpointsBootstrapV5, useBreakpoints} from '@vueuse/core'
import {XpSectionTypes} from '@/components/characters/character/stores/experienceBreakdownStore.ts'

const route = useRoute()
const skillData = skillStore()
const skillInfo = skillStore()
const activeBreakpoint = useBreakpoints(breakpointsBootstrapV5)
const isMobile = activeBreakpoint.smaller('md')

const skillTypes = computed(() => {
  return [
    { name: 'Offensive Skills', skills: skillData.offensiveSkills },
    { name: 'Defensive Skills', skills: skillData.defensiveSkills },
  ]
})

onMounted(async () => {
  await getEditOptions()
})

async function getEditOptions() {
  await skillData.getSkills(route.params.id)
}

const wizardContentInfo = wizardContentStore()
const updateWizardContent = (skill: CharacterSkillsResponse) => {
  wizardContentInfo.updateContent(
    {
      headerName: 'Edit Skill',
      component: EditSkillDetail,
      props: { skill: skill },
    } as WizardContent,
  )
}

</script>

<template>
  <ShowXPCosts :section-type="XpSectionTypes.skills" />
  <div>
    <Panel v-for="skillType in skillTypes" class="mb-3 flex-shrink-1">
      <template #header>
        <div class="row">
          <h3 class="col pb-0 mb-0 mt-0 pt-0">
            {{ skillType.name }}
          </h3>
          <div v-if="skillInfo.showExperience" class="col text-right">
            Infinite EXP
          </div>
        </div>
      </template>
      <DataTable :value="skillType.skills" data-key="statTypeId">
        <Column v-if="isMobile">
          <template #body="slotProps">
            <Button class="float-end " size="small" label="View" @click="updateWizardContent(slotProps.data)" />
          </template>
        </Column>
        <Column field="name" header="Name">
          <template #body="slotProps">
            <SkeletonWrapper height="1.5rem" width="2rem" :show-skeleton="skillData.isLoadingSkills">
              {{ slotProps.data.name }}
            </SkeletonWrapper>
          </template>
        </Column>
        <Column field="level" header="Name" header-class="text-center" body-class="text-center">
          <template #body="slotProps">
            <SkeletonWrapper height="1.5rem" width="2rem" :show-skeleton="skillData.isLoadingSkills">
              {{ slotProps.data.levelName }}
            </SkeletonWrapper>
          </template>
        </Column>
        <Column field="bonus" header="Level" header-class="text-center" body-class="text-center">
          <template #body="slotProps">
            <SkeletonWrapper height="1.5rem" width="2rem" :show-skeleton="skillData.isLoadingSkills">
              {{ slotProps.data.levelNumber }}
            </SkeletonWrapper>
          </template>
        </Column>
        <Column v-if="!isMobile">
          <template #body="slotProps">
            <Button class="float-end " size="small" label="View" @click="updateWizardContent(slotProps.data)" />
          </template>
        </Column>
      </DataTable>
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
