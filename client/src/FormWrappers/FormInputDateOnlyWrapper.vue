<script setup lang="ts">

import DatePicker from 'primevue/datepicker'
import { computed } from 'vue'
import Skeleton from 'primevue/skeleton'
import type { FormField } from '@/FormWrappers/Interfaces/FormField'
import { DateTime } from 'luxon'

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
    default: false,
  },
})

const dataCyTagCalc = computed(() => {
  if (props.dataCyTag != '') {
    return props.dataCyTag
  }
  return model.value.label.replace(' ', '-').toLowerCase()
})

function onDateChange(jsDate: Date) {
  // Convert the JS Date (which is in local tz) to UTC, keeping the *clock time* the same
  model.value.field.value = DateTime.fromObject({
    year: jsDate.getFullYear(),
    month: jsDate.getMonth() + 1,
    day: jsDate.getDate(),
    hour: jsDate.getHours(),
    minute: jsDate.getMinutes(),
  }, { zone: 'UTC' })
}

function luxonToDate(dt: DateTime | null): Date | null {
  if (!dt) return null
  return new Date(dt.year, dt.month - 1, dt.day, dt.hour, dt.minute)
}

</script>

<template>
  <div class="mb-3">
    <label :for="dataCyTagCalc">{{ model.label }}<span v-if="model.isRequired" class="text-danger font-italic"> (Required)</span></label>
    <Skeleton v-if="showSkeleton" :id="dataCyTagCalc + '-skeleton'" class="w-100" height="3em" />
    <DatePicker
      v-else
      :id="dataCyTagCalc" :data-cy="dataCyTagCalc" class="w-100"
      date-format="DD MM, d, yy"
      :model-value="luxonToDate(model.field.value)" v-bind="$attrs"
      :class="{ 'p-invalid': model.error && model.error.length > 0 }" @update:model-value="onDateChange"
    />
    <small :data-cy="dataCyTagCalc + '-help'" class="text-danger">{{ model.error }}</small>
    <slot />
  </div>
</template>
