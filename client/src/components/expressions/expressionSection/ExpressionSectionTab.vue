<script setup lang="ts">
import ExpressionSection from '@/components/expressions/ExpressionSection.vue'
import { onBeforeRouteUpdate, useRoute } from 'vue-router'
import { expressionStore } from '@/stores/expressionStore'
import { computed, nextTick, onMounted, ref, watch } from 'vue'
import CreateExpressionSection from '@/components/expressions/CreateExpressionSection.vue'
import Button from 'primevue/button'
import '@he-tree/vue/style/default.css'
import '@he-tree/vue/style/material-design.css'
import { can } from '@/stores/userPermissionStore.ts'
import { scrollToSection } from '@/components/expressions/expressionUtilities.ts'

const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  },
})

const expressionInfo = expressionStore()
const route = useRoute()

const isLoading = ref(true)
const showCreate = ref(false)
const showPreview = ref(false)

async function fetchData() {
  isLoading.value = true
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
      scrollToSection(location.hash)
    }
  },
)

const canCreate = computed(() => {
  if (expressionInfo.expressionTypeId == 1 && can.Expression.Create)
    return true

  return (expressionInfo.expressionTypeId == 13 || expressionInfo.expressionTypeId == 14) && can.ContentManagementSystem.Create
})

</script>

<template>
  <article id="expression-body">
    <ExpressionSection :sections="expressionInfo.sections" :current-level="1" :show-skeleton="isLoading" :is-read-only="showPreview" @refresh-list="fetchData(route.params.name)" />
    <Button v-if="canCreate && !showPreview" label="Add Section" class="m-2" @click="toggleCreate" />
    <div v-if="showCreate">
      <CreateExpressionSection @cancel-event="toggleCreate" @added-section="fetchData(route.params.name)" />
    </div>
  </article>
</template>
