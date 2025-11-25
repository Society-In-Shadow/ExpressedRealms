<script setup lang="ts">

import { computed, onMounted, ref, watch } from 'vue'
import Message from 'primevue/message'
import { useRoute } from 'vue-router'
import {
  experienceStore,
  type XpSectionType,
  XpSectionTypes,
} from '@/components/characters/character/stores/experienceBreakdownStore.ts'
import Button from 'primevue/button'
import axios from 'axios'
import { characterStore } from '@/components/characters/character/stores/characterStore.ts'
import toaster from '@/services/Toasters'
import TabList from 'primevue/tablist'
import Tab from 'primevue/tab'
import TabPanel from 'primevue/tabpanel'
import TabPanels from 'primevue/tabpanels'
import Tabs from 'primevue/tabs'
import AssignedXpPanel from '@/components/characters/character/xp/AssignedXpPanel.vue'
import { FeatureFlags, userStore } from '@/stores/userStore.ts'

const route = useRoute()
const xpInfo = experienceStore()
const characterInfo = characterStore()
const userInfo = userStore()
const disadvantageBucket = ref(0)
const discretionaryBucket = ref(0)
const overallDiscretionaryTotal = ref(0)
const hasAssignedXpPanelFeatureToggle = ref(false)
const showAssignedXpPanel = computed(() => {
  return !characterInfo.isInCharacterCreation && hasAssignedXpPanelFeatureToggle.value && characterInfo.isPrimaryCharacter
})

const characterId = Number.parseInt(route.params.id)

const props = defineProps({
  isReadOnly: {
    type: Boolean,
    required: false,
    default: false,
  },
})

onMounted(async () => {
  await xpInfo.updateExperience(characterId)
  hasAssignedXpPanelFeatureToggle.value = await userInfo.hasFeatureFlag(FeatureFlags.ShowAssignedXpPanel)
})

watch(xpInfo.calculatedValues, () => {
  const disadvantage = xpInfo.getExperienceInfoForSection(XpSectionTypes.disadvantage)

  overallDiscretionaryTotal.value = 16 + disadvantage.total
  const availableDiscretionary = xpInfo.availableDiscretionary

  disadvantageBucket.value = Math.max(0, disadvantage.total - availableDiscretionary)
  discretionaryBucket.value = overallDiscretionaryTotal.value - availableDiscretionary >= 16 ? 16 : 16 + disadvantage.total - availableDiscretionary
}, { immediate: true })

async function finalizeCreation() {
  await axios.put(`characters/${characterId}/finalizeCharacterCreate`)
    .then(async () => {
      toaster.success('Successfully Finalized Character!')
      await characterInfo.getCharacterDetails(characterId)
      await xpInfo.updateExperience(characterId)
    })
}

const spentAllPoints = computed(() => {
  const discretionarySpent = overallDiscretionaryTotal.value - discretionaryBucket.value - disadvantageBucket.value == 0
  const allSectionsMeetMinimum = xpInfo.calculatedValues.every((section) => {
    if (section.sectionTypeId == XpSectionTypes.advantage)
      return true
    if (section.sectionTypeId == XpSectionTypes.disadvantage)
      return true
    if (section.sectionTypeId == XpSectionTypes.discretionary)
      return true

    return section.total >= section.characterCreateMax
  })

  return discretionarySpent && allSectionsMeetMinimum
})

const displayedSections = computed(() => {
  if (characterInfo.isInCharacterCreation) {
    return xpInfo.calculatedValues
  }
  const filteredSections = new Set<XpSectionType>([XpSectionTypes.advantage, XpSectionTypes.discretionary, XpSectionTypes.disadvantage])

  return xpInfo.calculatedValues.filter((section) => {
    return !filteredSections.has(section.sectionTypeId)
  })
})

</script>

