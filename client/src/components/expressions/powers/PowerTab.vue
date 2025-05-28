<script setup lang="ts">
import {onBeforeMount, ref} from "vue";
import AddPower from "@/components/expressions/powers/AddPower.vue";
import {powersStore} from "@/components/expressions/powers/stores/powersStore";
import PowerCard from "@/components/expressions/powers/PowerCard.vue";
import Button from 'primevue/button';

const powers = powersStore();

const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  }
});

const showAddPower = ref(false);

onBeforeMount(async () => {
  await powers.getPowers(props.expressionId);
})

const toggleAddPower = () => {
  showAddPower.value = !showAddPower.value;
}
</script>

<template>
  <div v-for="power in powers.powers">
    <PowerCard :power="power" />
  </div>
  <AddPower v-if="showAddPower" :expression-id="props.expressionId" @canceled="toggleAddPower" />
  <Button v-else label="Add Power" @click="toggleAddPower" />
</template>

<style scoped>

</style>
