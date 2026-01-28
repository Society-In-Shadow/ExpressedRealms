<script setup lang="ts">

import { computed, inject } from 'vue'
import Skeleton from 'primevue/skeleton'
import type { FormField } from '@/FormWrappers/Interfaces/FormField'
import RadioButton from 'primevue/radiobutton'

const model = defineModel<FormField>({ required: true })

defineOptions({
  inheritAttrs: false,
})

const emit = defineEmits<{
  selectedItem: {}
}>()

const props = defineProps({
  dataCyTag: {
    type: String,
    default: '',
  },
  rowData: {
    type: Array,
    required: true,
  },
  rowKey: {
    type: String,
    required: true,
  },
  showSkeleton: {
    type: Boolean,
    default: undefined,
  },
})

const dataCyTagCalc = computed(() => {
  if (props.dataCyTag != '') {
    return props.dataCyTag
  }
  return model.value.label.replace(' ', '-').toLowerCase()
})

const showSkeleton = props.showSkeleton ?? inject('showSkeleton', false)
const isDisabled = inject('isDisabled', false)
const isInvalid = computed(() => (model.value.error.value ?? '').length > 0)

function selectFrequency(freq) {
  if (freq.isDisabled) return

  model.value.field.value = freq
  emit('selected-item', freq)
}

</script>

<template>
  <div class="mb-3 w-100">
    <label :for="dataCyTagCalc">{{ model.label }}<span v-if="model.isRequired" class="text-danger font-italic"> (Required)</span></label>
    <Skeleton v-if="showSkeleton" :id="dataCyTagCalc + '-skeleton'" class="w-100" height="5em" />

    <div v-else class="frequency-levels mt-3" role="radiogroup">
      <div class="frequency-header">
        <span />
        <slot name="header" />
      </div>

      <!-- Rows -->
      <div
        v-for="data in props.rowData"
        :key="data[props.rowKey]"
        class="frequency-row"
        :class="{
          selected: data[props.rowKey] === model.field.value[props.rowKey],
          disabled: data.isDisabled || isDisabled
        }"
        @click="selectFrequency(data)"
      >
        <RadioButton
          :model-value="model.field.value[props.rowKey]"
          :value="data[props.rowKey]"
          :disabled="data.isDisabled"
          @update:model-value="() => selectFrequency(data)"
        />

        <slot name="row" :data="data" />
      </div>
    </div>
    <small v-if="isInvalid" :data-cy="dataCyTagCalc + '-help'" class="text-danger">{{ model.error }}</small>
    <slot />
  </div>
</template>

<style scoped>
.frequency-levels {
  border: 1px solid var(--surface-300);
  overflow: hidden;
}

.frequency-header,
.frequency-row {
  display: grid;
  grid-template-columns: 3rem 1fr auto;
  align-items: center;
  padding: 0.5rem 0.75rem;
}

.frequency-header {
  color: var(--p-datatable-header-cell-color);
  background-color: var(--p-datatable-header-cell-background);
  font-weight: 600;
  border-bottom: 1px solid var(--surface-300);
}

.frequency-row {
  border-bottom: 1px solid var(--surface-200);
  cursor: pointer;
}

.frequency-row:last-child {
  border-bottom: none;
}

.frequency-row:hover:not(.disabled) {
  background: var(--surface-100);
}

.frequency-row.selected {
  background: var(--primary-50);
}

.frequency-row.disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

</style>
