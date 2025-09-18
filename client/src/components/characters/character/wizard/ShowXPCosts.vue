<script setup lang="ts">

import {computed, onMounted, ref, watch} from "vue";
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
})

watch(experienceInfo, () => {
  xp.value = experienceInfo.experienceBreakdown.experience.filter(x => x.name == props.xpNameTag)[0];
}, {immediate: true, deep: true})

const currentOptionalXp = computed(() => { return xp.value.total >= xp.value.characterCreateMax ? xp.value.total - xp.value.characterCreateMax : 0 })
const optionalMaxXP = computed(() => { return currentOptionalXp.value + experienceInfo.availableDiscretionary })

</script>

<template>
  <div class="d-flex flex-row justify-content-between gap-3">
    <div>
      <div class="d-flex flex-row justify-content-center gap-2">
        <div><strong>Required XP:</strong> {{ xp.total >= xp.characterCreateMax ? xp.characterCreateMax : xp.total }} / {{ xp.characterCreateMax }}</div>
        <div><span class="material-symbols-outlined" title="You are required to spend all points">{{ xp.total >= xp.characterCreateMax ? "check_circle" : "warning" }}</span></div>
      </div>
    </div>
    <div>
      <!-- TODO: Need text that shows Discretionary vs Disadvantage breakdown -->
      <strong>Optional XP:</strong> {{ currentOptionalXp }} / {{ optionalMaxXP }}
    </div>
  </div>
</template>