<template>
  <h2>Review Character</h2>
  <Tabs value="0">
    <TabList>
      <Tab value="0">
        XP Breakdown
      </Tab>
      <Tab v-if="showAssignedXpPanel" value="4">
        Assigned XP
      </Tab>
    </TabList>
    <TabPanels>
      <TabPanel value="0">
        <table class="w-100">
          <tr>
            <th v-if="characterInfo.isInCharacterCreation" class="pr-2" />
            <th class="text-left">
              Name
            </th>
            <th v-if="!characterInfo.isInCharacterCreation" class="text-left">
              Spent XP
            </th>
            <th v-if="characterInfo.isInCharacterCreation" class="text-center">
              Required
            </th>
            <th v-if="characterInfo.isInCharacterCreation" class="text-center">
              Available
            </th>
            <th v-if="characterInfo.isInCharacterCreation" class="text-right">
              Disc.
            </th>
          </tr>
          <tr v-for="section in displayedSections" :key="section.name">
            <td v-if="characterInfo.isInCharacterCreation" class="text-center pr-2">
              <span v-if="section.sectionTypeId == XpSectionTypes.advantage || section.sectionTypeId == XpSectionTypes.disadvantage" class="material-symbols-outlined" title="No Status">
                do_not_disturb_on
              </span>
              <span v-else-if="section.sectionTypeId == XpSectionTypes.discretionary" class="material-symbols-outlined" title="You are required to spend all points">
                {{ overallDiscretionaryTotal - discretionaryBucket - disadvantageBucket == 0 ? "check_circle" : "warning" }}
              </span>
              <span v-else class="material-symbols-outlined" title="You are required to spend all points">
                {{ section.total >= section.characterCreateMax ? "check_circle" : "warning" }}
              </span>
            </td>
            <td class="text-left">
              {{ section.name }}
            </td>
            <!-- Required XP -->
            <td v-if="characterInfo.isInCharacterCreation" class="text-center">
              <div v-if="section.sectionTypeId == XpSectionTypes.discretionary">
                {{ discretionaryBucket + disadvantageBucket }} / {{ overallDiscretionaryTotal }}
              </div>
              <div v-else-if="section.sectionTypeId == XpSectionTypes.advantage">
                --
              </div>
              <div v-else-if="section.sectionTypeId == XpSectionTypes.disadvantage">
                --
              </div>
              <div v-else>
                {{ section.requiredXp }} / {{ section.characterCreateMax }}
              </div>
            </td>
            <!-- Available XP -->
            <td v-if="characterInfo.isInCharacterCreation" class="text-center">
              <div v-if="section.sectionTypeId == XpSectionTypes.disadvantage ">
                --
              </div>
              <div v-else-if="section.sectionTypeId == XpSectionTypes.discretionary ">
                <div v-if="overallDiscretionaryTotal - discretionaryBucket - disadvantageBucket == 0">
                  --
                </div>
                <div v-else>
                  {{ overallDiscretionaryTotal - discretionaryBucket - disadvantageBucket }}
                </div>
              </div>
              <div v-else-if="section.total < section.characterCreateMax && section.sectionTypeId !== XpSectionTypes.advantage">
                {{ section.characterCreateMax - section.total }}
              </div>
              <div v-else>
                --
              </div>
            </td>
            <!-- Discretionary XP -->
            <td v-if="characterInfo.isInCharacterCreation" class="text-center">
              <div v-if="section.sectionTypeId == XpSectionTypes.discretionary">
                --
              </div>
              <div v-else-if="section.sectionTypeId == XpSectionTypes.disadvantage">
                <div v-if="section.total == 0">
                  --
                </div>
                <div v-else>
                  ({{ section.total }})
                </div>
              </div>
              <div v-else-if="section.sectionTypeId == XpSectionTypes.advantage">
                <div v-if="section.total == 0">
                  --
                </div>
                <div v-else>
                  {{ section.total }}
                </div>
              </div>
              <div v-else>
                <div v-if="section.currentOptionalXp == 0">
                  --
                </div>
                <div v-else>
                  {{ section.currentOptionalXp }}
                </div>
              </div>
            </td>
            <td v-if="!characterInfo.isInCharacterCreation" class="">
              {{ section.levelXp }}
            </td>
          </tr>
          <!-- Total XP -->
          <tr v-if="!characterInfo.isInCharacterCreation">
            <td>Total</td>
            <td v-if="characterInfo.isPrimaryCharacter">
              {{ xpInfo.getTotalXp() }} / {{ xpInfo.totalAvailableXp }}
            </td>
            <td v-else>
              {{ xpInfo.getTotalXp() }} / <span class="material-symbols-outlined inline-icon">all_inclusive</span>
            </td>
          </tr>
        </table>

        <div v-if="!characterInfo.isInCharacterCreation" class="d-flex flex-row justify-content-between gap-3">
          <div>Character Level: {{ xpInfo.getCharacterLevel() }}</div>
          <div />
        </div>

        <Message v-if="characterInfo.isInCharacterCreation && !props.isReadOnly" severity="warn" class="mt-3">
          <p v-if="!spentAllPoints">
            To finalize a character will require you to spend all XP.
          </p>
          <p>Finalizing a character will block the ability to add / remove / change levels for Advantages / Disadvantages</p>
        </Message>
        <div v-if="characterInfo.isInCharacterCreation && !props.isReadOnly" class="text-right">
          <Button label="Finalize Creation" class="mt-3" :disabled="!spentAllPoints" @click="finalizeCreation" />
        </div>
        <Message v-if="characterInfo.isInCharacterCreation" severity="info" class="mt-3">
          <p>Here's a breakdown of what each column means:</p>
          <ul v-if="characterInfo.isInCharacterCreation">
            <li>Status - (Unlabeled, first column) These are the icons you see.  Checkmark means it's done.  Warning means it still needs work.  Dash means its optional</li>
            <li>Required - This column shows you the total xp for a section, and how much you have spent on it.</li>
            <li>Available - This is the remaining XP you need to spend for the given category</li>
            <li>Disc. - Discretionary - This is showing how much discretionary points you have spent in each section</li>
          </ul>
        </Message>
        <Message v-if="!characterInfo.isInCharacterCreation" severity="info" class="mt-3">
          <div>Experience Levels:</div>
          <ol>
            <li>1 - 25</li>
            <li>26 - 75</li>
            <li>76 - 150</li>
            <li>151 - 250</li>
            <li>251 - 375</li>
            <li>376 - 525</li>
            <li>526 - 700</li>
            <li>700+</li>
          </ol>
        </Message>
      </TabPanel>
      <TabPanel v-if="showAssignedXpPanel" value="4">
        <AssignedXpPanel :character-id="characterId" />
      </TabPanel>
    </TabPanels>
  </Tabs>
</template>

<style>
.inline-icon {
  vertical-align: bottom;
}
</style>
