<script setup lang="ts">
import type { PropType } from 'vue'
import { computed, ref } from 'vue'
import type { PlayerListItem } from '@/components/players/Objects/Player'
import Tabs from 'primevue/tabs'
import TabList from 'primevue/tablist'
import Tab from 'primevue/tab'
import TabPanels from 'primevue/tabpanels'
import TabPanel from 'primevue/tabpanel'
import Button from 'primevue/button'
import PlayerRoles from '@/components/players/Tiles/PlayerRoles.vue'
import Tag from 'primevue/tag'
import { fetchUserPolicies } from '@/components/players/Services/PlayerRoleService'
import ActivityLogs from '@/components/players/Tiles/ActivityLogs.vue'
import { formatDistance } from 'date-fns/formatDistance'
import { userConfirmationPopups } from '@/components/players/Services/PlayerConfirmationPopupService'
import { playerList } from '@/components/players/Stores/PlayerListStore'
import Panel from 'primevue/panel'

const playerListStore = playerList()

const showInfo = ref(false)

const props = defineProps({
  playerInfo: {
    type: Object as PropType<PlayerListItem>,
    required: true,
  },
})

var userConfirmations = userConfirmationPopups(props.playerInfo.id)

function updatePlayerRoles() {
  fetchUserPolicies(props.playerInfo.id)
    .then((response) => {
      playerListStore.fetchPlayers()
    })
}

const timeTillLockoutExpires = computed(() => {
  if (props.playerInfo.lockedOut) {
    return formatDistance(new Date(props.playerInfo.lockedOutExpires), new Date(), { includeSeconds: true })
  }
  return ''
})

</script>

<template>
  <Panel class="mb-3">
    <template #header>
      <h1 class="m-0 p-0">
        {{ props.playerInfo.username }}
      </h1>
    </template>
    <div class="d-flex flex-column flex-md-row">
      <div class="flex-grow-1">
        <div class="flex-grow-1">
          <div class="d-flex flex-md-row flex-column">
            <div class="d-flex flex-column flex-grow-1">
              <div>
                <Tag v-for="role in props.playerInfo.roles" :key="role" class="m-1" :value="role" />
              </div>
            </div>
          </div>
          <div class="d-flex flex-row align-self-center pt-3 pr-3">
            <div class="flex-grow-1">
              {{ props.playerInfo.email }}
            </div>
            <div>
              <Tag v-if="props.playerInfo.emailVerified" value="Email Verified" />
              <Tag v-if="props.playerInfo.isDisabled" severity="danger" value="Disabled" />
              <Tag v-else-if="props.playerInfo.lockedOut" severity="warn" :value="'Locked Out for ' + timeTillLockoutExpires" />
            </div>
          </div>
        </div>
      </div>
      <div>
        <div class="flex flex-column">
          <Button v-if="props.playerInfo.isDisabled" label="Enable Account" class="m-2" @click="userConfirmations.enableConfirmation($event)" />
          <Button v-else-if="!props.playerInfo.isDisabled" severity="danger" label="Disable Account" class="m-2" @click="userConfirmations.deleteConfirmation($event)" />
          <Button v-else-if="props.playerInfo.lockedOut" label="Unlock Account" class="m-2" @click="userConfirmations.unlockConfirmation($event)" />
          <Button :label="showInfo ? 'Cancel' : 'Details'" class="m-2" @click="showInfo = !showInfo" />
          <Button v-if="!props.playerInfo?.emailConfirmed" severity="warn" :label=" 'Bypass Email Confirmation'" class="m-2" @click="userConfirmations.bypassConfirmation($event)" />
        </div>
      </div>
    </div>
    <div v-if="showInfo" class="row">
      <div class="col">
        <Tabs value="0" :lazy="true">
          <TabList>
            <Tab value="0">
              User Policies
            </Tab>
            <Tab value="1">
              Activity Logs
            </Tab>
          </TabList>
          <TabPanels>
            <TabPanel value="0">
              <PlayerRoles :user-id="props.playerInfo.id" @policies-changed="updatePlayerRoles()" />
            </TabPanel>
            <TabPanel value="1">
              <ActivityLogs :user-id="props.playerInfo.id" />
            </TabPanel>
          </TabPanels>
        </Tabs>
      </div>
    </div>
  </Panel>
</template>

<style>
.p-panel-header{
  background: var(--p-panel-background) !important;
  border-bottom: 0px !important;
  padding: 1.5em 1.5em 0em !important;
}
</style>
