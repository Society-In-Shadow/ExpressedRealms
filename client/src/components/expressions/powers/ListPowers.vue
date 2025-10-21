<script setup lang="ts">
import {onMounted, ref} from 'vue'
import AddPower from '@/components/expressions/powers/AddPower.vue'
import PowerCard from '@/components/expressions/powers/PowerCard.vue'
import Button from 'primevue/button'

import {UserRoles, userStore} from '@/stores/userStore'
import type {Power} from '@/components/expressions/powers/types'
import PowerReorder from '@/components/expressions/powers/PowerReorder.vue'

let userInfo = userStore()

const props = defineProps({
  powerPathId: {
    type: Number,
    required: true,
  },
  powers: {
    type: Array as () => Power[],
    required: true,
  },
  isReadOnly: {
    type: Boolean,
    required: false,
  },
})

const showAddPower = ref(false)
const hasPowerManagementRole = ref(false)

onMounted(async () => {
  hasPowerManagementRole.value = await userInfo.hasUserRole(UserRoles.PowerManagementRole)
})

const toggleAddPower = () => {
  showAddPower.value = !showAddPower.value
}

const readOnly = ref(false)
const toggleReadOnly = () => {
  readOnly.value = !readOnly.value
}

</script>

<template>
  <PowerReorder :powers="props.powers" :power-path-id="props.powerPathId" @toggle-preview="toggleReadOnly" />
  <div v-if="props.powers && props.powers.length > 0">
    <div v-for="power in props.powers" :key="power.id">
      <PowerCard :power="power" :power-path-id="props.powerPathId" :is-read-only="props.isReadOnly || readOnly" />
    </div>
  </div>

  <AddPower
    v-if="showAddPower && hasPowerManagementRole && (!props.isReadOnly || !readOnly)"
    :power-path-id="props.powerPathId" @cancelled="toggleAddPower"
  />
  <Button
    v-if="!showAddPower && hasPowerManagementRole && (!props.isReadOnly || !readOnly)" class="w-100 m-2"
    label="Add Power" @click="toggleAddPower"
  />
</template>
