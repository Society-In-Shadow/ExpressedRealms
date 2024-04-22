<script setup lang="ts">
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";

const props = defineProps({
  statLevelInfo: {
    type: Object,
    required: true,
  },
  isLoading: {
    type: Boolean,
    required: false,
    default: false
  }
});
</script>

<template>
  <div class="row">
    <div class="col-4 text-center">
      <div>Level</div>
      <div>
        <SkeletonWrapper :show-skeleton="isLoading" height="2rem" width="100%">
          {{props.statLevelInfo.level}}
        </SkeletonWrapper>
      </div>
    </div>
    <div class="col-4 text-center">
      <div>Bonus</div>
      <div>
        <SkeletonWrapper :show-skeleton="isLoading" height="2rem" width="100%">
          {{props.statLevelInfo.bonus}}
        </SkeletonWrapper>
      </div>
    </div>
    <div class="col-4 text-center">
      <div>XP</div>
      <div>
        <SkeletonWrapper :show-skeleton="isLoading" height="2rem" width="100%">
          {{props.statLevelInfo.xp}}
        </SkeletonWrapper>
      </div>
    </div>
  </div>
  <div class="row">
    <div class="col">
      <SkeletonWrapper :show-skeleton="isLoading" height="4em" width="100%">
        {{props.statLevelInfo.description}}
      </SkeletonWrapper>
    </div>
  </div>
</template>

<style scoped>

</style>