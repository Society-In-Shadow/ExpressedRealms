<script setup lang="ts">
import { computed, onBeforeMount, type PropType } from 'vue'
import type { PlayerListItem } from '@/components/admin/players/types'
import Tag from 'primevue/tag'
import { formatDistance } from 'date-fns/formatDistance'
import { userConfirmationPopups } from '@/components/admin/players/services/playerConfirmationPopupService'
import Panel from 'primevue/panel'
import { useRouter } from 'vue-router'
import SplitButton from 'primevue/splitbutton'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'

const router = useRouter()
const userPermissionInfo = userPermissionStore()
const permissionCheck = userPermissionInfo.permissionCheck

const props = defineProps({
  playerInfo: {
    type: Object as PropType<PlayerListItem>,
    required: true,
  },
})

const userConfirmations = userConfirmationPopups(props.playerInfo.id)

const timeTillLockoutExpires = computed(() => {
  if (props.playerInfo.lockedOut) {
    return formatDistance(new Date(props.playerInfo.lockedOutExpires), new Date(), { includeSeconds: true })
  }
  return ''
})

const items = []

async function toggleEdit() {
  await router.push({ name: 'editPlayer', params: { id: props.playerInfo.id } })
}

onBeforeMount(async () => {
  if (props.playerInfo.isDisabled && permissionCheck.Player.Enable) {
    items.push(
      {
        label: 'Enable Account',
        command: ($event) => {
          userConfirmations.enableConfirmation($event)
        },
      })
  }
  else if (!props.playerInfo.isDisabled && permissionCheck.Player.Disable) {
    items.push(
      {
        label: 'Disable Account',
        command: ($event) => {
          userConfirmations.deleteConfirmation($event)
        },
      })
  }
  else if (props.playerInfo.lockedOut && permissionCheck.Player.BypassLockout) {
    items.push(
      {
        label: 'Unlock Account',
        command: ($event) => {
          userConfirmations.unlockConfirmation($event)
        },
      })
  }
  if (!props.playerInfo?.emailConfirmed && permissionCheck.Player.BypassEmailConfirmation) {
    items.push({
      label: 'Bypass Email Confirmation',
      command: ($event) => {
        userConfirmations.bypassConfirmation($event)
      },
    })
  }
})

</script>

<template>
  <Panel class="mb-3">
    <template #header>
      <div class="d-flex flex-column flex-md-row flex-grow-1">
        <div class="flex-grow-1">
          <div class="flex-grow-1">
            <div class="d-flex flex-md-row flex-column">
              <div class="d-flex flex-column flex-grow-1">
                <div>
                  <h1 class="m-0 p-0">
                    {{ props.playerInfo.username }}
                    <span>
                      <Tag v-if="props.playerInfo.emailVerified" value="Email Verified" />
                      <Tag v-if="props.playerInfo.isDisabled" severity="danger" value="Disabled" />
                      <Tag v-else-if="props.playerInfo.lockedOut" severity="warn" :value="'Locked Out for ' + timeTillLockoutExpires" />
                    </span>
                  </h1>
                  <Tag v-for="role in props.playerInfo.roles" :key="role" class="m-1" :value="role" />
                </div>
              </div>
            </div>
            <div class="d-flex flex-row align-self-center pt-3 pr-3">
              <div class="flex-grow-1">
                {{ props.playerInfo.email }}
              </div>
            </div>
          </div>
        </div>
        <div>
          <div class="flex flex-column">
            <SplitButton label="View" severity="info" :model="items" @click="toggleEdit()" />
          </div>
        </div>
      </div>
    </template>
  </Panel>
</template>

<style>
.p-panel-header{
  background: var(--p-panel-background) !important;
  border-bottom: 0px !important;
  padding: 1.5em 1.5em 0em !important;
}
</style>
