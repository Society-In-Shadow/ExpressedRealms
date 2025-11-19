<script setup lang="ts">

import { computed } from 'vue'
import Skeleton from 'primevue/skeleton'
import type { FormField } from '@/FormWrappers/Interfaces/FormField'
import Listbox from 'primevue/listbox'

const model = defineModel<FormField>({ required: true, type: Object })

defineOptions({
  inheritAttrs: false,
})

const props = defineProps({
  options: {
    type: Array,
    required: true,
  },
  optionValue: {
    type: String,
    required: true,
  },
  optionDisabled: {
    type: String,
    default: 'disabled',
  },
  dataCyTag: {
    type: String,
    default: '',
  },
  showSkeleton: {
    type: Boolean,
    default: false,
  },
})

const dataCyTagCalc = computed(() => {
  if (props.dataCyTag != '') {
    return props.dataCyTag
  }
  return model.value.label.replace(' ', '-').toLowerCase()
})

</script>

<template>
  <div class="mb-3 w-100">
    <label :for="dataCyTagCalc">{{ model.label }}<span v-if="model.isRequired" class="text-danger font-italic"> (Required)</span></label>
    <Skeleton v-if="showSkeleton" :id="dataCyTagCalc + '-skeleton'" class="w-100" height="3em" />
    <Listbox
      id="dataCyTagCalc" v-model="model.field"
      :data-cy="dataCyTagCalc"
      :options="options" :option-value="optionValue" :option-disabled="props.optionDisabled"
      class="w-100" :class="{ 'p-invalid': model.error && model.error.length > 0 }" v-bind="$attrs"
    >
      <template #option="slotProps">
        <slot name="option" :option="slotProps.option" />
      </template>
    </Listbox>
    <small :data-cy="dataCyTagCalc + '-help'" class="text-danger">{{ model.error }}</small>
    <slot />
  </div>
</template>
