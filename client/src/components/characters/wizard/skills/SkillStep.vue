<script setup lang="ts">

import Panel from 'primevue/panel'
import { computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import type {
  CharacterSkillsResponse,
} from '@/components/characters/character/skills/interfaces/CharacterSkillsResponse'
import { skillStore } from '@/components/characters/character/skills/Stores/skillStore'
import SkeletonWrapper from '@/FormWrappers/SkeletonWrapper.vue'
import Column from 'primevue/column'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import EditSkillDetail from '@/components/characters/wizard/skills/supporting/EditSkillDetail.vue'
import ShowXPCosts from '@/components/characters/wizard/ShowXPCosts.vue'
import { wizardContentStore } from '@/components/characters/wizard/stores/wizardContentStore.ts'
import type { WizardContent } from '@/components/characters/wizard/types.ts'
import { breakpointsBootstrapV5, useBreakpoints } from '@vueuse/core'
import { XpSectionTypes } from '@/components/characters/character/stores/experienceBreakdownStore.ts'
import ProficiencyTableTile from '@/components/characters/character/proficiency/ProficiencyTableTile.vue'

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
  <h2>Skills</h2>
  <p>
    Skills affect your proficiencies for attacking and defending, as well as having secondary effects with many powers.
  </p>
  <p>
    Each skill starts at level 0 untrained. A skill level of 3 or 4 will provide additional bonuses, see the full
    skills section for details.
  </p>
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
        <Column field="name" header="Name">
          <template #body="slotProps">
            <SkeletonWrapper height="1.5rem" width="2rem" :show-skeleton="skillData.isLoadingSkills">
              <div>
                {{ slotProps.data.name }}
              </div>
              <div>{{ slotProps.data.levelName }} ({{ slotProps.data.levelNumber }})</div>
            </SkeletonWrapper>
          </template>
        </Column>
        <Column>
          <template #body="slotProps">
            <Button class="float-end " size="small" label="View" @click="updateWizardContent(slotProps.data)" />
          </template>
        </Column>
      </DataTable>
    </Panel>
  </div>
  <div>
    <h3>Proficiencies</h3>
    <p>
      Proficiencies are the aspect of your character that you will be using most often. Proficiencies are used when
      characters are interacting with and affecting each other. Each proficiency consists of two of your character’s
      statistics added together.
    </p>
    <p>
      There are two kinds of proficiencies: Offensive and defensive. Every offensive proficiency
      has a default matching defensive proficiency.
    </p>
    <p>
      How you spend your points to customize your statistics and skills when you first make your character will have a major effect
      upon how well your character can do different things.
    </p>
    <p>
      Click on each one below to get more details on how they are calculated
    </p>
    <ProficiencyTableTile />
  </div>
</template>

<style>
 .p-panel-header{
   background: var(--p-panel-background) !important;
   border-bottom: 0px !important;
   padding: 1.5em 1.5em 0em !important;
 }
</style>
