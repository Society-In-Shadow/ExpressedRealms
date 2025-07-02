<script setup lang="ts">
import {computed, type PropType} from 'vue';
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";
import type {SkillResponse} from "@/components/characters/character/skills/interfaces/SkillOptionsResponse";
import {skillStore} from "@/components/characters/character/skills/Stores/skillStore";

const skillInfo = skillStore();
const props = defineProps({
  selectedItem: {
    type: Object as PropType<SkillResponse>,
    required: true,
  },
  isLoading: {
    type: Boolean,
    required: true
  },
  currentXpLevel:{
    type: Number
  },
  showcaseOnly:{
    type: Boolean
  }
});

const plusOrMinusSign = computed(() => {
  return props.selectedItem.experienceCost > props.currentXpLevel ? "-" : "+";
});

</script>

<template>
  <SkeletonWrapper :show-skeleton="props.isLoading" height="5rem" width="100%">
    <div class="row">
      <div class="col text-left">
        <div class="mb-2">
          Level {{props.selectedItem.levelNumber}}
        </div>
        <div>
          <SkeletonWrapper :show-skeleton="isLoading" height="2rem" width="100%">
            {{ props.selectedItem.name }}
          </SkeletonWrapper>
        </div>
      </div>
      <div v-if="skillInfo.showExperience && !props.showcaseOnly" class="col text-right">
        <div class="mb-2">
          XP
        </div>
        <div>
          <SkeletonWrapper :show-skeleton="isLoading" height="2rem" width="100%">
            {{ plusOrMinusSign }} {{ Math.abs(props.selectedItem.experienceCost - props.currentXpLevel) }}
          </SkeletonWrapper>
        </div>
      </div>
    </div>
    <p class="m-0 mt-3 pb-2">
      {{ props.selectedItem.description }}
    </p>
    <div v-if="props.selectedItem.benefits && props.selectedItem.benefits.length > 0">
      <h3>Benefits</h3>
      <div v-for="benefit in props.selectedItem.benefits">
        <h4 class="d-flex justify-content-between w-100">
          <div>{{ benefit.name }}</div>
          <div class="text-right">
            +{{ benefit.modifier }}
          </div>
        </h4>
        <p class="m-0">
          {{ benefit.description }}
        </p>
      </div>
    </div>
  </SkeletonWrapper>
</template>
