<script setup lang="ts">

import Tabs from 'primevue/tabs'
import TabList from 'primevue/tablist'
import Tab from 'primevue/tab'
import TabPanels from 'primevue/tabpanels'
import TabPanel from 'primevue/tabpanel'
import { onBeforeRouteUpdate, useRoute } from 'vue-router'
import { expressionStore } from '@/stores/expressionStore'
import { nextTick, onMounted, ref, watch } from 'vue'
import Card from 'primevue/card'
import ScrollTop from 'primevue/scrolltop'
import '@he-tree/vue/style/default.css'
import '@he-tree/vue/style/material-design.css'
import PowerTab from '@/components/expressions/powers/PowerTab.vue'
import ProgressionTab from '@/components/expressions/progressionPaths/ProgressionTab.vue'
import { can } from '@/stores/userPermissionStore.ts'
import FactionTab from '@/components/expressions/factions/FactionTab.vue'
import { scrollToSection } from '@/components/expressions/expressionUtilities.ts'
import ExpressionSectionTab from '@/components/expressions/expressionSection/ExpressionSectionTab.vue'
import ExpressionHeader from '@/components/expressions/expressionSection/ExpressionHeader.vue'

const expressionInfo = expressionStore()
const route = useRoute()

const currentTab = ref('0')

onMounted(async () => {
  await expressionInfo.getExpressionId(route)
})

onBeforeRouteUpdate(async (to, from) => {
  if (to.params.slug !== from.params.slug) {
    await expressionInfo.getExpressionId(to)
  }
})

watch(
  () => route.hash,
  async (newPath, oldPath) => {
    if (newPath !== oldPath) {
      await nextTick()
      window.location.replace(location.hash)
      scrollToSection(location.hash)
    }
  },
)

</script>

<template>
  <div class="d-flex flex-column flex-md-row gap-2">
    <Card class="custom-toc flex-grow-0 sticky-md-top d-print-none zIndexFix">
      <template #title>
        Table Of Contents
      </template>
      <template #content>
        <article id="expression-toc">
          <!--          <ExpressionToC v-if="currentTab == '0'" v-model="sections" :can-edit="showEdit" :show-skeleton="isLoading" @toggle-preview="togglePreview" />-->
          <!--          <PowersToC v-else :show-skeleton="isLoading" />-->
        </article>
      </template>
    </Card>
    <Card class="custom-card flex-grow-1">
      <template #content>
        <ExpressionHeader :expression-id="expressionInfo.currentExpressionId" />
        <Tabs v-model:value="currentTab">
          <TabList>
            <Tab value="0">
              Background
            </Tab>
            <Tab value="3">
              Factions
            </Tab>
            <Tab value="1">
              Powers
            </Tab>
            <Tab v-if="can.ProgressionPath.View" value="2">
              Progressions
            </Tab>
          </TabList>
          <TabPanels>
            <TabPanel value="0">
              <ExpressionSectionTab :expression-id="expressionInfo.currentExpressionId" />
            </TabPanel>
            <TabPanel value="1">
              <PowerTab v-if="expressionInfo.isDoneLoading" :expression-id="expressionInfo.currentExpressionId" />
            </TabPanel>
            <TabPanel v-if="can.ProgressionPath.View" value="2">
              <ProgressionTab v-if="expressionInfo.isDoneLoading" :expression-id="expressionInfo.currentExpressionId" />
            </TabPanel>
            <TabPanel value="3">
              <FactionTab v-if="expressionInfo.isDoneLoading" :expression-id="expressionInfo.currentExpressionId" />
            </TabPanel>
          </TabPanels>
        </Tabs>
      </template>
    </Card>
  </div>
  <ScrollTop />
</template>

<style>

@media(min-width: 768px){
  .custom-toc {
    max-height: calc(100vh - 1rem);
    overflow-y: auto;
    height:100%;
    min-width: 18em;
  }
}

@media(max-width: 768px){
  .custom-card > .p-card-body{
    padding: 0.75rem !important;
  }

  .custom-toc > .p-card-body{
    padding-left: 1rem !important;
    padding-right: 1rem !important;
  }

  .custom-card .p-tabpanels{
    padding: 0.5rem !important;
  }
}

.zIndexFix {
  z-index: inherit !important;
}

</style>
