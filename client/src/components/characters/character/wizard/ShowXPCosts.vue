<script setup lang="ts">

import {onMounted, ref, watch} from "vue";
import {useRoute} from "vue-router";
import {
  experienceStore,
  type XpSectionType
} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import type {CalculatedExperience} from "@/components/characters/character/types.ts";

const route = useRoute()
const experienceInfo = experienceStore();

const props = defineProps({
  sectionType: {
    type: Number as unknown as () => XpSectionType,
    required: true,
  }
});

const xp = ref<CalculatedExperience>({});

onMounted(async () => {
  await experienceInfo.updateExperience(route.params.id);
})

watch(experienceInfo, () => {
  xp.value = experienceInfo.getExperienceInfoForSection(props.sectionType);
}, {immediate: true, deep: true})

</script>

<template>
  <div class="d-flex flex-row justify-content-between gap-3">
    <div>
      <div class="d-flex flex-row justify-content-center gap-2">
        <div><strong>Required XP:</strong> {{ xp.requiredXp }} / {{ xp.characterCreateMax }}</div>
        <div><span class="material-symbols-outlined" title="You are required to spend all points">{{ xp.total >= xp.characterCreateMax ? "check_circle" : "warning" }}</span></div>
      </div>
    </div>
    <div>
      <strong>Discretionary XP:</strong> {{ xp.currentOptionalXp }} / {{ xp.optionalMaxXP }}
    </div>
    <div>
      <strong>Available XP:</strong> {{ xp.availableXp }}
    </div>
  </div>
</template>
