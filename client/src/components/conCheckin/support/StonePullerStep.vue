<script setup lang="ts">

import StonePuller from '@/components/stonePuller/StonePuller.vue'
import InputNumber from 'primevue/inputnumber'
import Button from 'primevue/button'
import { computed, onMounted, ref } from 'vue'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'

const eventCheckinInfo = EventCheckinStore()
const checkinBonus = ref<number | null>(null)

onMounted(() => {
  checkinBonus.value = eventCheckinInfo.assignedXp?.amount ?? 0

  if (eventCheckinInfo.broughtNewPlayer)
    checkinBonus.value = 5

  if (eventCheckinInfo.goCheckinInfo.isFirstTimeUser)
    checkinBonus.value = 5
})

const overrideBonus = computed(() => eventCheckinInfo.goCheckinInfo.isFirstTimeUser || eventCheckinInfo.broughtNewPlayer)

function handleStonePulled(bonus: number) {
  if (checkinBonus.value === null)
    checkinBonus.value = bonus
}

async function permanentlySaveBonus() {
  let type = 2
  if (eventCheckinInfo.goCheckinInfo.isFirstTimeUser)
    type = 4
  else if (eventCheckinInfo.broughtNewPlayer)
    type = 5
  await eventCheckinInfo.addAssignedXp(type, checkinBonus.value!)
}

</script>

<template>
  <p>You can use physical stones for this, or use the digital one below.</p>
  <p>Chart is provided as reference for physical stone pull</p>
  <div class="d-flex flex-column">
    <label for="stoneBonus">Bonus:</label>
    <div class="d-flex flex-row gap-2">
      <InputNumber
        id="stoneBonus" v-model="checkinBonus" class="flex-shrink-1" :max="5" :min="0"
        :disabled="eventCheckinInfo.assignedXp !== null || overrideBonus"
      />
      <Button v-if="eventCheckinInfo.assignedXp === null" label="Permanently Save" @click="permanentlySaveBonus" />
      <p v-if="eventCheckinInfo.assignedXp !== null">
        Already Finalized
      </p>
    </div>
  </div>
  <p v-if="eventCheckinInfo.goCheckinInfo.isFirstTimeUser">
    This is a new player, they automatically get the full 5 xp <span v-if="!eventCheckinInfo.assignedXp">, you just need to save it</span>
  </p>
  <p v-if="eventCheckinInfo.broughtNewPlayer">
    They brought in a new player, they automatically get the full 5 xp <span v-if="!eventCheckinInfo.assignedXp">, you just need to save it</span>
  </p>

  <StonePuller :hide-description="true" @neutral-pulled="handleStonePulled" />
</template>
