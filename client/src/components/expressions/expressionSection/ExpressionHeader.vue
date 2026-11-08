<script setup lang="ts">

import axios from 'axios'
import { onBeforeRouteUpdate, useRoute } from 'vue-router'
import { expressionStore } from '@/stores/expressionStore'
import { nextTick, onMounted, ref, watch } from 'vue'
import CreateExpressionSection from '@/components/expressions/CreateExpressionSection.vue'
import Button from 'primevue/button'
import '@he-tree/vue/style/default.css'
import '@he-tree/vue/style/material-design.css'
import EditExpressionSection from '@/components/expressions/EditExpressionSection.vue'
import ExpressionLogo from '@/components/common/ExpressionLogo.vue'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'
import { scrollToSection } from '@/components/expressions/expressionUtilities.ts'

const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  },
})

const userPermissionInfo = userPermissionStore()
const permissionCheck = userPermissionInfo.permissionCheck

const expressionInfo = expressionStore()
const route = useRoute()

const expressionHeader = ref({})

const headerIsLoading = ref(true)
const showPreview = ref(false)

async function fetchData() {
  headerIsLoading.value = true
  await axios.get(`/expressionSubSections/${expressionInfo.currentExpressionId}/expression`)
    .then(async (json) => {
      expressionHeader.value = json.data
      headerIsLoading.value = false
    })
}

onMounted(async () => {
  await expressionInfo.getExpressionId(route)
  await fetchData()
})

onBeforeRouteUpdate(async (to, from) => {
  if (to.params.slug !== from.params.slug) {
    await expressionInfo.getExpressionId(to)
    await fetchData()
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

async function downloadExpressionBooklet() {
  const res = await axios.get(`/expression/${expressionInfo.currentExpressionId}/booklet`, {
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

</script>

<template>
  <div class="pb-4">
    <div class="d-flex flex-column flex-md-row">
      <div class="col-12 order-1 order-md-0 col-md-8">
        <div v-if="permissionCheck.Expression.DownloadBooklet" class="d-flex flex-row justify-content-end align-items-center">
          <Button label="Download Booklet" @click="downloadExpressionBooklet()" />
        </div>
        <CreateExpressionSection v-if="!expressionHeader" :add-expression-header="true" @added-section="fetchData(route.params.name)" />
        <EditExpressionSection
          v-else :section-info="expressionHeader" :current-level="1" :show-skeleton="headerIsLoading" :is-read-only="showPreview"
          :is-header-section="true"
        />
      </div>
      <div class="col-12 align-self-center text-center order-0 order-md-1 col-md-4">
        <ExpressionLogo :expression-sub-type-id="expressionInfo.expressionSubTypeId" :is-loading="headerIsLoading" />
      </div>
    </div>
  </div>
</template>
