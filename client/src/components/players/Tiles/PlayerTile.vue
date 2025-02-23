<script setup lang="ts">
import { ref } from 'vue'
import Card from "primevue/card";
import type {PlayerListItem} from "@/components/players/Objects/Player";
import type {PropType} from "vue";
import Tabs from 'primevue/tabs';
import TabList from 'primevue/tablist';
import Tab from 'primevue/tab';
import TabPanels from 'primevue/tabpanels';
import TabPanel from 'primevue/tabpanel';
import Button from "primevue/button";
import PlayerRoles from "@/components/players/Tiles/PlayerRoles.vue";
import Tag from 'primevue/tag';
import {fetchUserPolicies} from "@/components/players/Services/PlayerRoleService";
import ActivityLogs from "@/components/players/Tiles/ActivityLogs.vue";

const showInfo = ref(false);

const props = defineProps({
  playerInfo: {
    type: Object as PropType<PlayerListItem>,
    required: true
  }
});

function updatePlayerRoles(){
  fetchUserPolicies(props.playerInfo.id)
      .then(response => {
        props.playerInfo.roles = response.data.roles.filter(x => x.isEnabled).map(x => x.name);
      });
}

</script>

<template>
  <Card class="m-3">
    <template #content>
      <div class="d-flex flex-row">
        <div class="flex-grow-1">
          <div class="row">
            <div class="col">
              <h2 class="d-inline-flex m-0 pr-3">
                {{ props.playerInfo.username }}
              </h2>
              <Tag v-for="role in props.playerInfo.roles" :key="role" class="m-1" :value="role" />
            </div>
          </div>
          <div class="row">
            <div class="col">
              {{ props.playerInfo.email }}
            </div>
          </div>
        </div>
        <div>
          <Button :label="showInfo ? 'Cancel' : 'Edit'" class="m-2" @click="showInfo = !showInfo" />
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
    </template>
  </Card>
</template>

