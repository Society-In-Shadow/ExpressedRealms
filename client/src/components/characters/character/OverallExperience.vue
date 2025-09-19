<script setup lang="ts">

import {onMounted} from "vue";
import Message from "primevue/message";
import {useRoute} from "vue-router";
import {experienceStore, XpSectionTypes} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import type {ExperienceBreakdown} from "@/components/characters/character/types.ts";

const route = useRoute()
const xpInfo = experienceStore();

onMounted(async () => {
  xpInfo.updateExperience(route.params.id);
})

function showSpent(xp: ExperienceBreakdown) {
  if(xp.characterCreateMax < 0 || xp.total < 0) return "--";
  return xp.levelXp;
}

</script>

<template>
  <h2>Experience Breakdown</h2>
  
  <table class="w-100">
    <tr>
      <th class="text-left">Name</th>
      <th></th>
      <th class="text-center">Required</th>
      <th class="text-center">Discretionary</th>
      <th class="text-center">Available</th>
    </tr>
    <tr v-for="section in xpInfo.calculatedValues">
      <td class="text-left">{{section.name}}</td>
      <td class="text-center">
        <div v-if="section.sectionTypeId == XpSectionTypes.advantage || section.sectionTypeId == XpSectionTypes.disadvantage || section.sectionTypeId == XpSectionTypes.discretionary">--</div>
        <span v-else class="material-symbols-outlined" title="You are required to spend all points">
          {{ section.total >= section.characterCreateMax ? "check_circle" : "warning" }}
        </span>
      </td>
      <td class="text-center">
          {{ section.requiredXp }} / {{ section.characterCreateMax }}
      </td>
      <td class="text-center">
        {{ section.currentOptionalXp }} / {{ section.optionalMaxXP }}
      </td>
      <td class="text-center">
        {{ section.availableXp }}
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

<style>
@media(max-width: 768px){
  .custom-card > .p-card-body{
    padding: 0rem !important;
  }

  .custom-toc > .p-card-body{
    padding-left: 1rem !important;
    padding-right: 1rem !important;
  }

  .custom-card .p-tabpanels{
    padding: 0.5rem !important;
  }
  
  .custom-card >>> .p-card-content{
    padding: 0;
  }
}
</style>