<script setup lang="ts">

import Button from 'primevue/button'
import { ref, watch } from 'vue'
import axios from 'axios'
import type { HighLevelExpressionInfoResponse } from '@/components/characters/wizard/basicInfo/types.ts'
import { useRouter } from 'vue-router'
import { cmsStore } from '@/stores/cmsStore.ts'

const cmsInfo = cmsStore()
const router = useRouter()

const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  },
})

const expressionInfo = ref<HighLevelExpressionInfoResponse>({})

async function loadInfo() {
  await axios.get<HighLevelExpressionInfoResponse>(`/characters/options/${props.expressionId}`)
    .then((response) => {
      expressionInfo.value = response.data
    })
}

watch(() => props.expressionId, async (newValue) => {
  await loadInfo()
}, { immediate: true, deep: true })

function redirectToExpression() {
  const currentExpression = cmsInfo.expressionItems.find(x => x.id == props.expressionId)
  const routeData = router.resolve({ name: 'viewExpression', params: { slug: currentExpression.slug } })
  window.open(routeData.href, '_blank')
}

</script>

<template>
  <div>
    <div class="d-flex justify-content-between mt-3">
      <h2 class="m-0 p-0">
        Background
      </h2>
      <div class="d-none d-md-block">
        <Button label="More information" icon="pi pi-external-link" icon-pos="right" @click="redirectToExpression" />
      </div>
      <div class="d-block d-md-none text-right">
        <Button label="" icon="pi pi-external-link" icon-pos="right" @click="redirectToExpression" />
      </div>
    </div>

    <div v-html="expressionInfo.background" />

    <h2>Description</h2>
    <div v-html="expressionInfo.description" />
  </div>
</template>
