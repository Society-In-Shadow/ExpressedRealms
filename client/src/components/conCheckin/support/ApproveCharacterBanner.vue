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

  return (!showFactionInfo.value || reviewedFactionPromotionRequest.value) && allKnowledgesReviewed && allContactsReviewed
})

</script>

<template>
  <Message v-if="showBanner" severity="warn" class="mb-3">
    <div class="w-100">
      <p>You need to review this character sheet.</p>
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
  </Message>
</template>
