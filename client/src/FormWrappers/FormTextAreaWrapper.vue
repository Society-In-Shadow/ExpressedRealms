<script setup lang="ts">

import Textarea from 'primevue/textarea'
import { computed, inject } from 'vue'
import Skeleton from 'primevue/skeleton'
import type { FormField } from '@/FormWrappers/Interfaces/FormField'

const model = defineModel<FormField>({ required: true })

defineOptions({
  inheritAttrs: false,
})

const props = defineProps({
  dataCyTag: {
    type: String,
    default: '',
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

</script>

<template>
  <div class="mb-3 w-100">
    <label :for="dataCyTagCalc">{{ model.label }}<span v-if="model.isRequired" class="text-danger font-italic"> (Required)</span></label>
    <Skeleton v-if="showSkeleton" :id="dataCyTagCalc + '-skeleton'" class="w-100" height="5em" />
    <Textarea
      v-else
      :id="dataCyTagCalc" v-model="model.field.value" :data-cy="dataCyTagCalc" class="w-100"
      :invalid="isInvalid" v-bind="$attrs" auto-resize :disabled="isDisabled"
    />
    <small v-if="isInvalid" :data-cy="dataCyTagCalc + '-help'" class="text-danger">{{ model.error }}</small>
    <slot />
  </div>
</template>
