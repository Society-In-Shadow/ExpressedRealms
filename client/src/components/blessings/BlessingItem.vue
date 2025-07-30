<script setup lang="ts">

import {type PropType} from "vue";
import type {Blessing} from "@/components/blessings/types";

const props = defineProps({
  blessing: {
    type: Object as PropType<Blessing>,
    required: true,
  },
  isReadOnly: {
    type: Boolean,
    required: true
  }
});
</script>

<template>
  <div class="d-flex flex-column flex-md-row align-self-center justify-content-between mt-4">
    <div>
      <h1 class="p-0 m-0">
        {{ props.blessing.name }}
      </h1>
      <div class="p-0 m-0">
        {{ props.blessing?.subCategory }}
      </div>
    </div>
  </div>
  <p>{{ props.blessing.description }}</p>
  <ul>
    <li v-for="level in props.blessing.levels" :key="level.id" class="mt-3">
      <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
        <div>{{ level.name }} – {{ level.description }}</div>
      </div>
    </li>
  </ul>
</template>
