<script setup lang="ts">

import {computed, onMounted, ref, watch} from "vue";
import Message from "primevue/message";
import {useRoute} from "vue-router";
import {experienceStore, XpSectionTypes} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import Button from "primevue/button";
import axios from "axios";

const route = useRoute()
const xpInfo = experienceStore();
const disadvantageBucket = ref(0);
const discretionaryBucket = ref(0);
const overallDiscretionaryTotal = ref(0);

onMounted(async () => {
  await xpInfo.updateExperience(route.params.id);  
})

watch(xpInfo.calculatedValues, () => {
  const disadvantage = xpInfo.getExperienceInfoForSection(XpSectionTypes.disadvantage);

  overallDiscretionaryTotal.value = 16 + disadvantage.total;
  const availableDiscretionary = xpInfo.availableDiscretionary;

  disadvantageBucket.value = Math.max(0, disadvantage.total - availableDiscretionary);
  discretionaryBucket.value = overallDiscretionaryTotal.value - availableDiscretionary >= 16 ? 16 : 16 + disadvantage.total - availableDiscretionary  ;
}, { immediate: true })

async function finalizeCreation(){
  await axios.put(`characters/${route.params.id}/finalizeCharacterCreate`)
}

const spentAllPoints = computed(() => {
  
  const discretionarySpent = overallDiscretionaryTotal.value - discretionaryBucket.value - disadvantageBucket.value == 0;
  const allSectionsMeetMinimum = xpInfo.calculatedValues.every(section => {
    if (section.sectionTypeId == XpSectionTypes.advantage)
      return true;
    if (section.sectionTypeId == XpSectionTypes.disadvantage)
      return true;
    if (section.sectionTypeId == XpSectionTypes.discretionary)
      return true;

    return section.total >= section.characterCreateMax;
  });

  return discretionarySpent && allSectionsMeetMinimum;
})

</script>

<template>
  <h2>Review Character</h2>
  
  <table class="w-100">
    <tr>
      <th class="pr-2"></th>
      <th class="text-left">Name</th>
      <th class="text-center">Required</th>
      <th class="text-center">Available</th>
      <th class="text-right">Disc.</th>
    </tr>
    <tr v-for="section in xpInfo.calculatedValues">
      <td class="text-center pr-2">
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
      <td class="text-left">{{section.name}}</td>
      
      <!-- Required XP -->
      <td class="text-center">
        <div v-if="section.sectionTypeId == XpSectionTypes.discretionary">
          {{ discretionaryBucket + disadvantageBucket }} / {{overallDiscretionaryTotal}}
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
      <td class="text-center">
        <div v-if="section.sectionTypeId == XpSectionTypes.disadvantage ">
          --
        </div>
        <div v-else-if="section.sectionTypeId == XpSectionTypes.discretionary ">
          <div v-if="overallDiscretionaryTotal - discretionaryBucket - disadvantageBucket == 0">--</div>
          <div v-else>{{ overallDiscretionaryTotal - discretionaryBucket - disadvantageBucket }}</div>
          
        </div>
        <div v-else-if="section.total < section.characterCreateMax && section.sectionTypeId != XpSectionTypes.discretionary && section.sectionTypeId != XpSectionTypes.advantage">
          {{ section.characterCreateMax - section.total }}
        </div>
        <div v-else>
          --
        </div>
      </td>
      <!-- Discretionary XP -->
      <td class="text-center">
        <div v-if="section.sectionTypeId == XpSectionTypes.discretionary">
          --
        </div>
        <div v-else-if="section.sectionTypeId == XpSectionTypes.disadvantage">
          <div v-if="section.total == 0">--</div>
          <div v-else>({{ section.total }})</div>
        </div>
        <div v-else-if="section.sectionTypeId == XpSectionTypes.advantage">
          <div v-if="section.total == 0">--</div>
          <div v-else>{{ section.total }}</div>
        </div>
        <div v-else>
          <div v-if="section.currentOptionalXp == 0">--</div>
          <div v-else>{{ section.currentOptionalXp }}</div>
        </div>
      </td>
    </tr>
  </table>
  <div class="text-right">
    <Button label="Finalize Creation" class="mt-3" @click="finalizeCreation" :disabled="!spentAllPoints"/>
  </div>

  
  <Message severity="info" class="mt-3">
    <div>This is an breakdown of all the XP the current character has. The calculations are assuming you are spending everything you can. This will be changed later.</div>
    <ul>
      <li>Status - These are the icons you see.  Checkmark means it's done.  Warning means it still needs work.  Dash means its optional</li>
      <li>Required - This column shows you the total xp for a section, and how much you have spent on it.</li>
      <li>Available - This is the remaining XP you need to spend for the given category</li>
      <li>Disc. - Discretionary - This is showing how much discretionary points you have spent in each section</li>
    </ul>
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
</template>
