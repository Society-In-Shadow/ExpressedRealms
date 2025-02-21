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

const value = ref(0);

const showInfo = ref(false);

const props = defineProps({
  playerInfo: {
    type: Object as PropType<PlayerListItem>,
    required: true
  }
});

</script>

<template>
  <Card class="m-3">
    <template #content>
      <div class="row">
        <div class="col">
          <h2 class="p-0 m-0">
            {{ props.playerInfo.username }}
          </h2>
        </div>
        <div class="col text-end">
          <Button label="Edit" class="m-2" @click="showInfo = !showInfo" />
        </div>
      </div>
      <div class="row">
        <div class="col">
          {{ props.playerInfo.email }}
        </div>
      </div>
      <div class="row" v-if="showInfo">
        <div class="col">
          <Tabs value="0" >
            <TabList>
              <Tab value="0">User Policies</Tab>
            </TabList>
            <TabPanels>
              <TabPanel value="0">
                <PlayerRoles :user-id="props.playerInfo.id"></PlayerRoles>
              </TabPanel>
              <TabPanel value="1">
                <p class="m-0">
                  Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim
                  ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Consectetur, adipisci velit, sed quia non numquam eius modi.
                </p>
              </TabPanel>
              <TabPanel value="2">
                <p class="m-0">
                  At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa
                  qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus.
                </p>
              </TabPanel>
            </TabPanels>
          </Tabs>
        </div>
      </div>
    </template>
  </Card>
</template>

