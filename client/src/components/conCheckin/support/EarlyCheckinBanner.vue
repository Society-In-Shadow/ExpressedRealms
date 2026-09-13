<script setup lang="ts">

import { computed, onBeforeMount, ref } from 'vue'

import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'
import { characterStore } from '@/components/characters/character/stores/characterStore.ts'
import Button from 'primevue/button'
import { hasFlag } from '@/stores/featureFlags/featureFlagStore.ts'

const eventCheckinInfo = EventCheckinStore()
const characterInfo = characterStore()

const showFullMessage = ref(false)
const hasCheckedIn = ref(false)
const isApproved = ref(false)
const hasPrimaryCharacter = ref(true)

onBeforeMount(async () => {
  await eventCheckinInfo.getCheckinAvailable()
})

const showBanner = computed(() => hasFlag.ShowPreCheckinFunctionality)

// Things I need
// - Early Checkin Stage Progress
// - Deadline for character checkin
// - If they have checked in (might fold into above progress enum)
// - If they have opted into Charcter Storage and it's active
// - Name of the upcoming event?
</script>

<template>
  <div v-if="showBanner" class="custom-message m-1 m-md-3">
    <div class="d-flex align-items-center" @click="showFullMessage = !showFullMessage">
      <h2 class="m-0 p-0 flex-fill">
        Early Check In
      </h2>
      <Button :label="showFullMessage ? 'Show Less' : 'Show More'" size="small" />
    </div>
    <div v-if="showFullMessage">
      <p>You have paid for character storage, which allows you to do an early check in. This allows GO's to review, reach out, finalize, and print out your character before con.</p>
      <p>In addition, you should have gotten an additional 5xp assigned out to you once the early check in window opens.  This XP amount will be pushed to the next con if you are unable to make it to this one.</p>
      <p>The deadline for you to take advantage of Early Check In is Oct. 3, 2026 at 11:59pm CST (~10 days from now)</p>
      <div v-if="!hasPrimaryCharacter">
        <p>You need to select a primary character before you can proceed.</p>
      </div>
      <div v-else>
        <p>To start with, you should review your primary character, and verify that it's good for play.</p>
        <Button label="Review Sir Mountain Top" />
        <p>Then, once you are happy, hit the 'Understood and Check In' button below, which will kick off the following:</p>
        <ul>
          <li>A GO will be notified that the character is ready to be reviewed</li>
          <li>While it's waiting to be reviewed, you can still make changes to the Character, though it's recommended that you have everything in place.</li>
          <li>Once a GO has some free time, they will review your character and either approve it, or reach out on discord if they can.</li>
          <li>Once the character has been approved, a copy of your character will be created, and that copy is what will be printed and ready to be picked up at con.</li>
          <li>Which means you can make additional changes to the character and it will not affect what will be printed.</li>
          <li>Do note, depending on how things align, you may need to wait a bit for your booklet after check in.</li>
          <li>As per normal, once the CRB is ready for pickup, you will get an email notification denoting it is ready for pickup</li>
        </ul>
        <div v-if="!hasCheckedIn">
          <Button label="Understood and Check In" />
        </div>
        <div v-if="isApproved">
          <p>Character has been approved, awaiting printout.</p>
          <p>Character has been approved, and printed.  It's awaiting for assembly.</p>
          <p>Character has been approved, CRB is ready for pickup at SHQ.</p>
        </div>
      </div>
    </div>
  </div>
</template>

<style>
 .custom-message{
   background: color-mix(in srgb,var(--p-blue-500),transparent 84%);
   color: var(--p-blue-500);
   border-radius:  var(--p-content-border-radius);
   outline-width:  var(--p-content-border-width);
   padding: 0.75rem 1rem;
   outline-style: solid;
   -webkit-tap-highlight-color: transparent;
 }
</style>
