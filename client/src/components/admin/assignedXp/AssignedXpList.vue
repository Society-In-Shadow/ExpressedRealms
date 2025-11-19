<script setup lang="ts">

import Skeleton from 'primevue/skeleton'
import { AssignedXpStore } from '@/components/admin/assignedXp/stores/assignedXpStore'
import { computed, inject, onBeforeMount, type Ref, ref } from 'vue'
import { UserRoles, userStore } from '@/stores/userStore'
import Button from 'primevue/button'

import type { AssignedXpInfo } from '@/components/admin/assignedXp/types.ts'
import AssignedXpItem from '@/components/admin/assignedXp/AssignedXpItem.vue'
import AddAssignedXp from '@/components/admin/assignedXp/AddAssignedXp.vue'

const store = AssignedXpStore()
const userInfo = userStore()
const hasManagePlayerExperienceRole = ref(false)

const dialogRef = inject('dialogRef') as Ref
const characterId = ref(dialogRef.value.data.characterId)
const isReadOnly = ref(dialogRef.value.data.isReadOnly)

onBeforeMount(async () => {
  await store.getAssignedXp(characterId.value)
  hasManagePlayerExperienceRole.value = await userInfo.hasUserRole(UserRoles.ManagePlayerExperience)
})

const showAdd = ref(false)

const toggleAdd = () => {
  showAdd.value = !showAdd.value
}

// Create a computed property for sorted Events
const sortedItems = computed<AssignedXpInfo[]>(() => {
  return [...store.assignedXpItems].sort((a, b) => b.dateAssigned - a.dateAssigned)
})

</script>

<template>
  <div v-if="!store.hasItems">
    <Skeleton class="w-100 mb-3" height="5em" />
    <Skeleton class="w-100 mb-3" height="5em" />
    <Skeleton class="w-100 mb-3" height="5em" />
  </div>
  <div v-for="item in sortedItems" v-else :key="item.id" class="py-3">
    <AssignedXpItem :character-id="characterId" :item="item" :is-read-only="isReadOnly" />
  </div>

  <AddAssignedXp v-if="showAdd && hasManagePlayerExperienceRole && !isReadOnly" :character-id="characterId" @canceled="toggleAdd" />
  <Button
    v-if="!showAdd && hasManagePlayerExperienceRole && !isReadOnly" class="w-100 m-2"
    label="Add XP" @click="toggleAdd"
  />
</template>
