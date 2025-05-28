<script setup lang="ts">

import Card from "primevue/card";
import type {PropType} from "vue";
import type {Power} from "@/components/expressions/powers/types/power";

const props = defineProps({
  power: {
    type: Object as PropType<Power>,
    required: true,
  },
});

</script>

<template>
  <Card>
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
  </Card>
</template>
