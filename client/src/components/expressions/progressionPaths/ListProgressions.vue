<script setup lang="ts">

import {onBeforeMount, ref} from "vue";
import Button from 'primevue/button';
import Divider from 'primevue/divider';
import {UserRoles, userStore} from "@/stores/userStore";
import {progressionPathStore} from "@/components/expressions/progressionPaths/stores/progressionPathsStore.ts";
import AddProgressionPath from "@/components/expressions/progressionPaths/AddProgressionPath.vue";
import ShowProgressionPath from "@/components/expressions/progressionPaths/ShowProgressionPath.vue";
import ListProgressionLevels from "@/components/expressions/progressionLevels/ListProgressionLevels.vue";

let userInfo = userStore();
let progressionPaths = progressionPathStore();

const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  }
});

const hasManageProgressionPathsRole = ref(false);

onBeforeMount(async () => {
  await progressionPaths.getProgressionPaths(props.expressionId);
  hasManageProgressionPathsRole.value = await userInfo.hasUserRole(UserRoles.ManageProgressionPaths);
})

const showAddPower = ref(false);

const toggleAddPower = () => {
  showAddPower.value = !showAddPower.value;
}

</script>

<template>
  
  <div v-for="path in progressionPaths.progressionPaths" :key="path.id">
    <ShowProgressionPath :path="path" :expression-id="props.expressionId" :is-read-only="false" />
   
    <Divider />
    <h2>Levels</h2>
    <ListProgressionLevels :expression-id="props.expressionId" :progression-id="path.id" :progression-levels="path.levels"/>
  </div>

  <Button
      v-if="!showAddPower && hasManageProgressionPathsRole" class="w-100 m-2"
      label="Add Progression Path" @click="toggleAddPower"
  />
  <AddProgressionPath
    v-if="showAddPower && hasManageProgressionPathsRole"
    :expression-id="props.expressionId" @canceled="toggleAddPower"
  />
</template>
