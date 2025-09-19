<script setup lang="ts">

import {onMounted, ref, watch} from "vue";
import Message from "primevue/message";
import {useRoute} from "vue-router";
import {experienceStore, XpSectionTypes} from "@/components/characters/character/stores/experienceBreakdownStore.ts";

const route = useRoute()
const xpInfo = experienceStore();
const disadvantageBucket = ref(0);
const discretionaryBucket = ref(0);

onMounted(async () => {
  await xpInfo.updateExperience(route.params.id);  
})

watch(xpInfo.calculatedValues, () => {
  const disadvantage = xpInfo.getExperienceInfoForSection(XpSectionTypes.disadvantage);

  const overallTotal = 16 + disadvantage.total;
  const availableDiscretionary = xpInfo.availableDiscretionary;

  disadvantageBucket.value = Math.max(0, disadvantage.total - availableDiscretionary);
  discretionaryBucket.value = overallTotal - availableDiscretionary >= 16 ? 16 : 16 + disadvantage.total - availableDiscretionary  ;
}, { immediate: true })

</script>

<template>
  <h2>Experience Breakdown</h2>
  
  <table class="w-100">
    <tr>
      <th class="pr-2"></th>
      <th class="text-left">Name</th>
      <th class="text-center">Required</th>
      <th class="text-center">Discretionary</th>
      <th class="text-center">Available</th>
    </tr>
    <tr v-for="section in xpInfo.calculatedValues">
      <td class="text-center pr-2">
        <span v-if="section.sectionTypeId == XpSectionTypes.advantage" class="material-symbols-outlined" title="No Status">
          do_not_disturb_off
        </span>
        <span v-else-if="section.sectionTypeId == XpSectionTypes.disadvantage" class="material-symbols-outlined" title="You are required to spend all points">
          {{ disadvantageBucket == section.total ? "check_circle" : "warning" }}
        </span>
        <span v-else-if="section.sectionTypeId == XpSectionTypes.discretionary" class="material-symbols-outlined" title="You are required to spend all points">
          {{ discretionaryBucket == 16 ? "check_circle" : "warning" }}
        </span>
        <span v-else class="material-symbols-outlined" title="You are required to spend all points">
          {{ section.total >= section.characterCreateMax ? "check_circle" : "warning" }}
        </span>
      </td>
      <td class="text-left">{{section.name}}</td>

      <td class="text-center">
        <div v-if="section.sectionTypeId == XpSectionTypes.discretionary">
          {{ discretionaryBucket }} / 16
        </div>
        <div v-else-if="section.sectionTypeId == XpSectionTypes.advantage">
          0
        </div>
        <div v-else-if="section.sectionTypeId == XpSectionTypes.disadvantage">
          {{ disadvantageBucket }} / {{ section.total }}
        </div>
        <div v-else>
          {{ section.requiredXp }} / {{ section.characterCreateMax }}
        </div>
      </td>
      <td class="text-center">
        <div v-if="section.sectionTypeId == XpSectionTypes.discretionary || section.sectionTypeId == XpSectionTypes.disadvantage">
          --
        </div>
        <div v-else-if="section.sectionTypeId == XpSectionTypes.advantage">
          {{ section.total }}
        </div>
        <div v-else>
          {{ section.currentOptionalXp }}
        </div>
        
      </td>
      <td class="text-center">
        <div v-if="section.sectionTypeId == XpSectionTypes.advantage">
          {{ section.total == 8 ? '--' : 8-section.total }}
        </div>
        <div v-else-if="section.sectionTypeId == XpSectionTypes.disadvantage || section.sectionTypeId == XpSectionTypes.discretionary">
          --
        </div>
        <div v-else>{{xpInfo.availableDiscretionary}}</div>
      </td>
    </tr>
  </table>
  
  <Message severity="info" class="mt-3">
    <div>This is an breakdown of all the XP the current character has. The calculations are assuming you are spending everything you can. This will be changed later.</div>
    <ul>
      <li>Total XP - Total experience in each category including character creation XP and XP spent past creation.</li>
      <li>Creation XP - Base experience to be spent in each section during character creation.</li>
      <li>Spent XP - Experience spent past character creation, this determines your character's XL.</li>
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
