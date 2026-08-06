<script setup lang="ts">

import Skeleton from 'primevue/skeleton'
import { computed, type PropType } from 'vue'
import { ExpressionSubTypes } from '@/components/expressions/expressionSubTypes.ts'

const props = defineProps({
  expressionSubTypeId: {
    type: Number as PropType<ExpressionSubTypes>,
    required: false,
    default: 0,
  },
  isLoading: {
    type: Boolean,
    required: false,
    default: false,
  },
})

const image = computed(() => {
  switch (props.expressionSubTypeId) {
    case ExpressionSubTypes.Adepts: return '/expressions/adepts_square.png'
    case ExpressionSubTypes.Aeternari: return '/expressions/aeternari_square.png'
    case ExpressionSubTypes.Shammas: return '/expressions/shammas_square.png'
    case ExpressionSubTypes.Sidhe: return '/expressions/sidhe_square.png'
    case ExpressionSubTypes.Sorcerers: return '/expressions/sorcerers_square.png'
    case ExpressionSubTypes.Vampyre: return '/expressions/vampyres_square.png'
    default: return ''
  }
})

</script>

<template>
  <Skeleton v-if="props.isLoading" width="150px" height="150px" />
  <img v-else :src="image" class="w-100" style="max-width: 250px" alt="round circle">
</template>
