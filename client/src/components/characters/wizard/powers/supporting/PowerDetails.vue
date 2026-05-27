<script setup lang="ts">

import { type PropType } from 'vue'
import type { Power } from '@/components/expressions/powers/types'
import { isNullOrWhiteSpace, makeIdSafe } from '@/utilities/stringUtilities'
import { scrollToSection } from '@/components/expressions/expressionUtilities'

const props = defineProps({
  power: {
    type: Object as PropType<Power>,
    required: true,
  },
  showPickButton: {
    type: Boolean,
    required: false,
  },
})

</script>

<template>
  <div :id="makeIdSafe(props.power.name)" class="card-body-fix">
    <div class="d-flex flex-row align-self-center justify-content-between">
      <div class="flex-grow-1">
        <h2 class="p-0 m-0">
          {{ props.power.name }}
        </h2>
        <div class="p-0 m-0">
          {{ props.power.powerLevel.name }}
        </div>
      </div>
      <div class="p-0 m-2 d-inline-flex align-items-start align-items-center gap-2">
        <slot name="buttons" />
      </div>
    </div>
    <div v-html="props.power.description" />
    <div style="overflow: auto">
      <div class="custom-table-container">
        <table class="w-100 custom-table">
          <tbody>
            <tr>
              <th scope="col">
                <p>Category</p>
              </th>

              <th scope="col">
                <p>Power Duration</p>
              </th>

              <th scope="col">
                <p>Area of Effect</p>
              </th>
            </tr>

            <tr>
              <td>
                <p
                  v-for="category in props.power.category"
                  v-if="props.power.category && props.power.category.length > 0"
                  :key="category.id"
                  class="pr-3"
                >
                  {{ category.name }}
                </p>

                <p v-else>
                  N/A
                </p>
              </td>

              <td :title="props.power.powerDuration.description">
                <p>{{ props.power.powerDuration.name }}</p>
              </td>

              <td :title="props.power.areaOfEffect.description">
                <p>{{ props.power.areaOfEffect.name }}</p>
              </td>
            </tr>
          </tbody>

          <tbody>
            <tr>
              <th scope="col">
                <p>Activation Type</p>
              </th>

              <th scope="col">
                <p>Power Used?</p>
              </th>

              <th scope="col">
                <p>Cost</p>
              </th>
            </tr>

            <tr>
              <td :title="props.power.powerActivationType.description">
                <p>{{ props.power.powerActivationType.name }}</p>
              </td>

              <td>
                <p>{{ props.power.isPowerUse ? "Yes" : "No" }}</p>
              </td>

              <td>
                <p>
                  <span v-if="!isNullOrWhiteSpace(props.power.cost)">
                    {{ props.power.cost }}
                  </span>

                  <span v-else>
                    N/A
                  </span>
                </p>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <h2>Game Mechanic Effect</h2>
      <div v-html="props.power.gameMechanicEffect" />

      <h2 v-if="!isNullOrWhiteSpace(props.power.limitation)">
        Limitations
      </h2>
      <div v-if="!isNullOrWhiteSpace(props.power.limitation)" v-html="props.power.limitation" />

      <h2 v-if="props.power.prerequisites">
        Prerequisites
      </h2>
      <div v-if="props.power.prerequisites">
        <div v-if="props.power.prerequisites.powers.length == 1">
          <a :href="'#' + makeIdSafe(props.power.prerequisites.powers[0])" @click.prevent="scrollToSection(props.power.prerequisites.powers[0])">{{ props.power.prerequisites.powers[0] }}</a>
        </div>
        <div v-else-if="props.power.prerequisites.powers.length == props.power.prerequisites.requiredAmount">
          All of the following powers :
          <span v-for="(power, index) in props.power.prerequisites.powers">
            <a :href="'#' + makeIdSafe(power)" @click.prevent="scrollToSection(power)">{{ power }}</a>
            <span v-if="index != props.power.prerequisites.powers.length -1"> and </span>
          </span>
        </div>
        <div v-else>
          Any of
          <span v-if="props.power.prerequisites.requiredAmount != 1">{{ props.power.prerequisites.requiredAmount }}</span>
          the following powers :
          <span v-for="(power, index) in props.power.prerequisites.powers">
            <a :href="'#' + makeIdSafe(power)" @click.prevent="scrollToSection(power)">{{ power }}</a>
            <span v-if="index != props.power.prerequisites.powers.length -1"> or </span>
          </span>
        </div>
      </div>

      <h2 v-if="!isNullOrWhiteSpace(props.power.other)">
        Additional Information
      </h2>
      <div v-if="!isNullOrWhiteSpace(props.power.other)" v-html="props.power.other" />
    </div>
  </div>
</template>

<style>
@media(max-width: 768px){
  .card-body-fix .p-card-body{
    padding: 0 !important;
  }
}
</style>
