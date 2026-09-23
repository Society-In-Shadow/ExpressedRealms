<script setup lang="ts">

import { computed, ref } from 'vue'

import Button from 'primevue/button'
import { hasFlag } from '@/stores/featureFlags/featureFlagStore.ts'
import { useQuery } from '@pinia/colada'
import { earlyCheckinQuery } from '@/components/conCheckin/services/earlyCheckinService.ts'
import { DateTime } from 'luxon'
import { useRouter } from 'vue-router'
import { CheckinStage } from '@/components/conCheckin/types.ts'
import { confirmationPopups } from '@/components/conCheckin/services/popupService.ts'

const router = useRouter()

const showFullMessage = ref(false)

const { data, isPending } = useQuery(earlyCheckinQuery)

const popups = confirmationPopups()

const showBanner = computed(() => hasFlag.ShowPreCheckinFunctionality && !isPending.value && (data?.value?.showBanner ?? false))

const now = DateTime.now()
const formatted = computed(() => {
  const dateDiff = Math.round(data.value.event?.dueDate.diff(now, 'days').days)

  if (dateDiff <= 0)
    return `Today at 11:59 pm`

  return `${data.value?.event?.dueDate.toFormat('MMM. d, yyyy') ?? ''} at 11:59 pm CST (~${dateDiff} days from now)`
})

async function redirectToCharacterSheet() {
  await router.push({ name: 'characterSheet', params: { id: data.value.character.key } })
}

</script>

<template>
  <div
    v-if="showBanner" class="custom-message m-1 m-md-3 pl-3 pr-3 pt-2 pb-2"
  >
    <div
      class="d-flex align-items-center" role="button"
      @click="showFullMessage = !showFullMessage"
      @keydown.enter="showFullMessage = !showFullMessage"
      @keydown.space.prevent="showFullMessage = !showFullMessage"
    >
      <h3 class="m-0 p-0 flex-fill">
        Early Check In
      </h3>
      <Button :label="showFullMessage ? 'Show Less' : 'Show More'" size="small" @click.prevent />
    </div>
    <div v-if="showFullMessage">
      <p>You have paid for character storage, which enables Early Check In for you. This allows GO's to review, reach out if needed, finalize, and print out your character before con.</p>
      <p>In addition, in the Assigned XP tab, you should now see an "Ongoing Character Storage Bonus" entry worth <strong>5 xp</strong>. This XP amount will be pushed to the next con if you are unable to make it to this one.  You cannot stack other XP bonuses on top of this one for check in.</p>
      <p>The deadline for you to take advantage of Early Check In is <strong>{{ formatted }}</strong></p>
      <div v-if="!data.character">
        <p>You need to select a primary character before you can proceed.</p>
      </div>
      <div v-else>
        <p>To get started, you should review your primary character, and verify that it's good for play.</p>
        <Button :label="'Review ' + data.character.value" @click="redirectToCharacterSheet" />
        <p>Then, once you are happy, hit the 'Understood and Check In' button below, which will kick off the following:</p>
        <ul>
          <li>A GO will be notified that the character is ready to be reviewed</li>
          <li>While it's waiting to be reviewed, you can still make changes to the Character, though it's recommended that you have everything in place.</li>
          <li>Once a GO has some free time, they will review your character and either approve it, or reach out on discord if they can.</li>
          <li>
            Once the character has been approved, a copy of your character will be created, and that copy is what will
            be printed and ready to be picked up at con.
          </li>
          <li>Do note, depending on how things align, you may need to wait a bit for your booklet after check in.</li>
          <li>As per normal, once the CRB is ready for pickup, you will get an email notification denoting it is ready for pickup</li>
        </ul>
        <div v-if="!data?.nextStage?.key">
          <Button label="Understood and Check In" @click="popups.earlyCheckinConfirmation($event)" />
        </div>
        <div v-else>
          <h4>Status</h4>
          <p v-if="data?.nextStage?.key == CheckinStage.GoApproval">
            Character is awaiting GO Approval
          </p>
          <p v-if="data?.nextStage?.key == CheckinStage.CrbPrinted">
            Character has been approved and is awaiting to be printed
          </p>
          <p v-if="data?.nextStage?.key == CheckinStage.CrbAssembled">
            Character has been approved, and printed.  It's awaiting for assembly.
          </p>
          <p v-if="data?.nextStage?.key == CheckinStage.CrbPickedUp">
            Character has been approved, CRB is ready for pickup at SHQ.
          </p>
        </div>
      </div>
    </div>
  </div>
</template>
