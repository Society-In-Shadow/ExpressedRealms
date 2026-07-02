<script setup lang="ts">
import { ref } from 'vue'
import PowerCard from '@/components/expressions/powers/PowerCard.vue'
import Button from 'primevue/button'

import { type Power, TargetPowerType } from '@/components/expressions/powers/types'
import PowerReorder from '@/components/expressions/powers/PowerReorder.vue'
import { can } from '@/stores/userPermissionStore.ts'
import { powersStore } from '@/components/expressions/powers/stores/powersStore.ts'
import { powerDialogs } from '@/components/expressions/powers/services/dialogs.ts'

const powerInfo = powersStore()

const dialogs = powerDialogs()

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

const readOnly = ref(false)
const toggleReadOnly = () => {
  readOnly.value = !readOnly.value
}

const showAddPower = async () => {
  const result = await dialogs.showAddPower({ target: TargetPowerType.PowerPath, targetId: props.powerPathId })

  if (result?.action == 'added') {
    await powerInfo.updatePowersByPathId(props.powerPathId)
  }
}

</script>

<template>
  <PowerReorder v-if="can.Powers.Edit" :powers="props.powers" :power-path-id="props.powerPathId" @toggle-preview="toggleReadOnly" />
  <div v-if="props.powers && props.powers.length > 0">
    <div v-for="power in props.powers" :key="power.id">
      <PowerCard :power="power" :power-path-id="props.powerPathId" :is-read-only="props.isReadOnly || readOnly" />
    </div>
  </div>

  <Button
    v-if="can.Powers.Create && (!props.isReadOnly || !readOnly)" class="w-100 m-2"
    label="Add Power" @click="showAddPower"
  />
</template>
