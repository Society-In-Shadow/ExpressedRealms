<script setup lang="ts">
import type { PropType } from 'vue';
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";
import type {SkillResponse} from "@/components/characters/character/skills/interfaces/SkillOptionsResponse";

const props = defineProps({
  selectedItem: {
    type: Object as PropType<SkillResponse>,
    required: true,
  },
  isLoading: {
    type: Boolean,
    required: true
  }
});

</script>

<template>
  <SkeletonWrapper :show-skeleton="props.isLoading" height="5rem" width="100%">
    <h3 class="d-flex mt-0 pt-0 justify-content-between w-100">
      <div>Level</div>
      <div class="text-right">{{props.selectedItem.name}}</div>
    </h3>
    <p class="m-0 pb-2">{{ props.selectedItem.description}}</p>
    <div v-if="props.selectedItem.benefits && props.selectedItem.benefits.length > 0">
      <h3>Benefits</h3>
      <div v-for="benefit in props.selectedItem.benefits">
        <h4 class="d-flex justify-content-between w-100">
          <div>{{benefit.name}}</div>
          <div class="text-right">+{{benefit.modifier}}</div>
        </h4>
        <p class="m-0">{{ benefit.description}}</p>
      </div>
    </div>
  </SkeletonWrapper>
</template>

<style scoped>

</style>