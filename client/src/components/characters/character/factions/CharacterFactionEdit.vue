<script setup lang="ts">

import { type PropType } from 'vue'
import Card from 'primevue/card'
import { TargetPowerType } from '@/components/expressions/powers/types.ts'
import { useQueryWithLoading } from '@/utilities/queryOverride.ts'
import { factionListQuery } from '@/components/expressions/factions/stores/factionStore.ts'
import { expressionStore } from '@/stores/expressionStore.ts'
import PowerCard from '@/components/expressions/powers/PowerCard.vue'
import { characterStore } from '@/components/characters/character/stores/characterStore.ts'
import { pickedFactionQuery } from '@/components/characters/wizard/factions/stores/factionStore.ts'
import StatusIcon from '@/components/characters/wizard/factions/StatusIcon.vue'
import { can } from '@/stores/userPermissionStore.ts'
import Button from 'primevue/button'
import { factionDialogs } from '@/components/characters/character/factions/services/dialogs.ts'
import type { Faction, FactionLevel } from '@/components/characters/wizard/factions/types.ts'

const expressionData = expressionStore()
const characterInfo = characterStore()

const props = defineProps({
  item: {
    type: Object as PropType<Faction>,
    required: true,
  },
})

const { refetch } = useQueryWithLoading(factionListQuery(expressionData.currentExpressionId))
const { data } = useQueryWithLoading(pickedFactionQuery(characterInfo.characterId))

const modifiedPower = async () => {
  await refetch()
}

function lookupFactionLevel(level: FactionLevel) {
  if (!data.value) return null
  return data.value!.factionLevels.find(f => f.factionLevelId == level.id)
}

enum ApprovalStatus {
  AwaitingPromotion,
  Approved,
  CanApprove,
  CanRequestPromotion,
  RequirementsNotMet,
}

function getApprovalStatus(level: FactionLevel) {
  if (!data.value) return null
  const currentLevel = data.value!.factionLevels.find(f => f.factionLevelId == level.id)
  const levelIndex = props.item.factionLevels!.indexOf(level)
  const isAwaiting = currentLevel!.approvalDate == null && currentLevel!.requestedPromotion

  const isBasicLevel = levelIndex == 0
  if (isBasicLevel)
    return ApprovalStatus.Approved

  const previousLevel = props.item.factionLevels![levelIndex - 1]
  const previousLevelApproved = (data.value!.factionLevels.find(x => x.factionLevelId == previousLevel.id)!.approvalDate != null)
  const canApprove = previousLevelApproved
    && currentLevel?.hasKnowledge
    && currentLevel?.hasKnowledgeLevel
    && currentLevel?.hasSpecialization
    && !currentLevel.approvalDate

  if (canApprove && can.Faction.ApprovePromotion) {
    return ApprovalStatus.CanApprove
  }

  else if (canApprove)
    return ApprovalStatus.CanRequestPromotion

  else if (isAwaiting) {
    return ApprovalStatus.AwaitingPromotion
  }

  else if (currentLevel!.approvalDate != null) {
    return ApprovalStatus.Approved
  }
  return ApprovalStatus.RequirementsNotMet
}

function approvalStatusDisplay(status: ApprovalStatus | null): string {
  switch (status) {
    case ApprovalStatus.RequirementsNotMet:
      return 'Requirements Not Met'

    case ApprovalStatus.CanApprove:
      return 'Can Approve'

    case ApprovalStatus.AwaitingPromotion:
      return 'Awaiting Promotion'

    case ApprovalStatus.CanRequestPromotion:
      return 'Can Request Promotion'

    case ApprovalStatus.Approved:
      return 'Approved'

    default:
      return ''
  }
}

const dialogs = factionDialogs()

const showApprovePromotion = async (factionLevelId: number) => {
  await dialogs.approvePromotion({ characterId: characterInfo.characterId, factionLevelId: factionLevelId, approvalReason: null })
}

</script>

<template>
  <Card>
    <template #content>
      <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
        <h1 class="p-0 m-0 flex-fill">
          {{ props.item?.name }}
        </h1>
      </div>
      <div class="p-0 m-0">
        <div v-html="props.item.background" />
      </div>

      <div v-for="(level, index) in props.item.factionLevels" :key="level.id">
        <div class="d-flex flex-row">
          <div class="flex-fill">
            <h2 class="mb-0">
              {{ level.rankName }} Rank
            </h2>
            <h4 class="m-0 p-0">
              <span class="text-color-secondary"><em>({{ approvalStatusDisplay(getApprovalStatus(level)) }})</em></span>
            </h4>
          </div>
          <div v-if="getApprovalStatus(level) == ApprovalStatus.CanApprove">
            <Button label="Approve Promotion" @click="showApprovePromotion(level.id)" />
          </div>
        </div>
        <div v-if="lookupFactionLevel(level)?.requestedPromotionReason">
          <h3>Promotion Request</h3>
          <p>{{ lookupFactionLevel(level)?.requestedPromotionReason }}</p>
        </div>
        <div v-if="lookupFactionLevel(level)?.approvalReason">
          <h3>Approval Reason</h3>
          <p>{{ lookupFactionLevel(level)?.approvalReason }}</p>
        </div>
        <h3>Requirements:</h3>
        <div v-if="level.rankName == 'Basic'">
          No Requirements to join
        </div>
        <div v-else>
          <div>
            <div><StatusIcon :value="lookupFactionLevel(props.item.factionLevels[index -1])?.approvalDate" /> - {{ props.item.factionLevels[index -1].rankName }} Rank </div>
            <div><StatusIcon :value="lookupFactionLevel(level)?.hasKnowledge" /> -  Knowledge "{{ level.knowledge }}"</div>
            <div><StatusIcon :value="lookupFactionLevel(level)?.hasKnowledgeLevel" /> - Knowledge Level of "{{ level.knowledgeLevel }}" </div>
            <div><StatusIcon :value="lookupFactionLevel(level)?.hasSpecialization" /> -  Specialization in "{{ level.specialization }}"</div>
          </div>
        </div>
        <div class="pt-3">
          <PowerCard
            v-if="level.power" :target-type="TargetPowerType.FactionLevel" :power="level.power" :power-path-id="-1" :starting-header="3"
            :is-read-only="true" @modified="modifiedPower"
          />
          <div v-else class="pt-3">
            No Known Powers for this rank
          </div>
        </div>
      </div>
    </template>
  </Card>
</template>
