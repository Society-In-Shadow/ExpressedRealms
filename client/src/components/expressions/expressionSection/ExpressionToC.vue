<script setup lang="ts">

import { makeIdSafe } from '@/utilities/stringUtilities'
import Skeleton from 'primevue/skeleton'
import { BaseTree, Draggable } from '@he-tree/vue'
import { computed, ref, toRaw } from 'vue'
import Button from 'primevue/button'
import axios from 'axios'
import toaster from '@/services/Toasters'
import { expressionStore } from '@/stores/expressionStore'
import { getIdsWithDynamicSortForArray, scrollToSection } from '@/components/expressions/expressionUtilities'
import { can } from '@/stores/userPermissionStore.ts'

const expressionInfo = expressionStore()

const emit = defineEmits<{
  togglePreview: []
}>()

let originalModel

const showSkeleton = computed(() => { return !expressionInfo.isDoneLoading })

const canEdit = computed(() => {
  if (expressionInfo.expressionTypeId == 1 && can.Expression.Edit)
    return true

  return (expressionInfo.expressionTypeId == 13 || expressionInfo.expressionTypeId == 14) && can.ContentManagementSystem.Edit
})

function saveChanges() {
  axios.put(`/expression/${expressionInfo.currentExpressionId}/updateHierarchy`, {
    expressionId: expressionInfo.currentExpressionId,
    items: getIdsWithDynamicSortForArray(expressionInfo.sections, null),
  }).then(() => {
    emit('togglePreview')
    showTocEdit.value = !showTocEdit.value
    toaster.success('Successfully Updated Expression Tree!')
  })
}

const showTocEdit = ref(false)
function toggleEdit() {
  if (!showTocEdit.value)
    originalModel = JSON.parse(JSON.stringify(toRaw(expressionInfo.sections)))

  if (showTocEdit.value)
    expressionInfo.sections = originalModel

  emit('togglePreview')
  showTocEdit.value = !showTocEdit.value
}

</script>

<template>
  <Draggable
    v-if="canEdit && showTocEdit"
    v-model="expressionInfo.sections"
    class="mtl-tree"
    children-key="subSections"
    update-behavior="new"
    text-key="name"
  >
    <template #default="{ node }">
      <div class="p-1">
        <i class="pi pi-bars mr-2" />{{ node.name }}
      </div>
    </template>
  </Draggable>
  <BaseTree
    v-else
    v-model="expressionInfo.sections"
    children-key="subSections"
  >
    <template #default="{ node }">
      <Skeleton v-if="showSkeleton" id="toc-skeleton" class="mb-2" height="1.5em" />
      <a v-else class="p-1 tocItem" :href="'#' + makeIdSafe(node.name)" @click.prevent="scrollToSection(node.name)">{{ node.name }}</a>
    </template>
  </BaseTree>
  <div v-if="canEdit">
    <Button v-if="showTocEdit" label="Save" class="mt-2 w-100" @click="saveChanges" />
    <Button :label="showTocEdit ? 'Cancel' : 'Edit Order'" class="mt-2 w-100" @click="toggleEdit" />
  </div>
</template>

<style>

.he-tree-drag-placeholder {
  background: var(--p-form-field-disabled-background) !important;
  border: 1px dashed var(--p-button-primary-background);
  height: 1.5em;
  width: 100%;
}

.mtl-tree .tree-node:hover {
  background-color: var(--p-form-field-disabled-background);
  cursor: move;
}

.mtl-tree .tree-node-inner {
  display: flex;
  align-items: center;
  font-size: inherit;
}

</style>

<style scoped>

.tocItem{
  text-decoration: none;
  display: block;
  color: inherit;
}

.tocItem:hover {
  background: var(--p-form-field-disabled-background);
  cursor: pointer;
}

</style>
