<script setup lang="ts">
import {onBeforeMount, ref} from "vue";
import AddPower from "@/components/expressions/powers/AddPower.vue";
import {powersStore} from "@/components/expressions/powers/stores/powersStore";
import PowerCard from "@/components/expressions/powers/PowerCard.vue";
import Button from 'primevue/button';

const props = defineProps({
  powerPathId: {
    type: Number,
    required: true,
  }
});

const powers = powersStore( props.powerPathId);

const showAddPower = ref(false);

onBeforeMount(async () => {
  await powers.getPowers(props.powerPathId);
})

const toggleAddPower = () => {
  showAddPower.value = !showAddPower.value;
}
</script>

<template>
  <div v-for="power in powers.powers">
    <PowerCard :power="power" :power-path-id="props.powerPathId" />
  </div>
  
  <AddPower v-if="showAddPower" :power-path-id="props.powerPathId" @canceled="toggleAddPower" />
  <Button v-else label="Add Power" @click="toggleAddPower" />
</template>

<style scoped>

</style>
