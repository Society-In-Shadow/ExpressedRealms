<script setup lang="ts">
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";
import {computed} from "vue";

const props = defineProps({
  statLevelInfo: {
    type: Object,
    required: true,
  },
  isLoading: {
    type: Boolean,
    required: false,
    default: false
  },
  currentLevelXp: {
    type: Number,
    required: false,
    default: 0
  },
  currentLevelId: {
    type: Number,
    required: false,
    default: 0
  },
  displayOnly: {
    type: Boolean,
    required: false,
    default: false
  }
});

const showXPTag = computed(() => {
  return !props.displayOnly;
})

const plusOrMinusSign = computed(() => {
  return props.statLevelInfo.level > props.currentLevelId ? "-" : "+";
});

</script>

<template>
  <div>
    <div class="row">
      <div class="col text-center">
        <div class="mb-2">
          Level
        </div>
        <div>
          <SkeletonWrapper :show-skeleton="isLoading" height="2rem" width="100%">
            {{ props.statLevelInfo.level }}
          </SkeletonWrapper>
        </div>
      </div>
      <div class="col text-center">
        <div class="mb-2">
          Bonus
        </div>
        <div>
          <SkeletonWrapper :show-skeleton="isLoading" height="2rem" width="100%">
            <span v-if="props.statLevelInfo.bonus > 0">+</span>{{ props.statLevelInfo.bonus }}
          </SkeletonWrapper>
        </div>
      </div>
      <div v-if="showXPTag" class="col text-center">
        <div class="mb-2">
          XP
        </div>
        <div>
          <SkeletonWrapper :show-skeleton="isLoading" height="2rem" width="100%">
            {{ plusOrMinusSign }}{{ Math.abs(props.statLevelInfo.totalXP - props.currentLevelXp) }}
          </SkeletonWrapper>
        </div>
      </div>
    </div>
    <div class="row">
      <div class="col">
        <SkeletonWrapper :show-skeleton="isLoading" height="4em" width="100%">
          {{ props.statLevelInfo.description }}
        </SkeletonWrapper>
      </div>
    </div>
  </div>
</template>
