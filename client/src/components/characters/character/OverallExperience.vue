<script setup lang="ts">

import {onMounted} from "vue";
import Card from "primevue/card";
import Message from "primevue/message";
import {useRoute} from "vue-router";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import type {ExperienceBreakdown} from "@/components/characters/character/types.ts";

const route = useRoute()
const experienceInfo = experienceStore();

onMounted(async () => {
  experienceInfo.updateExperience(route.params.id);
})

function showSpent(xp: ExperienceBreakdown) {
  if(xp.characterCreateMax < 0 || xp.total < 0) return "--";
  return xp.levelXp;
}

</script>

<template>

  <Card class="custom-card">
    <template #content>
      <table class="w-100">
        <thead>
          <tr>
            <th></th>
            <th class="text-right pl-3">Total XP</th>
            <th class="text-right pl-3">Creation XP</th>
            <th class="text-right pl-3">Spent XP</th>
          </tr>
        </thead>
        <tr v-for="section in experienceInfo.experienceBreakdown.experience">
          <td>{{section.name}}</td>
          <td class="text-right">{{ section.total < 0 ? '--' : section.total }}</td>
          <td class="text-right">{{ section.characterCreateMax < 0 ? '--' : section.characterCreateMax }}</td>
          <td class="text-right">{{ showSpent(section) }}</td>
        </tr>
      </table>
    </template>
  </Card>
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