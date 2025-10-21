<script setup lang="ts">

import {computed, onMounted, ref, type Ref} from 'vue'
import type {SkillResponse} from '@/components/characters/character/skills/interfaces/SkillOptionsResponse'
import {useRoute} from 'vue-router'
import axios from 'axios'
import SkeletonWrapper from '@/FormWrappers/SkeletonWrapper.vue'

const route = useRoute()

const props = defineProps({
  skillTypeId: {
    type: Number,
    required: true,
  },
  selectedLevelId: {
    type: Number,
    required: true,
  },
})

const skillLevels: Ref<Array<SkillResponse>> = ref([])
const isLoading = ref(true)
const selectedItem = ref(props.selectedLevelId)

const selectedLevel = computed(() => {
  return skillLevels.value.find(x => x.levelId === selectedItem.value)
})

onMounted(async () => {
  getEditOptions()
})

function getEditOptions() {
  axios.get(`characters/${route.params.id}/skills/${props.skillTypeId}`)
    .then((response) => {
      skillLevels.value = response.data
      isLoading.value = false
    })
}

</script>

<template>
  <div class="row pt-3">
    <div class="col p-0 m-0">
      <div class="p-3">
        <SkeletonWrapper :show-skeleton="isLoading" height="5rem" width="100%">
          <div class="row">
            <div class="col text-left">
              <div class="mb-2">
                Level {{ selectedLevel.levelNumber }}
              </div>
              <div>
                <SkeletonWrapper :show-skeleton="isLoading" height="2rem" width="100%">
                  {{ selectedLevel.name }}
                </SkeletonWrapper>
              </div>
            </div>
          </div>
          <p class="m-0 mt-3 pb-2">
            {{ selectedLevel.description }}
          </p>
          <div v-if="selectedLevel.benefits && selectedLevel.benefits.length > 0">
            <h3>Benefits</h3>
            <div v-for="benefit in selectedLevel.benefits">
              <h4 class="d-flex justify-content-between w-100">
                <div>{{ benefit.name }}</div>
                <div class="text-right">
                  +{{ benefit.modifier }}
                </div>
              </h4>
            </div>
          </div>
        </SkeletonWrapper>
      </div>
    </div>
  </div>
</template>
