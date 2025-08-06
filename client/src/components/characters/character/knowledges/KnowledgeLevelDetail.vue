<script setup lang="ts">
import {computed, type PropType} from 'vue';
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";
import type {KnowledgeOptions} from "@/components/characters/character/knowledges/types";

const props = defineProps({
  selectedItem: {
    type: Object as PropType<KnowledgeOptions>,
    required: true,
  },
  isLoading: {
    type: Boolean,
    required: true
  },
  currentXpLevel:{
    type: Number,
    required: true,
  },
  showcaseOnly:{
    type: Boolean
  },
  isUnknownKnowledge:{
    type: Boolean,
  }
});

const plusOrMinusSign = computed(() => {
  return baseXp.value > props.currentXpLevel ? "-" : "+";
});

const baseXp = computed(() => {
  return props.isUnknownKnowledge ? props.selectedItem?.totalUnknownXpCost : props.selectedItem?.totalGeneralXpCost;
})

</script>

<template>
  <SkeletonWrapper :show-skeleton="props.isLoading" height="5rem" width="100%" class="w-100">
    <div class="d-inline-flex justify-content-between w-100">
      <div class="mb-2">
        {{ props.selectedItem?.name }}
      </div>
      <div>
        Level {{ props.selectedItem.level }}
      </div>
    </div>
    <div class="row ">
      <div v-if="!props.showcaseOnly" class="col text-center">
        <div class="mb-2">
          XP
        </div>
        <div>
          <SkeletonWrapper :show-skeleton="isLoading" height="2rem" width="100%">
            {{ plusOrMinusSign }} {{ Math.abs(baseXp - props.currentXpLevel) }}
          </SkeletonWrapper>
        </div>
      </div>
      <div v-if="!props.showcaseOnly" class="col text-center">
        <div class="mb-2">
          Stones
        </div>
        <div>
          <SkeletonWrapper :show-skeleton="isLoading" height="2rem" width="100%">
            {{ props.selectedItem.stoneModifier }}
          </SkeletonWrapper>
        </div>
      </div>
      <div v-if="!props.showcaseOnly" class="col text-center">
        <div class="mb-2">
          Specializations
        </div>
        <div>
          <SkeletonWrapper :show-skeleton="isLoading" height="2rem" width="100%">
            {{ props.selectedItem.specializationCount }}
          </SkeletonWrapper>
        </div>
      </div>
    </div>
  </SkeletonWrapper>
</template>
