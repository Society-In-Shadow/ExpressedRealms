<script setup lang="ts">
import {onBeforeMount, ref, computed} from "vue";
import AddPower from "@/components/expressions/powers/AddPower.vue";
import {powersStore} from "@/components/expressions/powers/stores/powersStore";
import PowerCard from "@/components/expressions/powers/PowerCard.vue";
import Button from 'primevue/button';

import {UserRoles, userStore} from "@/stores/userStore";

let userInfo = userStore();

const props = defineProps({
  powerPathId: {
    type: Number,
    required: true,
  }
});

const powers = powersStore();

const powerPath = computed(() => {
  return powers.powers.find(x => x.powerPathId === props.powerPathId)
});

const showAddPower = ref(false);

onBeforeMount(async () => {
  await powers.getPowers(props.powerPathId);
})

const toggleAddPower = () => {
  showAddPower.value = !showAddPower.value;
}
</script>

<template>
  <div v-if="powerPath && powerPath.powers.length > 0">
    <div v-for="power in powerPath.powers" :key="power.id">
      <PowerCard :power="power" :power-path-id="props.powerPathId" />
    </div>
  </div>

  <AddPower
    v-if="showAddPower && userInfo.hasUserRole(UserRoles.PowerManagementRole)"
    :power-path-id="props.powerPathId" @cancelled="toggleAddPower"
  />
  <Button
    v-if="!showAddPower && userInfo.hasUserRole(UserRoles.PowerManagementRole)" class="w-100 m-2"
    label="Add Power" @click="toggleAddPower"
  />
</template>
