<script setup lang="ts">

import { onBeforeMount, ref } from 'vue'
import { UserRoles, userStore } from '@/stores/userStore'
import Card from 'primevue/card'
import { RoleStore } from './stores/roleStore.ts'
import { useRoute } from 'vue-router'
import TabPanel from 'primevue/tabpanel'
import TabList from 'primevue/tablist'
import Tab from 'primevue/tab'
import Tabs from 'primevue/tabs'
import TabPanels from 'primevue/tabpanels'
import Button from 'primevue/button'
import { ConfirmationPopup } from './services/confirmationPopupService.ts'
import SkeletonWrapper from '@/FormWrappers/SkeletonWrapper.vue'
import type { EditRole } from './types.ts'
import EditRoleForm from '@/components/admin/roles/EditRoleForm.vue'

let userInfo = userStore()
const route = useRoute()
const roleData = RoleStore()
const roleId = Number.parseInt(route.params.id as string)

const role = ref<EditRole>({})

const hasManageEventRole = ref(false)
const isLoaded = ref(false)

onBeforeMount(async () => {
  role.value = await roleData.getEvent(roleId)

  hasManageEventRole.value = await userInfo.hasUserRole(UserRoles.ManageEventRole)
  isLoaded.value = true
})

let popups = ConfirmationPopup()

</script>

<template>
  <Card>
    <template #content>
      <div class="pb-5 d-flex flex-row justify-content-between">
        <div>
          <h1 class="p-0 m-0">
            <SkeletonWrapper :show-skeleton="!isLoaded" height="1.2em" width="8em" class="mb-1">
              {{ role?.name }}
            </SkeletonWrapper>
          </h1>
          <div class="p-0 m-0">
            <SkeletonWrapper :show-skeleton="!isLoaded" height="1.2em">
              <div>{{ role.description }}</div>
            </SkeletonWrapper>
          </div>
        </div>
        <div v-if="isLoaded">
          <Button class="mr-2" severity="danger" label="Delete" @click="popups.deleteConfirmation($event, role.id, role.name)" />
        </div>
      </div>
      <Tabs value="0" scrollable>
        <TabList>
          <Tab value="0">
            Basic Info
          </Tab>
          <Tab value="1">
            Permissions
          </Tab>
        </TabList>
        <TabPanels>
          <TabPanel value="0">
            <EditRoleForm :role-id="roleId" />
          </TabPanel>
          <TabPanel value="1">
            <h1>Permissions!</h1>
          </TabPanel>
        </TabPanels>
      </Tabs>
    </template>
  </Card>
</template>
