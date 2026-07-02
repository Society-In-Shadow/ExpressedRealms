<script setup lang="ts">
import { ref } from 'vue'
import AddPower from '@/components/expressions/powers/AddPower.vue'
import PowerCard from '@/components/expressions/powers/PowerCard.vue'
import Button from 'primevue/button'

import { type Power, TargetPowerType } from '@/components/expressions/powers/types'
import PowerReorder from '@/components/expressions/powers/PowerReorder.vue'
import { can } from '@/stores/userPermissionStore.ts'
import { powersStore } from '@/components/expressions/powers/stores/powersStore.ts'

const powerInfo = powersStore()

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

const toggleAddPower = () => {
  showAddPower.value = !showAddPower.value
}

const readOnly = ref(false)
const toggleReadOnly = () => {
  readOnly.value = !readOnly.value
}

const updatePowerPaths = async () => {
  await powerInfo.updatePowersByPathId(props.powerPathId)
}

</script>

<template>
  <PowerReorder v-if="can.Powers.Edit" :powers="props.powers" :power-path-id="props.powerPathId" @toggle-preview="toggleReadOnly" />
  <div v-if="props.powers && props.powers.length > 0">
    <div v-for="power in props.powers" :key="power.id">
      <PowerCard :power="power" :power-path-id="props.powerPathId" :is-read-only="props.isReadOnly || readOnly" />
    </div>
  </div>

  <AddPower
    v-if="showAddPower && can.Powers.Create && (!props.isReadOnly || !readOnly)" :target="TargetPowerType.PowerPath"
    :target-id="props.powerPathId" @cancelled="toggleAddPower" @updated="updatePowerPaths"
  />
  <Button
    v-if="!showAddPower && can.Powers.Create && (!props.isReadOnly || !readOnly)" class="w-100 m-2"
    label="Add Power" @click="toggleAddPower"
  />
</template>
