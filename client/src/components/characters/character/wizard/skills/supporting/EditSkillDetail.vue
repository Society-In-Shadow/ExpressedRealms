<script setup lang="ts">

import {onBeforeMount, type PropType, ref, type Ref, watch} from 'vue'
import axios from 'axios'
import type {SkillResponse} from '@/components/characters/character/skills/interfaces/SkillOptionsResponse'
import {useRoute} from 'vue-router'
import toasters from '@/services/Toasters'
import {skillStore} from '@/components/characters/character/skills/Stores/skillStore'
import {proficiencyStore} from '@/components/characters/character/proficiency/stores/proficiencyStore'
import {experienceStore, XpSectionTypes} from '@/components/characters/character/stores/experienceBreakdownStore.ts'
import type {
  CharacterSkillsResponse,
} from '@/components/characters/character/skills/interfaces/CharacterSkillsResponse.ts'
import SkeletonWrapper from '@/FormWrappers/SkeletonWrapper.vue'
import Column from 'primevue/column'
import DataTable from 'primevue/datatable'
import Button from 'primevue/button'
import ShowXPCosts from '@/components/characters/character/wizard/ShowXPCosts.vue'

const props = defineProps({
  skill: {
    type: Object as PropType<CharacterSkillsResponse>,
    required: true,
  },
})

const route = useRoute()
const experienceInfo = experienceStore()
const profStore = proficiencyStore()
const skillInfo = skillStore()

const emptySkill: SkillResponse = {
  skillTypeId: 0,
  name: '',
  description: '',
  levelId: 0,
  levelNumber: 0,
  benefits: [],
  experienceCost: 0,
  disabled: false,
}

const skillLevel: Ref<SkillResponse> = ref(emptySkill)
const expandedRows = ref({})
const selectedSkillItem: Ref<SkillResponse> = ref(emptySkill)

watch(() => props.skill, async (newValue, oldValue) => {
  await getEditOptions()
})

onBeforeMount(async () => {
  await getEditOptions()
})

async function getEditOptions() {
  await skillInfo.getSkillOptions(route.params.id, props.skill?.skillTypeId)
  const info = experienceInfo.getExperienceInfoForSection(XpSectionTypes.skills)
  selectedSkillItem.value = skillInfo.skillLevels.find(x => x.levelId === props.skill.levelId)
  skillLevel.value = skillInfo.skillLevels.find(x => x.levelId === props.skill.levelId)

  skillInfo.skillLevels.forEach(function (level: SkillResponse) {
    const levelInfo = skillInfo.skillLevels.find(x => x.levelId === level.levelId)
    level.disabled = levelInfo.experienceCost - skillLevel.value.experienceCost > info.availableXp
    return level
  })
  expandedRows.value = Object.fromEntries(skillInfo.skillLevels.map(x => [x.levelId, true]))
}

function handleStatUpdate(skill: SkillResponse) {
  // Don't allow them to unselect the option
  if (selectedSkillItem.value == undefined) {
    selectedSkillItem.value = skillInfo.skillLevels.find(x => x.levelId === props.skill.levelId)
    return
  }

  axios.put(`/characters/${route.params.id}/skill/${selectedSkillItem.value.skillTypeId}`, {
    characterId: route.params.id,
    skillTypeId: props.skill.skillTypeId,
    skillLevelId: selectedSkillItem.value.levelId,
  }).then(async function () {
    props.skill.levelId = selectedSkillItem.value.levelId
    await skillInfo.getSkills(route.params.id)
    await experienceInfo.updateExperience(route.params.id)
    await profStore.getUpdateProficiencies(route.params.id)
    await getEditOptions()
    toasters.success('Successfully updated level!')
  })
}

// Prevent deselecting the only option
const onRowUnselect = (event) => {
  selectedSkillItem.value = skillInfo.skillLevels.find(x => x.levelId === event.data.levelId)
}

</script>

<template>
  <div class="row">
    <div class="col">
      <h3 class="mt-0">
        <SkeletonWrapper :show-skeleton="skillInfo.isLoadingSkillLevels" height="2rem">
          <div class="row">
            <div class="col">
              {{ props.skill.name }}
            </div>
            <div class="col text-right">
              <Button label="Update" size="small" @click="handleStatUpdate(skillLevel)" />
            </div>
          </div>
        </SkeletonWrapper>
      </h3>
      <div class="mb-3">
        <SkeletonWrapper :show-skeleton="skillInfo.isLoadingSkillLevels" height="3rem">
          {{ props.skill.description }}
        </SkeletonWrapper>
      </div>
    </div>
  </div>
  <ShowXPCosts :section-type="XpSectionTypes.skills" />
  <DataTable
    v-model:selection="selectedSkillItem" v-model:expanded-rows="expandedRows" :value="skillInfo.skillLevels" selection-mode="single" data-key="levelId"
    :row-class="row => (row.disabled ? 'non-selectable' : '')" @row-unselect="onRowUnselect"
  >
    <Column selection-mode="single" header-style="width: 3rem" />
    <Column field="level" header="Name">
      <template #body="slotProps">
        <SkeletonWrapper :show-skeleton="skillInfo.isLoadingSkillLevels" height="2rem" width="100%" header-class="text-center" body-class="text-center">
          {{ slotProps.data.name }}
        </SkeletonWrapper>
      </template>
    </Column>
    <Column header="Level">
      <template #body="slotProps">
        <SkeletonWrapper :show-skeleton="skillInfo.isLoadingSkillLevels" height="2rem" width="100%" header-class="text-center" body-class="text-center">
          {{ slotProps.data.levelNumber }}
        </SkeletonWrapper>
      </template>
    </Column>
    <Column field="xp" header="XP" header-class="text-center" body-class="text-center">
      <template #body="slotProps">
        <SkeletonWrapper :show-skeleton="skillInfo.isLoadingSkillLevels" height="2rem" width="100%">
          {{ slotProps.data.experienceCost > skillLevel.experienceCost ? "-" : "+" }}{{ Math.abs(slotProps.data.experienceCost - skillLevel.experienceCost) }}
        </SkeletonWrapper>
      </template>
    </Column>
    <template #expansion="slotProps">
      <div :class="slotProps.data.disabled ? 'non-selectable' : ''">
        <SkeletonWrapper :show-skeleton="skillInfo.isLoadingSkillLevels" height="2rem" width="100%" style="padding-left: 3rem; cursor: pointer;" @click="selectedSkillItem = slotProps.data">
          <div>
            {{ slotProps.data.description }}
          </div>
          <div v-if="slotProps.data.benefits && slotProps.data.benefits.length > 0">
            <h3 class="pb-1 mb-0">
              Benefits
            </h3>
            <div v-for="benefit in slotProps.data.benefits">
              <div> +{{ benefit.modifier }} {{ benefit.name }}</div>
            </div>
          </div>
        </SkeletonWrapper>
      </div>
    </template>
  </DataTable>
</template>

<style>
.non-selectable { opacity:.6; pointer-events:none; }
</style>
