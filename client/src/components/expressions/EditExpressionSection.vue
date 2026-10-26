<script setup lang="ts">

import { makeIdSafe } from '@/utilities/stringUtilities'
import Skeleton from 'primevue/skeleton'
import Button from 'primevue/button'
import { computed, ref } from 'vue'

import { expressionStore } from '@/stores/expressionStore'
import CreateExpressionSection from '@/components/expressions/CreateExpressionSection.vue'
import KnowledgeList from '@/components/knowledges/KnowledgeList.vue'
import BlessingList from '@/components/blessings/BlessingList.vue'
import { can } from '@/stores/userPermissionStore.ts'
import { expressionSectionDialogs } from '@/components/expressions/expressionSection/services/dialog.ts'

const expressionInfo = expressionStore()
const dialogs = expressionSectionDialogs()

const emit = defineEmits<{
  refreshList: []
}>()

const props = defineProps({
  sectionInfo: {
    type: Object,
    required: true,
  },
  currentLevel: {
    type: Number,
    required: true,
  },
  showSkeleton: {
    type: Boolean,
    required: true,
  },
  isReadOnly: {
    type: Boolean,
    required: true,
  },
  isHeaderSection: {
    type: Boolean,
  },
})

const showCreate = ref(false)

function passThroughAddedSection() {
  emit('refreshList')
}

function toggleCreate() {
  showCreate.value = !showCreate.value
}

const showEditSection = async () => {
  const result = await dialogs.editExpressionSection({ id: props.sectionInfo.id, isHeaderSection: props.isHeaderSection })

  if (result?.action == 'refreshList') {
    emit('refreshList')
  }
}

const canEdit = computed(() => {
  if (expressionInfo.expressionTypeId == 1 && can.Expression.Edit)
    return true

  return (expressionInfo.expressionTypeId == 13 || expressionInfo.expressionTypeId == 14) && can.ContentManagementSystem.Edit
})

const canCreate = computed(() => {
  if (expressionInfo.expressionTypeId == 1 && can.Expression.Create)
    return true

  return (expressionInfo.expressionTypeId == 13 || expressionInfo.expressionTypeId == 14) && can.ContentManagementSystem.Create
})

</script>

<template>
  <div v-if="showSkeleton">
    <Skeleton id="expression-section-title-skeleton" class="mb-2" height="1.5em" />
    <Skeleton id="expression-section-body-skeleton" class="mb-2" height="5em" />
  </div>
  <div v-else>
    <div class="flex">
      <div class="col-flex flex-grow-1">
        <component
          :is="`h${Math.min(Math.max(currentLevel, 1), 6)}`"
          :id="makeIdSafe(sectionInfo.name)"
        >
          {{ sectionInfo.name }}
        </component>
      </div>
      <div v-if="props.sectionInfo.sectionTypeName != 'Knowledges Section' && props.sectionInfo.sectionTypeName != 'Blessings Section'" class="col-flex">
        <Button v-if="canCreate && !isHeaderSection && !isReadOnly" label="Add Child Section" class="m-2" @click="toggleCreate" />
        <Button v-if="canEdit && !isReadOnly" label="Edit" class="float-end m-2" @click="showEditSection()" />
      </div>
    </div>
    <div v-if="props.sectionInfo.sectionTypeName === 'Knowledges Section'">
      <KnowledgeList :is-read-only="!canEdit && isReadOnly" />
    </div>
    <div v-if="props.sectionInfo.sectionTypeName === 'Blessings Section'">
      <BlessingList :is-read-only="!canEdit && isReadOnly" />
    </div>

    <div v-else class="mb-2 fix-wrapping" v-html="props.sectionInfo.content" />
  </div>
  <div v-if="showCreate && canCreate">
    <CreateExpressionSection :parent-id="props.sectionInfo.id" @cancel-event="toggleCreate" @added-section="passThroughAddedSection()" />
  </div>
</template>

<style>

  .fix-wrapping {
    overflow-wrap: break-word;
  }

</style>
