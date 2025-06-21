<script setup lang="ts">
import {ref} from "vue";
import AddPower from "@/components/expressions/powers/AddPower.vue";
import PowerCard from "@/components/expressions/powers/PowerCard.vue";
import Button from 'primevue/button';

import {UserRoles, userStore} from "@/stores/userStore";
import type {Power} from "@/components/expressions/powers/types";

let userInfo = userStore();

const props = defineProps({
  powerPathId: {
    type: Number,
    required: true,
  },
  powers:{
    type: Array as () => Power[],
    required: true,
  }
});

const showAddPower = ref(false);

const toggleAddPower = () => {
  showAddPower.value = !showAddPower.value;
}
</script>

<template>
  <div v-if="props.powers && props.powers.length > 0">
    <div v-for="power in props.powers" :key="power.id">
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
