<script setup lang="ts">

import {nextTick, onMounted} from "vue";
import {publicExpressionsStore} from "@/components/public/stores/publicExpressionStore";
import {makeIdSafe} from "@/utilities/stringUtilities";

const store = publicExpressionsStore();

onMounted(async () => {
  await store.getExpressions();
  if(location.hash){
    await nextTick();
    window.location.replace(location.hash);
  }
})
</script>

<template>
  <h1>What Expression will you awaken to?</h1>
  <div v-for="expression in store.expressions" :key="expression.id" class="d-flex flex-column flex-md-row align-items-center">
    <div>
      <h2 :id="makeIdSafe(expression.name)">
        {{ expression.name }}
      </h2>
      <h4 class="text-sm text-secondary">
        <em>{{ expression.archetypes }}</em>
      </h4>
      <p>
        {{ expression.description }}
      </p>
      <p>For full background and information, please <a href="/login">login</a> and take a look at the expressions page.</p>
    </div>
    <div>
      <img src="/public/favicon.png" alt="If I had one" width="150">
    </div>
  </div>
</template>
