<script setup lang="ts">

import StonePuller from '@/components/stonePuller/StonePuller.vue'
import InputNumber from 'primevue/inputnumber'
import Button from 'primevue/button'
import { computed, onMounted, ref } from 'vue'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'

const eventCheckinInfo = EventCheckinStore()
const checkinBonus = ref<number | null>(null)
const isFirstTimeUser = ref(false)
const broughtNewPlayer = ref(false)
const assignedXpAmount = ref({})
const isReadOnly = ref(true)

onMounted(async () => {
  const response = await eventCheckinInfo.getStonePullInformation()

  isFirstTimeUser.value = response.isFirstTimeUser
  broughtNewPlayer.value = response.broughtFriend
  assignedXpAmount.value = response.assignedXp
  isReadOnly.value = response.hasCompletedStep
  checkinBonus.value = response.assignedXp?.amount ?? null

  if (broughtNewPlayer.value)
    checkinBonus.value = 5

  if (isFirstTimeUser.value)
    checkinBonus.value = 5
})

const overrideBonus = computed(() => isFirstTimeUser.value || broughtNewPlayer.value)

function handleStonePulled(bonus: number) {
  if (checkinBonus.value === null)
    checkinBonus.value = bonus
}

async function permanentlySaveBonus() {
  let type = 2
  if (isFirstTimeUser.value)
    type = 4
  else if (broughtNewPlayer.value)
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
        :disabled="assignedXpAmount !== null || overrideBonus"
      />
      <Button v-if="checkinBonus && (checkinBonus >= 1 || checkinBonus <=5) && !isReadOnly" label="Permanently Save" @click="permanentlySaveBonus" />
      <p v-if="assignedXpAmount !== null">
        Already Finalized
      </p>
    </div>
  </div>
  <p v-if="isFirstTimeUser">
    This is a new player, they automatically get the full 5 xp <span v-if="assignedXpAmount == null">, you just need to save it</span>
  </p>
  <p v-if="broughtNewPlayer && !isFirstTimeUser">
    They brought in a new player, they automatically get the full 5 xp <span v-if="assignedXpAmount == null">, you just need to save it</span>
  </p>

  <StonePuller :hide-description="true" @neutral-pulled="handleStonePulled" />
</template>
