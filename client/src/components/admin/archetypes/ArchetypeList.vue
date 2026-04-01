<script setup lang="ts">

import { computed, ref } from 'vue'
import Button from 'primevue/button'
import Card from 'primevue/card'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'
import { useQuery } from '@pinia/colada'
import { archetypeListQuery } from '@/components/admin/archetypes/stores/archetypeStore.ts'
import Skeleton from 'primevue/skeleton'
import ArchetypeItem from '@/components/admin/archetypes/ArchetypeItem.vue'
import type { Archetype } from '@/components/admin/archetypes/types.ts'
import { groupBy, mapValues, orderBy } from 'lodash'
import { useRouter } from 'vue-router'

const router = useRouter()
const permissionCheck = userPermissionStore().permissionCheck

const { data, isLoading, error } = useQuery(archetypeListQuery)

const showAdd = ref(false)

const toggleAdd = async () => {
  await router.push({ name: 'addWizard', query: { src: 'archetype_add' } })
}

const enableAdd = computed(() => {
  return permissionCheck.Archetypes.Create
})

const expressions = computed(() => {
  const expressionGroups = groupBy(data.value?.archetypes ?? [], (x: Archetype) => x.expressionName)

  const sortedGroups = mapValues(expressionGroups, (group: Archetype[]) =>
    orderBy(group, (x: Archetype) => x.name),
  )

  return orderBy(sortedGroups, (group: Archetype[]) => group[0].expressionName)
})

</script>

<template>
  <div class="d-flex flex-row align-items-center">
    <div class="flex-fill">
      <h1>Archetypes</h1>
    </div>
    <div>
      <Button
        v-if="!showAdd && enableAdd" class="w-100 m-2"
        label="Create Archetype" @click="toggleAdd"
      />
    </div>
  </div>
  <div v-if="isLoading">
    <Skeleton v-for="height in 3" :key="height" class="mb-3 mt-3" height="100px" />
  </div>
  <div v-else-if="error">
    <Card>
      <template #title>
        Error Loading Archetypes
      </template>
      <template #content>
        Please try again, or open an issue on discord
      </template>
    </Card>
  </div>
  <div v-else>
    <div v-for="(expression, index) in expressions" :key="index">
      <h2>{{ expression[0].expressionName }}</h2>
      <div v-for="archetype in expression" :key="archetype.id" class="py-3">
        <ArchetypeItem :item="archetype" />
      </div>
    </div>
  </div>
</template>
