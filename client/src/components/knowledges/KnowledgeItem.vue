<script setup lang="ts">

import type {PropType} from "vue";
import type {Knowledge} from "@/components/knowledges/types";

const props = defineProps({
  knowledge: {
    type: Object as PropType<Knowledge>,
    required: true,
  }
});
</script>

<template>
  <h1>{{props.knowledge.name}}</h1>
  <h3>{{props.knowledge.typeName}}</h3>
  <p>{{props.knowledge.description}}</p>
</template>
