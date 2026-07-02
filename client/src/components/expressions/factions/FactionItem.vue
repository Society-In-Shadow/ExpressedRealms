<script setup lang="ts">

import { onMounted, type PropType, ref } from 'vue'
import Card from 'primevue/card'
import { ConfirmationPopup } from './services/confirmationPopupService'
import { can, userPermissionStore } from '@/stores/userPermissionStore.ts'
import type { Faction } from '@/components/expressions/factions/types.ts'
import { factionDialogs } from '@/components/expressions/factions/services/dialogs.ts'
import CommandButton, { type Command } from '@/uiComponents/CommandButton.vue'
import { powerDialogs } from '@/components/expressions/powers/services/dialogs.ts'
import { TargetPowerType } from '@/components/expressions/powers/types.ts'
import Button from 'primevue/button'
import { useQueryWithLoading } from '@/utilities/queryOverride.ts'
import { factionListQuery } from '@/components/expressions/factions/stores/factionStore.ts'
import { expressionStore } from '@/stores/expressionStore.ts'
import PowerCard from '@/components/expressions/powers/PowerCard.vue'

const userPermissionInfo = userPermissionStore()
const permissionCheck = userPermissionInfo.permissionCheck

const dialogs = powerDialogs()
const expressionData = expressionStore()

const props = defineProps({
  item: {
    type: Object as PropType<Faction>,
    required: true,
  },
})

let popups = ConfirmationPopup(props.item.id, props.item.name)
let editItemPopup = factionDialogs()

const { refetch } = useQueryWithLoading(factionListQuery(expressionData.currentExpressionId))

const items = ref<Command[]>([])

onMounted(async () => {
  PopulateFactionActions()
})

const showAddPower = async (levelId: number) => {
  const result = await dialogs.showAddPower({ target: TargetPowerType.FactionLevel, targetId: levelId })

  if (result?.action == 'added') {
    await refetch()
  }
}

function PopulateFactionActions() {
  if (permissionCheck.Faction.Edit) {
    items.value.push({
      label: 'Edit',
      severity: 'primary',
      command: ($event) => {
        editItemPopup.showUpdateFaction(props.item.id)
      },
    })
  }
  if (permissionCheck.Faction.Delete) {
    items.value.push({
      label: 'Delete',
      command: ($event) => {
        popups.deleteConfirmation($event)
      },
      severity: 'danger',
    })
  }
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
          <p>Knowledge in {{ level.knowledge }} at a {{ level.knowledgeLevel }} level with a specialization in {{ level.specialization }}.</p>
        </div>
        <div class="d-flex flex-column flex-md-row align-self-center justify-content-between mt-3">
          <h3 class="p-0 m-0 flex-fill">
            Rank Power
          </h3>
          <div class="p-0 m-0 d-inline-flex align-items-start">
            <Button
              v-if="can.Faction.Edit && !level.power" class="w-100 m-2"
              label="Add Power" size="small" @click="showAddPower(level.id)"
            />
            <Button
              v-if="can.Faction.Edit && level.power" class="w-100 m-2"
              label="Edit Power" size="small" disabled
            />
          </div>
        </div>
        <PowerCard v-if="level.power" :power="level.power" :power-path-id="-1" :is-read-only="true" />
      </div>
    </template>
  </Card>
</template>
