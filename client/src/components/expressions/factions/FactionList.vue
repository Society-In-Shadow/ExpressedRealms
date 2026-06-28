<script setup lang="ts">

import { computed, ref } from 'vue'
import Button from 'primevue/button'
import Card from 'primevue/card'
import { can } from '@/stores/userPermissionStore.ts'
import Skeleton from 'primevue/skeleton'
import { useQueryWithLoading } from '@/utilities/queryOverride.ts'
import FactionItem from '@/components/expressions/factions/FactionItem.vue'
import { factionListQuery } from '@/components/expressions/factions/stores/factionStore.ts'
import { factionDialogs } from '@/components/expressions/factions/services/dialogs.ts'

const dialogs = factionDialogs()

const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  },
})

const { data, isLoading, error } = useQueryWithLoading(factionListQuery(props.expressionId))

const showAdd = ref(false)

const toggleAdd = async () => {
  dialogs.showCreateFaction()
}

const enableAdd = computed(() => {
  return can.Faction.Create
})

</script>

<template>
  <div class="d-flex flex-row align-items-center">
    <div class="flex-fill" />
    <div>
      <Button
        v-if="!showAdd && enableAdd" class="w-100 m-2"
        label="Create Faction" @click="toggleAdd"
      />
    </div>
  </div>
  <div v-if="isLoading">
    <Skeleton v-for="height in 3" :key="height" class="mb-3 mt-3" height="100px" />
  </div>
  <div v-else-if="error">
    <Card>
      <template #title>
        Error Loading Factions
      </template>
      <template #content>
        Please try again, or open an issue on discord
      </template>
    </Card>
  </div>
  <div v-else>
    <div v-for="item in data!.factions" :key="item.id">
      <FactionItem :item="item" />
    </div>
  </div>
</template>
