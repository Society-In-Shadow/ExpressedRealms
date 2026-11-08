<script setup lang="ts">

import Tabs from 'primevue/tabs'
import TabList from 'primevue/tablist'
import Tab from 'primevue/tab'
import TabPanels from 'primevue/tabpanels'
import TabPanel from 'primevue/tabpanel'
import { onBeforeRouteUpdate, useRoute, useRouter } from 'vue-router'
import { expressionStore } from '@/stores/expressionStore'
import { computed, nextTick, onMounted, ref, watch } from 'vue'
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
import ExpressionToC from '@/components/expressions/expressionSection/ExpressionToC.vue'
import PowersToC from '@/components/expressions/PowersToC.vue'
import AccordionHeader from 'primevue/accordionheader'
import AccordionContent from 'primevue/accordioncontent'
import AccordionPanel from 'primevue/accordionpanel'
import Accordion from 'primevue/accordion'
import { breakpointsBootstrapV5, useBreakpoints } from '@vueuse/core'
import FactionToC from '@/components/expressions/factions/FactionToC.vue'

const expressionInfo = expressionStore()
const route = useRoute()
const router = useRouter()

const activeBreakpoint = useBreakpoints(breakpointsBootstrapV5)
const isMobile = activeBreakpoint.smaller('md')

const currentTab = ref('background')

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
      scrollToSection(location.hash)
    }
  },
)

const routeSection = computed(() => {
  const section = route.query.section

  return typeof section === 'string' ? section : null
})

const isValidRouteSection = computed(() =>
  ['background', 'factions', 'powers', 'progressions'].includes(routeSection.value ?? ''),
)

watch(
  () => route.query.section,
  (section) => {
    if (isValidRouteSection.value) {
      currentTab.value = section as string
    }
  },
  { immediate: true },
)

watch(currentTab,
  async (tab) => {
    await router.push({
      query: {
        ...route.query,
        section: tab,
      },
      hash: '',
    })
  })

</script>

<template>
  <div class="d-flex flex-column flex-md-row gap-2">
    <Accordion v-if="isMobile" :value="[]" class="custom-toc flex-grow-0 sticky-md-top d-print-none zIndexFix">
      <AccordionPanel value="0">
        <AccordionHeader>Table Of Contents</AccordionHeader>
        <AccordionContent>
          <ExpressionToC v-if="currentTab == 'background'" />
          <FactionToC v-else-if="currentTab == 'factions'" />
          <PowersToC v-else :show-skeleton="false" />
        </AccordionContent>
      </AccordionPanel>
    </Accordion>
    <Card v-else class="custom-toc flex-grow-0 sticky-md-top d-print-none zIndexFix">
      <template #title>
        Table Of Contents
      </template>
      <template #content>
        <article id="expression-toc">
          <ExpressionToC v-if="currentTab == 'background'" />
          <FactionToC v-else-if="currentTab == 'factions'" />
          <PowersToC v-else :show-skeleton="false" />
        </article>
      </template>
    </Card>
    <Card class="custom-card flex-grow-1">
      <template #content>
        <ExpressionHeader :expression-id="expressionInfo.currentExpressionId" />
        <Tabs v-model:value="currentTab">
          <TabList>
            <Tab value="background">
              Background
            </Tab>
            <Tab value="factions">
              Factions
            </Tab>
            <Tab value="powers">
              Powers
            </Tab>
            <Tab v-if="can.ProgressionPath.View" value="progressions">
              Progressions
            </Tab>
          </TabList>
          <TabPanels v-if="expressionInfo.currentExpressionId != 0">
            <TabPanel v-if="currentTab == 'background'" value="background">
              <ExpressionSectionTab :expression-id="expressionInfo.currentExpressionId" />
            </TabPanel>
            <TabPanel v-if="currentTab == 'powers'" value="powers">
              <PowerTab :expression-id="expressionInfo.currentExpressionId" />
            </TabPanel>
            <TabPanel v-if="currentTab == 'progressions' && can.ProgressionPath.View" value="progressions">
              <ProgressionTab :expression-id="expressionInfo.currentExpressionId" />
            </TabPanel>
            <TabPanel v-if="currentTab == 'factions'" value="factions">
              <FactionTab :expression-id="expressionInfo.currentExpressionId" />
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
