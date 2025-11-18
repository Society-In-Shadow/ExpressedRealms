<script setup lang="ts">

import { AssignedXpStore } from '@/components/admin/assignedXp/stores/assignedXpStore'
import { computed, defineProps, onBeforeMount, ref } from 'vue'
import { UserRoles, userStore } from '@/stores/userStore'
import Button from 'primevue/button'

import type { AssignedXpInfo } from '@/components/admin/assignedXp/types.ts'
import AssignedXpItem from '@/components/admin/assignedXp/AssignedXpItem.vue'
import AddAssignedXp from '@/components/admin/assignedXp/AddAssignedXp.vue'

const store = AssignedXpStore()
const userInfo = userStore()
const hasEventManagementRole = ref(false)

const props = defineProps({
  characterId: {
    type: Number,
    required: true,
  },
  isReadOnly: {
    type: Boolean,
    required: true,
  },
})

onBeforeMount(async () => {
  await store.getAssignedXp(props.characterId)
  hasEventManagementRole.value = await userInfo.hasUserRole(UserRoles.ManageEventRole)
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
  <div v-for="item in sortedItems" :key="item.id" class="py-3">
    <AssignedXpItem :character-id="props.characterId" :item="item" :is-read-only="props.isReadOnly" />
  </div>

  <AddAssignedXp v-if="showAdd && hasEventManagementRole && !props.isReadOnly" :character-id="props.characterId" @canceled="toggleAdd" />
  <Button
    v-if="!showAdd && hasEventManagementRole && !props.isReadOnly" class="w-100 m-2"
    label="Add XP" @click="toggleAdd"
  />
</template>
