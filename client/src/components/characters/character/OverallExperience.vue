<script setup lang="ts">

import {computed, onMounted} from "vue";
import Card from "primevue/card";
import Message from "primevue/message";
import {useRoute} from "vue-router";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";

const route = useRoute()
const experienceInfo = experienceStore();
var totalCreatorXp = computed(() => {
  return experienceInfo.experienceBreakdown.setupStatsXp + 
      experienceInfo.experienceBreakdown.setupKnowledgeXp + 
      experienceInfo.experienceBreakdown.setupPowersXp + 
      experienceInfo.experienceBreakdown.setupSkillsXp + 
      16
})

onMounted(async () => {
  experienceInfo.updateExperience(route.params.id);
})

</script>

<template>

  <Card class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" style="max-width: 30em">
    <template #content>
      <table>
        <tr>
          <td></td>
          <td class="text-right pl-3">Raw XP</td>
          <td class="text-right pl-3">Creator XP</td>
          <td class="text-right pl-3">Level XP</td>
        </tr>
        <tr>
          <td>Knowledge XP</td>
          <td class="text-right">{{ experienceInfo.experienceBreakdown.knowledgeXp }}</td>
          <td class="text-right">{{ experienceInfo.experienceBreakdown.setupKnowledgeXp }}</td>
          <td class="text-right">{{ experienceInfo.experienceBreakdown.knowledgeXp - experienceInfo.experienceBreakdown.setupKnowledgeXp }}</td>
        </tr>
        <tr>
          <td>Stats XP</td>
          <td class="text-right">{{ experienceInfo.experienceBreakdown.statsXp }}</td>
          <td class="text-right">{{ experienceInfo.experienceBreakdown.setupStatsXp }}</td>
          <td class="text-right">{{ experienceInfo.experienceBreakdown.statsXp - experienceInfo.experienceBreakdown.setupStatsXp }}</td>
        </tr>
        <tr>
          <td>Skills XP</td>
          <td class="text-right">{{ experienceInfo.experienceBreakdown.skillsXp }}</td>
          <td class="text-right">{{ experienceInfo.experienceBreakdown.setupSkillsXp }}</td>
          <td class="text-right">{{ experienceInfo.experienceBreakdown.skillsXp -experienceInfo.experienceBreakdown.setupSkillsXp }}</td>
        </tr>
        <tr>
          <td>Powers XP</td>
          <td class="text-right">{{ experienceInfo.experienceBreakdown.powersXp }}</td>
          <td class="text-right">{{ experienceInfo.experienceBreakdown.setupPowersXp }}</td>
          <td class="text-right">{{ experienceInfo.experienceBreakdown.powersXp - experienceInfo.experienceBreakdown.setupPowersXp }}</td>
        </tr>
        <tr>
          <td>Descretionary</td>
          <td class="text-right">--</td>
          <td class="text-right">16</td>
          <td class="text-right">-16</td>
        </tr>
        <tr>
          <td>Total</td>
          <td class="text-right">{{ experienceInfo.experienceBreakdown.total }}</td>
          <td class="text-right">{{ totalCreatorXp }}</td>
          <td class="text-right">{{ experienceInfo.experienceBreakdown.total - totalCreatorXp }}</td>
        </tr>
      </table>
    </template>
  </Card>
  <Message severity="info" class="mb-3" style="max-width: 30em">
    <div>This is an breakdown of all the XP the current character has. The calculations are assuming you are spending everything you can. This will be changed later.</div>
    <ul>
      <li>Raw XP - This is the total count for everything in the given category</li>
      <li>Character XP - This is the this is the XP you are expected to spend in character creation.</li>
      <li>Level XP - Once you take the difference of the two above, this is what determines your XL level</li>
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

<style scoped>

</style>