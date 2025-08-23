<script setup lang="ts">

import axios from "axios";
import {onMounted, ref} from "vue";
import Card from "primevue/card";
import {useRoute} from "vue-router";

const route = useRoute()
const stats = ref<ExperienceBreakdownResponse>({});

onMounted(async () => {
  await axios.get(`/characters/${route.params.id}/experience`)
      .then((response) => {
        const ExperienceBreakdownResponse = response.data;
        stats.value = ExperienceBreakdownResponse;
      })
})

</script>

<template>
  <Card class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" style="max-width: 30em">
    <template #content>
      <div>Knowledge XP - {{ stats.knowledgeXp }} / {{ stats.knowledgeMax }}</div>
      <div>Stats XP - {{ stats.statsXp }} / {{ stats.setupStatsXp }}</div>
      <div>Skills XP - {{ stats.skillsXp }} / {{ stats.setupSkillsXp }}</div>
      <div>Powers XP - {{ stats.powersXp }} / {{ stats.setupPowersXp }}</div>
      <div>Total - {{ stats.total }}</div>
    </template>
  </Card>
</template>

<style scoped>

</style>