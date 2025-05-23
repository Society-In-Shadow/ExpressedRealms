<script setup lang="ts">

import {onBeforeMount, ref} from "vue";
import AddPower from "@/components/expressions/powers/AddPower.vue";
import {powersStore} from "@/components/expressions/powers/stores/powersStore";

const powers = powersStore();

const props = defineProps({
  expressionId: {
    type: Number
  }
});

onBeforeMount(async () => {
  await powers.getPowers(props.expressionId);
})

</script>

<template>
 <AddPower :expression-id="props.expressionId" />
</template>

<style scoped>

</style>