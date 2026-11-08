<script setup lang="ts">

import ExpressionSection from '@/components/expressions/ExpressionSection.vue'
import { useRoute } from 'vue-router'
import { expressionStore } from '@/stores/expressionStore'
import { computed, nextTick, onMounted, ref, watch } from 'vue'
import Card from 'primevue/card'
import ScrollTop from 'primevue/scrolltop'
import CreateExpressionSection from '@/components/expressions/CreateExpressionSection.vue'
import Button from 'primevue/button'
import '@he-tree/vue/style/default.css'
import '@he-tree/vue/style/material-design.css'
import ExpressionToC from '@/components/expressions/expressionSection/ExpressionToC.vue'
import axios from 'axios'
import BlessingToC from '@/components/expressions/BlessingToC.vue'
import { can, userPermissionStore } from '@/stores/userPermissionStore.ts'
import { scrollToSection } from '@/components/expressions/expressionUtilities.ts'
import AccordionPanel from 'primevue/accordionpanel'
import AccordionHeader from 'primevue/accordionheader'
import AccordionContent from 'primevue/accordioncontent'
import Accordion from 'primevue/accordion'
import { breakpointsBootstrapV5, useBreakpoints } from '@vueuse/core'

const userPermissionInfo = userPermissionStore()
const permissionCheck = userPermissionInfo.permissionCheck
const expressionInfo = expressionStore()
const route = useRoute()

const isLoading = ref(true)
const showCreate = ref(false)
const showPreview = ref(false)

const activeBreakpoint = useBreakpoints(breakpointsBootstrapV5)
const isMobile = activeBreakpoint.smaller('md')
async function fetchData() {
  isLoading.value = true
  await expressionInfo.getExpressionId(route)

  await expressionInfo.getExpressionSections()
    .then(async () => {
      isLoading.value = false
      if (location.hash) {
        await nextTick()
        scrollToSection(location.hash)
      }
    })
}

function toggleCreate() {
  showCreate.value = !showCreate.value
}

onMounted(async () => {
  await fetchData()
})

const hasBlessingSection = computed(() => {
  const hasBlessingSection = expressionInfo.sections.find(section => section.sectionTypeName === 'Blessings Section')
  return hasBlessingSection !== undefined
})

watch(
  () => route.path,
  async (newPath, oldPath) => {
    if (newPath !== oldPath) {
      await fetchData()
    }
  },
)

watch(
  () => route.hash,
  async (newPath, oldPath) => {
    if (newPath !== oldPath) {
      await nextTick()
      scrollToSection(location.hash)
    }
  },
)

async function downloadExpressionBooklet() {
  const res = await axios.get(`/expression/${expressionInfo.currentExpressionId}/report`, {
    responseType: 'blob',
  })
  const expression = route.params.slug
  const url = URL.createObjectURL(res.data)
  const a = document.createElement('a')
  a.href = url
  a.download = `${expression}-booklet.pdf`
  document.body.appendChild(a)
  a.click()
  a.remove()
  URL.revokeObjectURL(url)
}

const canCreate = computed(() => {
  if (expressionInfo.expressionTypeId == 1 && can.Expression.Create)
    return true

  return (expressionInfo.expressionTypeId == 13 || expressionInfo.expressionTypeId == 14) && can.ContentManagementSystem.Create
})

</script>

<template>
  <div id="expression" class="d-flex flex-column flex-md-row gap-2 ">
    <Accordion v-if="isMobile" :value="[]" class="custom-toc flex-grow-0 sticky-md-top d-print-none zIndexFix">
      <AccordionPanel value="0">
        <AccordionHeader>Table Of Contents</AccordionHeader>
        <AccordionContent>
          <ExpressionToC />
          <BlessingToC v-if="hasBlessingSection" :show-skeleton="isLoading" />
        </AccordionContent>
      </AccordionPanel>
    </Accordion>
    <Card v-else class="custom-toc flex-grow-0 sticky-md-top d-print-none zIndexFix">
      <template #title>
        Table Of Contents
      </template>
      <template #content>
        <article id="expression-toc">
          <ExpressionToC />
          <BlessingToC v-if="hasBlessingSection" :show-skeleton="isLoading" />
        </article>
      </template>
    </Card>
    <Card class="custom-card flex-grow-1">
      <template #content>
        <article id="expression-body">
          <div v-if="permissionCheck.ContentManagementSystem.DownloadReport" class="d-flex flex-row justify-content-end align-items-center">
            <Button label="Download Booklet" @click="downloadExpressionBooklet()" />
          </div>
          <ExpressionSection :sections="expressionInfo.sections" :current-level="1" :show-skeleton="isLoading" :is-read-only="showPreview" @refresh-list="fetchData(route.params.name)" />
          <Button v-if="canCreate && !showPreview" label="Add Section" class="m-2" @click="toggleCreate" />
          <div v-if="showCreate">
            <CreateExpressionSection @cancel-event="toggleCreate" @added-section="fetchData(route.params.name)" />
          </div>
        </article>
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
