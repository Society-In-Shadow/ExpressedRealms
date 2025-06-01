<script setup lang="ts">

import Card from "primevue/card";
import Button from "primevue/button";
import {type PropType, ref} from "vue";
import type {Power} from "@/components/expressions/powers/types/power";
import EditPower from "@/components/expressions/powers/EditPower.vue";
import {powerConfirmationPopups} from "@/components/expressions/powers/services/powerConfirmationPopupService";

const props = defineProps({
  power: {
    type: Object as PropType<Power>,
    required: true,
  },
});

const popups = powerConfirmationPopups(props.power.id, props.power.name);

const showEdit = ref(false);

const toggleEdit = () =>{
  showEdit.value = !showEdit.value;
}

</script>

<template>
  <EditPower v-if="showEdit" :power-id="props.power.id" @canceled="toggleEdit" />
  <Card v-else>
    <template #title>
      <div class="d-flex align-self-center justify-content-between">
        <div>
          <h1 class="p-0 m-0">
            {{ props.power.name }}
          </h1>
        </div>
        <div class="p-0 m-0">
          {{ props.power.powerLevel.name }}
        </div>
      </div>
    </template>
    <template #subtitle>
      <div v-html="props.power.description" />
    </template>
    <template #content>
      <table class="w-100 p-datatable-table">
        <!-- Table header -->
        <thead class="p-datatable-thead">
          <tr>
            <th class="p-datatable-header-cell">
              Category
            </th>
            <th class="p-datatable-header-cell">
              Activation Type
            </th>
            <th class="p-datatable-header-cell">
              Area of Effect
            </th>
            <th class="p-datatable-header-cell">
              Power Duration
            </th>
            <th class="p-datatable-header-cell">
              Power Used?
            </th>
          </tr>
        </thead>
        <tbody class="p-datatable-tbody">
          <tr class="p-row-even">
            <td>
              <p v-for="category in props.power.category" class="pr-3">
                {{ category.name }}
              </p>
            </td>
            <td :title="props.power.powerActivationType.description">
              {{ props.power.powerActivationType.name }}
            </td>
            <td :title="props.power.areaOfEffect.description">
              {{ props.power.areaOfEffect.name }}
            </td>
            <td :title="props.power.powerDuration.description">
              {{ props.power.powerDuration.name }}
            </td>
            <td>{{ props.power.isPowerUse ? "Yes" : "No" }}</td>
          </tr>
        </tbody>
      </table>

      <h2>Game Mechanic Effect</h2>
      <div v-html="props.power.gameMechanicEffect" />

      <h2>Limitations</h2>
      <div v-html="props.power.limitation" />

      <h2>Additional Information</h2>
      <div v-html="props.power.other" />
    </template>
    <template v-if="!showEdit" #footer>
      <Button class="mr-2" severity="danger" label="Delete" @click="popups.deleteConfirmation($event)" />
      <Button class="float-end" label="Edit" @click="toggleEdit" />
    </template>
  </Card>
</template>
