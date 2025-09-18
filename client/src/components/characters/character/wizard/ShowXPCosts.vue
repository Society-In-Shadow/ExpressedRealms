<script setup lang="ts">

import {computed, onMounted, type PropType, ref, watch} from "vue";
import {useRoute} from "vue-router";
import {
  experienceStore,
  type XpSectionType
} from "@/components/characters/character/stores/experienceBreakdownStore.ts";
import type {ExperienceBreakdown} from "@/components/characters/character/types.ts";

const route = useRoute()
const experienceInfo = experienceStore();

const props = defineProps({
  sectionType: {
    type: Object as PropType<XpSectionType>,
    required: true,
  }
});

const xp = ref<ExperienceBreakdown>({});

onMounted(async () => {
  await experienceInfo.updateExperience(route.params.id);
})

watch(experienceInfo, () => {
  xp.value = experienceInfo.experienceBreakdown.experience.filter(x => x.sectionTypeId == props.sectionType)[0];
}, {immediate: true, deep: true})

const requiredXp = computed(() => { return xp.value.total >= xp.value.characterCreateMax ? xp.value.characterCreateMax : xp.value.total  })
const currentOptionalXp = computed(() => { return xp.value.total >= xp.value.characterCreateMax ? xp.value.total - xp.value.characterCreateMax : 0 })
const optionalMaxXP = computed(() => { return currentOptionalXp.value + experienceInfo.availableDiscretionary })
const availableXp = computed(() => {
  if (requiredXp.value < xp.value.characterCreateMax) {
    return xp.value.characterCreateMax - requiredXp.value + optionalMaxXP.value;
  }
  return optionalMaxXP.value - currentOptionalXp.value;
});
</script>

<template>
  <div class="d-flex flex-row justify-content-between gap-3">
    <div>
      <div class="d-flex flex-row justify-content-center gap-2">
        <div><strong>Required XP:</strong> {{ requiredXp }} / {{ xp.characterCreateMax }}</div>
        <div><span class="material-symbols-outlined" title="You are required to spend all points">{{ xp.total >= xp.characterCreateMax ? "check_circle" : "warning" }}</span></div>
      </div>
    </div>
    <div>
      <!-- TODO: Need text that shows Discretionary vs Disadvantage breakdown -->
      <strong>Optional XP:</strong> {{ currentOptionalXp }} / {{ optionalMaxXP }}
    </div>
    <div>
      <strong>Available XP:</strong> {{ availableXp }}
    </div>
  </div>
</template>
