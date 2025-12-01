<script setup lang="ts">

import Select from 'primevue/select'
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
    default: undefined,
  },
  labelOverride: {
    type: String,
    default: '',
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
    <label :for="dataCyTagCalc">{{ props.labelOverride == "" ? model.label : props.labelOverride }}<span v-if="model.isRequired" class="text-danger font-italic"> (Required)</span></label>
    <Skeleton v-if="showSkeleton" :id="dataCyTagCalc + '-skeleton'" class="w-100" height="3em" />
    <Select
      v-else :id="dataCyTagCalc" v-model="model.field.value" :options="options" :option-label="optionLabel"
      :data-cy="dataCyTagCalc" :invalid="isInvalid"
      class="w-100" v-bind="$attrs"
    />
    <small v-if="isInvalid" :data-cy="dataCyTagCalc + '-help'" class="text-danger">{{ model.error }}</small>
    <slot />
  </div>
</template>
