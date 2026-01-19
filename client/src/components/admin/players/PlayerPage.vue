<script setup lang="ts">

import { computed, onBeforeMount, ref } from 'vue'
import { UserRoles, userStore } from '@/stores/userStore'
import Card from 'primevue/card'
import { useRoute } from 'vue-router'
import TabPanel from 'primevue/tabpanel'
import TabList from 'primevue/tablist'
import Tab from 'primevue/tab'
import Tabs from 'primevue/tabs'
import TabPanels from 'primevue/tabpanels'
import Button from 'primevue/button'
import SkeletonWrapper from '@/FormWrappers/SkeletonWrapper.vue'
import ActivityLogs from '@/components/admin/players/tiles/ActivityLogs.vue'
import PlayerRoles from '@/components/admin/players/tiles/PlayerRoles.vue'
import { PlayerStore } from '@/components/admin/players/stores/playerStore'
import { userConfirmationPopups } from '@/components/admin/players/services/playerConfirmationPopupService.ts'
import { formatDistance } from 'date-fns/formatDistance'
import Tag from 'primevue/tag'

let userInfo = userStore()
const route = useRoute()
const playerData = PlayerStore()
const userId = route.params.id as string

const hasManageEventRole = ref(false)
const isLoaded = ref(false)

onBeforeMount(async () => {
  playerData.getPlayer(userId)

  hasManageEventRole.value = await userInfo.hasUserRole(UserRoles.ManageEventRole)
  isLoaded.value = true
})

const userConfirmations = userConfirmationPopups(userId)

const timeTillLockoutExpires = computed(() => {
  if (playerData.player.lockedOut) {
    return formatDistance(new Date(playerData.player.lockedOutExpires), new Date(), { includeSeconds: true })
  }
  return ''
})

</script>

<template>
  <Card>
    <template #content>
      <div class="pb-5 d-flex flex-row justify-content-between">
        <div>
          <h1 class="p-0 m-0">
            <SkeletonWrapper :show-skeleton="!isLoaded" height="1.2em" width="8em" class="mb-1">
              {{ playerData.player?.username }}
              <span>
                <Tag v-if="playerData.player.emailVerified" value="Email Verified" />
                <Tag v-if="playerData.player.isDisabled" severity="danger" value="Disabled" />
                <Tag v-else-if="playerData.player.lockedOut" severity="warn" :value="'Locked Out for ' + timeTillLockoutExpires" />
              </span>
            </SkeletonWrapper>
          </h1>
          <div class="p-0 m-0">
            <SkeletonWrapper :show-skeleton="!isLoaded" height="1.2em">
              <div>{{ playerData.player?.email }}</div>
            </SkeletonWrapper>
          </div>
        </div>
        <div v-if="isLoaded">
          <div class="flex flex-column">
            <Button v-if="playerData.player.isDisabled" label="Enable Account" class="m-2" @click="userConfirmations.enableConfirmation($event)" />
            <Button v-else-if="!playerData.player.isDisabled" severity="danger" label="Disable Account" class="m-2" @click="userConfirmations.deleteConfirmation($event)" />
            <Button v-else-if="playerData.player.lockedOut" label="Unlock Account" class="m-2" @click="userConfirmations.unlockConfirmation($event)" />
            <Button v-if="!playerData.player?.emailConfirmed" severity="warn" :label=" 'Bypass Email Confirmation'" class="m-2" @click="userConfirmations.bypassConfirmation($event)" />
          </div>
        </div>
      </div>
      <Tabs value="0" scrollable>
        <TabList>
          <Tab value="0">
            Basic Info
          </Tab>
          <Tab value="1">
            Roles
          </Tab>
          <Tab value="2">
            Activity Logs
          </Tab>
        </TabList>
        <TabPanels>
          <TabPanel value="0">
            <h3>No Basic Info</h3>
          </TabPanel>
          <TabPanel value="1">
            <PlayerRoles :user-id="userId" />
          </TabPanel>
          <TabPanel value="2">
            <ActivityLogs :user-id="userId" />
          </TabPanel>
        </TabPanels>
      </Tabs>
    </template>
  </Card>
</template>
