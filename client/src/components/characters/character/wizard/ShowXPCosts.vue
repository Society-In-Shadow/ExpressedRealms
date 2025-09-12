<script setup lang="ts">

import {onMounted, ref} from "vue";
import {useRoute} from "vue-router";
import {experienceStore} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import type {ExperienceBreakdown} from "@/components/characters/character/types.ts";

const route = useRoute()
const experienceInfo = experienceStore();

const props = defineProps({
  xpNameTag: {
    type: String,
    required: true,
  }
});

const xp = ref<ExperienceBreakdown>({});

onMounted(async () => {
  await experienceInfo.updateExperience(route.params.id);
  xp.value = experienceInfo.experienceBreakdown.experience.filter(x => x.name == props.xpNameTag)[0];
})

</script>

<template>
  <div class="pb-3" v-if="experienceInfo.showAllExperience">{{ xp.total}} Total XP - {{xp.characterCreateMax}} Creation XP = {{xp.levelXp}} XP</div>
</template>
