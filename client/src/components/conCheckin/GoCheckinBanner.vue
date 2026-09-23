<script setup lang="ts">

import Button from 'primevue/button'
import { useRouter } from 'vue-router'
import { computed, onBeforeMount, ref } from 'vue'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'

const router = useRouter()
const permissionInfo = userPermissionStore()
const permissionCheck = permissionInfo.permissionCheck

const eventCheckinInfo = EventCheckinStore()

const hasCheckinPermission = ref(false)

onBeforeMount(async () => {
  await eventCheckinInfo.getCheckinAvailable()
  hasCheckinPermission.value = permissionCheck.Event.Checkin
})

const showBanner = computed(() => eventCheckinInfo.hasActiveEvent && hasCheckinPermission.value)

async function redirectToCheckinDetails() {
  await router.push({ name: 'gocheckin' })
}

</script>

<template>
  <div
    v-if="showBanner" class="custom-message m-1 m-md-3 pl-3 pr-3 pt-2 pb-2" role="button"
    @click="redirectToCheckinDetails"
    @keydown.enter="redirectToCheckinDetails"
    @keydown.space.prevent="redirectToCheckinDetails"
  >
    <div class="d-flex align-items-center">
      <h3 class="m-0 p-0 flex-fill">
        Check-in User
      </h3>
      <Button label="Checkin" size="small" @click.prevent />
    </div>
  </div>
</template>

<style>

.custom-message {
  cursor: pointer;
}

</style>
