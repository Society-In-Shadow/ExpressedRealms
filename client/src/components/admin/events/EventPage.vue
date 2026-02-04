<script setup lang="ts">

import { onBeforeMount, ref } from 'vue'
import { UserRoles, userStore } from '@/stores/userStore'
import Card from 'primevue/card'
import EditEvent from '@/components/admin/events/EditEvent.vue'
import { EventStore } from '@/components/admin/events/stores/eventStore.ts'
import { useRoute } from 'vue-router'
import EventScheduledItemList from '@/components/admin/eventScheduleItems/EventScheduledItemList.vue'
import TabPanel from 'primevue/tabpanel'
import TabList from 'primevue/tablist'
import Tab from 'primevue/tab'
import Tabs from 'primevue/tabs'
import TabPanels from 'primevue/tabpanels'
import Tag from 'primevue/tag'
import Button from 'primevue/button'
import { EventConfirmationPopup } from '@/components/admin/events/services/eventConfirmationPopupService.ts'
import SkeletonWrapper from '@/FormWrappers/SkeletonWrapper.vue'
import CharacterActivity from '@/components/admin/events/CharacterActivity.vue'
import { formatDate } from '@/utilities/dateUtilities.ts'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'
import EventQuestionList from '@/components/admin/eventQuestions/EventQuestionList.vue'

let userInfo = userStore()
const route = useRoute()
const eventData = EventStore()
const permissionData = userPermissionStore()
const permissionCheck = permissionData.permissionCheck
const eventId = Number.parseInt(route.params.id as string)

const event = ref<EditEvent>({})

const hasManageEventRole = ref(false)
const isLoaded = ref(false)

onBeforeMount(async () => {
  event.value = await eventData.getEvent(eventId)

  hasManageEventRole.value = await userInfo.hasUserRole(UserRoles.ManageEventRole)
  isLoaded.value = true
})

let popups = EventConfirmationPopup(event.value.id, event.value.name)

</script>

<template>
  <Card>
    <template #content>
      <div class="pb-5 d-flex flex-row justify-content-between">
        <div>
          <h1 class="p-0 m-0">
            <SkeletonWrapper :show-skeleton="!isLoaded" height="1.2em" width="8em" class="mb-1">
              {{ event?.name }} <Tag v-if="hasManageEventRole" severity="secondary">
                {{ event?.isPublished ? "Published" : "Draft" }}
              </Tag>
            </SkeletonWrapper>
          </h1>
          <div class="p-0 m-0">
            <SkeletonWrapper :show-skeleton="!isLoaded" height="1.2em">
              <div>{{ formatDate(event.startDate) }} - {{ formatDate(event.endDate) }}</div>
            </SkeletonWrapper>
          </div>
        </div>
        <div v-if="isLoaded">
          <Button v-if="!event.isPublished" class="mr-2" severity="info" label="Publish" @click="popups.publishConfirmation($event)" />
          <Button class="mr-2" severity="danger" label="Delete" @click="popups.deleteConfirmation($event)" />
        </div>
      </div>
      <Tabs value="0" scrollable>
        <TabList>
          <Tab value="0">
            Basic Info
          </Tab>
          <Tab v-if="permissionCheck.EventQuestion.View" value="3">
            Questions
          </Tab>
          <Tab value="1">
            Schedule
          </Tab>
          <Tab value="2">
            Character Activity
          </Tab>
        </TabList>
        <TabPanels>
          <TabPanel value="0">
            <EditEvent :event-id="eventId" />
          </TabPanel>
          <TabPanel value="1">
            <EventScheduledItemList :event-id="eventId" :is-read-only="false" />
          </TabPanel>
          <TabPanel value="2">
            <CharacterActivity :event-id="eventId" />
          </TabPanel>
          <TabPanel v-if="permissionCheck.EventQuestion.View" value="3">
            <EventQuestionList :event-id="eventId" />
          </TabPanel>
        </TabPanels>
      </Tabs>
    </template>
  </Card>
</template>
