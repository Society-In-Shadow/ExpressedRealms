<script setup lang="ts">

import axios from "axios";
import {onMounted, ref, type Ref} from "vue";
import {useRoute} from 'vue-router'
import Button from 'primevue/button';
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";
import {FeatureFlags, userStore} from "@/stores/userStore.ts";
import type {Stat} from "@/components/characters/character/stats/type.ts";

const route = useRoute()

const emit = defineEmits<{
  toggleStat: [],
}>();

const props = defineProps({
  statTypeId: {
    type: Number,
    required: true,
  },
});

const stat:Ref<Stat> = ref({
  statLevelInfo: {}
});
const isLoading = ref(true);
const showOptions = ref(false);
const showCharacterWizard = ref(false);
const userInfo = userStore();

onMounted(async () =>{
  reloadStatInfo();
  showCharacterWizard.value = await userInfo.hasFeatureFlag(FeatureFlags.ShowCharacterWizard);
});

function reloadStatInfo() {
  axios.get(`/characters/${route.params.id}/stat/${props.statTypeId}`)
      .then((response) => {
        stat.value = response.data;
        isLoading.value = false;
      })
}

</script>

<template>
  <div class="w-100" style="min-width: 300px">
    <div class="row">
      <div class="col">
        <h3 class="mt-0">
          <SkeletonWrapper :show-skeleton="isLoading" height="2rem">
            <div class="row">
              <div class="col">
                {{ stat.name }}
              </div>
              <div v-if="showOptions" class="col text-right">
                {{ stat.availableXP }} XP
              </div>
            </div>
          </SkeletonWrapper>
        </h3>
        <div class="mb-3">
          <SkeletonWrapper :show-skeleton="isLoading" height="3rem">
            {{ stat.description }}
          </SkeletonWrapper>
        </div>
      </div>
    </div>
    <div class="row">
      <div class="col">
        <div v-if="!showOptions" class="p-listbox p-3">
          <div class="row">
            <div class="col text-center">
              <div class="mb-2">
                Level
              </div>
              <div>
                <SkeletonWrapper :show-skeleton="isLoading" height="2rem" width="100%">
                  {{ stat.statLevelInfo.level }}
                </SkeletonWrapper>
              </div>
            </div>
            <div class="col text-center">
              <div class="mb-2">
                Bonus
              </div>
              <div>
                <SkeletonWrapper :show-skeleton="isLoading" height="2rem" width="100%">
                  <span v-if="stat.statLevelInfo.bonus > 0">+</span>{{ stat.statLevelInfo.bonus }}
                </SkeletonWrapper>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col">
              <SkeletonWrapper :show-skeleton="isLoading" height="4em" width="100%">
                {{ stat.statLevelInfo.description }}
              </SkeletonWrapper>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="row">
      <div class="col">
        <Button data-cy="logoff-button" label="Back" class="w-100 mb-2" @click="emit('toggleStat')" />
      </div>
    </div>
  </div>
</template>
