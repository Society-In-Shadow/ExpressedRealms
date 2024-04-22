<script setup lang="ts">

import axios from "axios";
import {onMounted, ref} from "vue";
import { useRoute } from 'vue-router'
import Button from 'primevue/button';
import SkeletonWrapper from "@/FormWrappers/SkeletonWrapper.vue";
import StatLevel from "@/components/characters/character/StatLevel.vue";
const route = useRoute()

const emit = defineEmits<{
  (e: 'toggleStat')
}>();

const props = defineProps({
  statTypeId: {
    type: Number,
    required: true,
  },
});

var stat = ref({});
var loading = ref(true);

onMounted(() =>{
  axios.get(`/api/characters/${route.params.id}/stats/${props.statTypeId}`)
      .then((response) => {
        stat.value = response.data;
        loading.value = false;
      })
});

</script>

<template>
  <div>
    <div class="row">
      <div class="col">
        <h3 class="mt-0">
          <SkeletonWrapper :show-skeleton="loading" height="2rem">
            {{stat.name}}
          </SkeletonWrapper>
        </h3>
        <div class="mb-3">
          <SkeletonWrapper :show-skeleton="loading" height="3rem">
            {{stat.description}}
          </SkeletonWrapper>
        </div>
      </div>
    </div>
    <div class="row">
      <div class="col">
        <div class="p-listbox p-3">
          <StatLevel :stat-level-info="stat.statLevelInfo" :is-loading="loading"></StatLevel>
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

<style scoped>

</style>