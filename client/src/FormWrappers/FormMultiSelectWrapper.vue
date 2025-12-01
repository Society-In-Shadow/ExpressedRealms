<script setup lang="ts">

import MultiSelect from 'primevue/multiselect'
import { computed, inject } from 'vue'
import Skeleton from 'primevue/skeleton'
import type { FormField } from '@/FormWrappers/Interfaces/FormField'

const model = defineModel<FormField>({ required: true, type: Object })

defineOptions({
  inheritAttrs: false,
})

const props = defineProps({
  options: {
    type: Array,
    required: true,
  },
  optionLabel: {
    type: String,
    required: true,
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

const showSkeleton = props.showSkeleton ?? inject('showSkeleton', false)
const isInvalid = computed(() => (model.value.error.value ?? '').length > 0)

</script>

<template>
  <div class="mb-3 w-100">
    <label :for="dataCyTagCalc">{{ model.label }}<span v-if="model.isRequired" class="text-danger font-italic"> (Required)</span></label>
    <Skeleton v-if="showSkeleton" :id="dataCyTagCalc + '-skeleton'" class="w-100" height="3em" />
    <MultiSelect
      v-else :id="dataCyTagCalc" v-model="model.field" :options="options" :option-label="optionLabel"
      :data-cy="dataCyTagCalc"
      class="w-100" :invalid="isInvalid" v-bind="$attrs"
    />
    <small v-if="isInvalid" :data-cy="dataCyTagCalc + '-help'" class="text-danger">{{ model.error }}</small>
    <slot />
  </div>
</template>
