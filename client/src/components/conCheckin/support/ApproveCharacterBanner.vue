<script setup lang="ts">

import { computed, onBeforeMount, ref, watch } from 'vue'

import Checkbox from 'primevue/checkbox'
import Message from 'primevue/message'
import Button from 'primevue/button'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'
import { characterStore } from '@/components/characters/character/stores/characterStore.ts'
import { useRoute } from 'vue-router'
import { pickedFactionQuery } from '@/components/characters/wizard/factions/stores/factionStore.ts'
import { useQuery } from '@pinia/colada'
import { goCheckList } from '@/components/conCheckin/stores/goStore.ts'
import type { GoChecksResponse } from '@/components/conCheckin/types.ts'

const eventCheckinInfo = EventCheckinStore()
const permissionInfo = userPermissionStore()
const characterInfo = characterStore()
const permissionCheck = permissionInfo.permissionCheck
const reviewedFactionPromotionRequest = ref(false)
const reviewedDealWithDevil = ref(false)
const hasCheckinPermission = ref(false)
const route = useRoute()

onBeforeMount(async () => {
  await eventCheckinInfo.getCheckinAvailable()
  hasCheckinPermission.value = permissionCheck.Event.GoApproval
})

const showBanner = computed(() => eventCheckinInfo.hasActiveEvent && hasCheckinPermission.value
  && characterInfo.isPrimaryCharacter && route.query.src == 'approve_character')

const { data: characterData, isLoading: characterDataLoading } = useQuery(() => ({
  ...pickedFactionQuery(Number.parseInt(route.params.id)),
  enabled: showBanner.value,
}))

const { data: goCheckData, isLoading: goChecksLoading } = useQuery(() => ({
  ...goCheckList(Number.parseInt(route.params.id)),
  enabled: showBanner.value,
}))

const reviewedCharacter = async () => {
  await eventCheckinInfo.approveCharacterSheet()
}

const showFactionInfo = computed(() => {
  return !characterDataLoading.value && characterData.value && characterData.value.factionLevels.find(x => x.requestedPromotion && x.approvalDate == null)
})

const reviewData = ref<GoChecksResponse | null>(null)

watch(goCheckData, (value) => {
  if (!value)
    return

  reviewData.value = structuredClone(value)
}, { immediate: true })

const showGoLists = computed(() => {
  return !goChecksLoading.value && goCheckData.value
})

const enableReviewButton = computed(() => {
  if (!showGoLists.value)
    return false

  const allKnowledgesReviewed = reviewData.value?.knowledgeChecks?.every(x => x.isReviewed) ?? false
  const allContactsReviewed = reviewData.value?.contacts?.every(x => x.isReviewed) ?? false

  const isBlocked = goCheckData?.value?.stillInCharacterCreation || goCheckData.value?.isLegacyExpression || goCheckData?.value?.spentTooMuchXp
  const factionReviewed = !showFactionInfo.value || reviewedFactionPromotionRequest.value
  const addressedDealWithDevil = goCheckData.value?.dealWithDevil == reviewedDealWithDevil.value

  return factionReviewed && allKnowledgesReviewed && allContactsReviewed && !isBlocked && addressedDealWithDevil
})

</script>

<template>
  <Message v-if="showBanner" severity="warn" class="mb-3">
    <div class="w-100">
      <h2>GO Character Review</h2>
      <p>As a GO you are responsible for making sure any edge cases below are addressed before you let the player play.</p>
      <p>If there is nothing listed, you still need to review the character in general before approving.</p>
      <div v-if="goCheckData?.isLegacyExpression">
        <h2>Using Legacy Content</h2>
        <p>
          Their character is using a legacy expression.  This can happen if their character was marked as a primary character, and
          an expression was recently marked as legacy content.
        </p>
        <p>They either need to migrate to the new expression, or will need to create a new character with an available expression.</p>
        <p>Once they are done, recheck them in</p>
      </div>
      <div v-else-if="goCheckData?.stillInCharacterCreation">
        <h2>In Character Creation</h2>
        <p>Their character is still in character creation, help them finalize it, then recheck them in.</p>
      </div>
      <div v-else>
        <div v-if="goCheckData?.xpSpentPercentage < 25">
          <h2>XP Expenditure</h2>
          <p>
            Sometimes players don't realize that they need to spend more XP after initial character creation, or realize they have XP to spend.
            That said, spending all XP is not required, newer players may choose to play the game before spending more XP.  We do have the ability to
            reprint booklets if they choose to spend more XP after playing for a bit.
          </p>
          <p>Let them know that and make sure it wasn't overlooked</p>
          <p>They have spent {{ goCheckData?.xpSpentPercentage }}% of their XP</p>
        </div>
        <div v-if="goCheckData?.dealWithDevil">
          <h2>Deal with a Devil</h2>
          <Checkbox v-model="reviewedDealWithDevil" input-id="deal-with-devil" class="mr-2" binary />
          <label for="deal-with-devil">There's probably a quest or something that the player needs to be talked through regarding their deal.</label>
        </div>
        <div v-if="goCheckData?.spentTooMuchXp">
          <h2>Spent Too Much XP</h2>
          <p>Somehow they have spent too much XP</p>
          <ul>
            <li>Fix the issue on their character sheet</li>
            <li>If it's an issue with the website, reach out to the Toolkit Director at SHQ or post something in Con Discord Channel</li>
            <li>Then recheck them in or refresh this page.</li>
          </ul>
        </div>
        <div v-if="showFactionInfo">
          <Checkbox v-model="reviewedFactionPromotionRequest" input-id="reviewed" class="mr-2" binary />
          <label for="reviewed">I have addressed their faction promotion request</label>
        </div>
        <div v-if="showGoLists && (reviewData?.contacts?.length ?? 0) > 0">
          <h2>Contacts Review</h2>
          <div v-for="contactCheck in reviewData!.contacts" :key="contactCheck.id" class="pt-1 mb-1">
            <Checkbox v-model="contactCheck.isReviewed" :input-id="'contact-' + contactCheck.id" class="mr-2" binary />
            <label :for="'contact-' + contactCheck.id"><strong>{{ contactCheck.name }}</strong> - Needs Approval</label>
          </div>
        </div>
        <div v-if="showGoLists && (goCheckData?.knowledgeChecks?.length ?? 0) > 0">
          <h2>Knowledge Review</h2>
          <div v-for="knowledge in reviewData!.knowledgeChecks" :key="knowledge.id" class="pt-1 mb-1">
            <Checkbox v-model="knowledge.isReviewed" :input-id="'knowledge-' + knowledge.id" class="mr-2" binary />
            <label :for="'knowledge-' + knowledge.id"><strong>{{ knowledge.name }}</strong> -
              <span v-if="knowledge.isDoctorateLevel">Is at a doctorate level, needs a quest or approval of a quest</span>
              <span v-if="knowledge.isUnknownKnowledge">Is an unknown knowledge, these are hidden and need to be approved by a GO</span>
            </label>
          </div>
        </div>
        <Button label="Reviewed Character" class="mt-3" :disabled="!enableReviewButton" @click="reviewedCharacter" />
      </div>
    </div>
  </Message>
</template>
