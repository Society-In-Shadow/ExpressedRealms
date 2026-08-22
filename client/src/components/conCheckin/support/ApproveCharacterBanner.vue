<script setup lang="ts">

import { computed, onBeforeMount, ref } from 'vue'

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

const eventCheckinInfo = EventCheckinStore()
const permissionInfo = userPermissionStore()
const characterInfo = characterStore()
const permissionCheck = permissionInfo.permissionCheck
const reviewedContacts = ref(false)
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

const showKnowledgeList = computed(() => {
  return !goChecksLoading.value && goCheckData.value
})

const enableReviewButton = computed(() => {
  if (!showKnowledgeList.value)
    return false

  const allKnowledgesReviewed = goCheckData!.value!.knowledgeChecks!.every(x => x.isReviewed)

  return reviewedContacts.value
    && (!showFactionInfo.value || reviewedFactionPromotionRequest.value) && allKnowledgesReviewed
})

</script>

<template>
  <Message v-if="showBanner" severity="warn" class="mb-3">
    <div class="w-100">
      <p>You need to review this character sheet.</p>
      <div class="pb-3">
        <Checkbox v-model="reviewedContacts" input-id="reviewed" class="mr-2" binary />
        <label for="reviewed">I have reviewed all contacts</label>
      </div>
      <div v-if="showFactionInfo">
        <Checkbox v-model="reviewedFactionPromotionRequest" input-id="reviewed" class="mr-2" binary />
        <label for="reviewed">I have addressed their faction promotion request</label>
      </div>
      <div v-if="showKnowledgeList">
        <h2>Knowledge Review</h2>
        <div v-for="knowledge in goCheckData!.knowledgeChecks" :key="knowledge.id" class="pt-1 mb-1">
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
