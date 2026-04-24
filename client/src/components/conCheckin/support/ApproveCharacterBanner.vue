<script setup lang="ts">

import { computed, onBeforeMount, ref } from 'vue'

import Checkbox from 'primevue/checkbox'
import Message from 'primevue/message'
import Button from 'primevue/button'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'
import { characterStore } from '@/components/characters/character/stores/characterStore.ts'
import { useRoute } from 'vue-router'

const eventCheckinInfo = EventCheckinStore()
const permissionInfo = userPermissionStore()
const characterInfo = characterStore()
const permissionCheck = permissionInfo.permissionCheck
const reviewedContacts = ref(false)
const hasCheckinPermission = ref(false)
const route = useRoute()

onBeforeMount(async () => {
  await eventCheckinInfo.getCheckinAvailable()
  hasCheckinPermission.value = permissionCheck.Event.GoApproval
})

const showBanner = computed(() => eventCheckinInfo.hasActiveEvent && hasCheckinPermission.value
  && characterInfo.isPrimaryCharacter && route.query.src == 'approve_character')

const reviewedCharacter = async () => {
  await eventCheckinInfo.approveCharacterSheet()
}
</script>

<template>
  <Message v-if="showBanner" severity="warn" class="mb-3">
    <div class="w-100">
      <p>You need to review this character sheet.</p>
      <div>
        <Checkbox v-model="reviewedContacts" input-id="reviewed" class="mr-2" binary />
        <label for="reviewed">I have reviewed all contacts</label>
      </div>

      <Button label="Reviewed Character" class="mt-3" :disabled="!reviewedContacts" @click="reviewedCharacter" />
    </div>
  </Message>
</template>
