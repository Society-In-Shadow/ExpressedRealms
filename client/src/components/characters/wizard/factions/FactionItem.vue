<script setup lang="ts">

import { onMounted, type PropType, ref } from 'vue'
import Card from 'primevue/card'
import type { Faction } from '@/components/expressions/factions/types.ts'
import CommandButton, { type Command } from '@/uiComponents/CommandButton.vue'
import { TargetPowerType } from '@/components/expressions/powers/types.ts'
import { useQueryWithLoading } from '@/utilities/queryOverride.ts'
import { factionListQuery } from '@/components/expressions/factions/stores/factionStore.ts'
import { expressionStore } from '@/stores/expressionStore.ts'
import PowerCard from '@/components/expressions/powers/PowerCard.vue'
import { characterStore } from '@/components/characters/character/stores/characterStore.ts'
import { pickFaction } from '@/components/characters/wizard/factions/stores/factionStore.ts'

const expressionData = expressionStore()
const characterInfo = characterStore()

const props = defineProps({
  item: {
    type: Object as PropType<Faction>,
    required: true,
  },
})

const { refetch } = useQueryWithLoading(factionListQuery(expressionData.currentExpressionId))
const items = ref<Command[]>([])

onMounted(async () => {
  PopulateFactionActions()
})

const modifiedPower = async () => {
  await refetch()
}

function PopulateFactionActions() {
  if (characterInfo.isOwner) {
    items.value.push({
      label: 'Select',
      severity: 'primary',
      command: async ($event) => {
        const action = pickFaction()
        await action.mutateAsync({ data: { characterId: characterInfo.characterId, factionId: props.item.id } })
      },
    })
  }
}

function article(value: string): 'A' | 'An' {
  return /^[aeiou]/i.test(value) ? 'An' : 'A'
}

</script>

<template>
  <Card>
    <template #content>
      <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
        <h1 class="p-0 m-0 flex-fill">
          {{ props.item?.name }}
        </h1>
        <div class="p-0 m-0 d-inline-flex align-items-start">
          <CommandButton :commands="items" />
        </div>
      </div>
      <div class="p-0 m-0">
        <div v-html="props.item.background" />
      </div>

      <div v-for="level in props.item.factionLevels" :key="level.id">
        <h2>{{ level.rankName }} Rank</h2>
        <h3>Requirements:</h3>
        <div v-if="level.rankName == 'Basic'">
          No Requirements to join
        </div>
        <div v-else>
          <ul>
            <li>{{ article(level.knowledgeLevel) }} "{{ level.knowledgeLevel }}" level in the "{{ level.knowledge }}" knowledge</li>
            <li>Specialization in "{{ level.specialization }}" for above knowledge</li>
            <li>GO approval with the completion of one or more tasks / trials</li>
          </ul>
        </div>
        <PowerCard
          v-if="level.power" :target-type="TargetPowerType.FactionLevel" :power="level.power" :power-path-id="-1" :starting-header="3"
          @modified="modifiedPower"
        />
        <div v-else>
          No Known Powers for this rank
        </div>
      </div>
    </template>
  </Card>
</template>
