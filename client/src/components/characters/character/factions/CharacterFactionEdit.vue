<script setup lang="ts">

import { type PropType } from 'vue'
import Card from 'primevue/card'
import type { Faction, FactionLevel } from '@/components/expressions/factions/types.ts'
import { TargetPowerType } from '@/components/expressions/powers/types.ts'
import { useQueryWithLoading } from '@/utilities/queryOverride.ts'
import { factionListQuery } from '@/components/expressions/factions/stores/factionStore.ts'
import { expressionStore } from '@/stores/expressionStore.ts'
import PowerCard from '@/components/expressions/powers/PowerCard.vue'
import { characterStore } from '@/components/characters/character/stores/characterStore.ts'
import { pickedFactionQuery } from '@/components/characters/wizard/factions/stores/factionStore.ts'
import StatusIcon from '@/components/characters/wizard/factions/StatusIcon.vue'

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

function approvalStatus(level: FactionLevel) {
  if (!data.value) return null
  const currentLevel = data.value!.factionLevels.find(f => f.factionLevelId == level.id)
  const isAwaiting = currentLevel!.approvalDate == null && currentLevel!.requestedPromotion
  if (isAwaiting) {
    return 'Awaiting Promotion'
  }
  else if (currentLevel!.approvalDate != null) {
    return 'Approved'
  }
  return 'Not Approved'
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
        <h2 class="mb-0">
          {{ level.rankName }} Rank
        </h2>
        <h4 class="m-0 p-0">
          <span class="text-color-secondary"><em>({{ approvalStatus(level) }})</em></span>
        </h4>
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
