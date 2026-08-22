<script setup lang="ts">

import Checkbox from 'primevue/checkbox'
import { onBeforeMount, ref, watch } from 'vue'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'
import Button from 'primevue/button'
import { confirmationPopups } from '@/components/conCheckin/services/popupService.ts'

const eventCheckinInfo = EventCheckinStore()

const optedIn = ref(false)
const popups = confirmationPopups()

onBeforeMount(async () => {
  // ageInfo.value = await eventCheckinInfo.getVerifiedAge()
})

watch(() => eventCheckinInfo.isReset, (old, newValue) => {
  if (eventCheckinInfo.isReset) {
    optedIn.value = false
  }
})

</script>

<template>
  <h2>Character Storage</h2>
  <p>
    Character Storage is a service where we store and transport your primary character’s Prop Cards, Power Cards, and Money
    between conventions. This allows you not to worry about losing the items between games. Character storage
    is not required to play the game and is an additional service offered. The reason we are offering this is to offset
    some of the costs we have for conventions and web-hosting. The cost of character storage is <strong>$20 dollars</strong> and is
    <strong>nonrefundable</strong>, due at checkin (now), and entitles you to the following:
  </p>
  <ul>
    <li>Max random bonus for experience (+5) at current event</li>
    <li>
      Your items cards, power cards, and in game money stored at SHQ between the event paid and your next attended
      event tp to the end of the chronicle.
    </li>
    <li>Access to non-plot relevant props provided by SHQ</li>
    <li>
      Access to early con check-in allowing you to get and spend xp up to 11:59pm the night before the convention
      and have your character printed and ready to go when you arrive.
    </li>
  </ul>
  <h2>GO's / SHQ</h2>
  <p>
    Lead GO is responsible for cash box on the first day of con, to allow SHQ the ability to focus on printing out CRBs.
    The remaining days SHQ will be responsible for handling the cash box
  </p>
  <p>
    By clicking the button below, you are on the hook for making sure the money gets handled correctly. This website will
    keep a ledger of who owes how much.
  </p>
  <div>
    <Checkbox v-model="optedIn" input-id="opt-in" binary /><label class="ml-2" for="opt-in">Paid in Cash</label>
  </div>

  <Button label="Update Character Storage" class="mt-3" @click="popups.characterStorageConfirmation($event, optedIn)" />
</template>
