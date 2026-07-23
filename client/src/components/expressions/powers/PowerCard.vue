<script setup lang="ts">

import Card from 'primevue/card'
import Button from 'primevue/button'
import { computed, type PropType } from 'vue'
import { type Power, TargetPowerType } from '@/components/expressions/powers/types'
import { powerConfirmationPopups } from '@/components/expressions/powers/services/powerConfirmationPopupService'
import { isNullOrWhiteSpace, makeIdSafe } from '@/utilities/stringUtilities'
import { scrollToSection } from '@/components/expressions/expressionUtilities'
import { can } from '@/stores/userPermissionStore.ts'
import { powerDialogs } from '@/components/expressions/powers/services/dialogs.ts'
import { powersStore } from '@/components/expressions/powers/stores/powersStore.ts'

const props = defineProps({
  targetType: {
    type: Number as PropType<TargetPowerType>,
    required: true,
  },
  power: {
    type: Object as PropType<Power>,
    required: true,
  },
  powerPathId: {
    type: Number,
    required: false,
    default: 0,
  },
  isReadOnly: {
    type: Boolean,
    required: false,
  },
  startingHeader: {
    type: Number,
    default: 1,
  },
})

const emits = defineEmits<{
  modified: []
}>()

const popups = powerConfirmationPopups(props.power.id, props.power.name, props.powerPathId)
const dialogs = powerDialogs()
const powerInfo = powersStore()

const showEditPopup = async () => {
  const result = await dialogs.showEditPower({ powerId: props.power.id, target: props.targetType, powerPathId: props.powerPathId })

  if (result?.action == 'modified') {
    emits('modified')
    await powerInfo.updatePowersByPathId(props.powerPathId)
  }
}

const canEdit = computed(() => {
  switch (props.targetType) {
    case TargetPowerType.PowerPath:
      return can.Powers.Edit
    case TargetPowerType.FactionLevel:
      return can.Faction.Edit
    default:
      return false
  }
})

const canDelete = computed(() => {
  switch (props.targetType) {
    case TargetPowerType.PowerPath:
      return can.Powers.Edit
    case TargetPowerType.FactionLevel:
      return false
    default:
      return false
  }
})

const parentHeader = computed(() => {
  return `h${Math.min(props.startingHeader, 6)}`
})

const childHeader = computed(() => {
  return `h${Math.min(props.startingHeader + 1, 6)}`
})

const childHeaderFix = computed(() => {
  return (props.startingHeader * 0.3)
})

</script>

<template>
  <Card :id="makeIdSafe(props.power.name)" class="card-body-fix">
    <template #content>
      <div>
        <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
          <div>
            <component :is="parentHeader" class="p-0 m-0">
              {{ props.power.name }}
            </component>
            <component :is="childHeader" class="p-0 m-0">
              {{ props.power.powerLevel.name }}
            </component>
          </div>
          <div
            v-if="(canEdit || canDelete) && !props.isReadOnly"
            class="p-0 m-0 d-inline-flex align-items-start"
          >
            <Button v-if="canDelete" class="mr-2" severity="danger" label="Delete" @click="popups.deleteConfirmation($event)" />
            <Button v-if="canEdit" class="float-end" label="Edit" @click="showEditPopup" />
          </div>
        </div>
      </div>

      <div class="p-card-subtitle" v-html="props.power.description" />
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

      <component :is="childHeader">
        Game Mechanic Effect
      </component>
      <div v-html="props.power.gameMechanicEffect" />

      <component :is="childHeader" v-if="!isNullOrWhiteSpace(props.power.limitation)">
        Limitations
      </component>
      <div v-if="!isNullOrWhiteSpace(props.power.limitation)" v-html="props.power.limitation" />

      <component :is="childHeader" v-if="props.power.prerequisites">
        Prerequisites
      </component>
      <div v-if="props.power.prerequisites">
        <div v-if="props.power.prerequisites.powers.length == 1">
          <a :href="'#' + makeIdSafe(props.power.prerequisites.powers[0])" @click.prevent="scrollToSection(props.power.prerequisites.powers[0])">{{ props.power.prerequisites.powers[0] }}</a>
        </div>
        <div v-else-if="props.power.prerequisites.powers.length == props.power.prerequisites.requiredAmount">
          All of the following powers :
          <span v-for="(power, index) in props.power.prerequisites.powers" :key="makeIdSafe(power)">
            <a :href="'#' + makeIdSafe(power)" @click.prevent="scrollToSection(power)">{{ power }}</a>
            <span v-if="index != props.power.prerequisites.powers.length -1"> and </span>
          </span>
        </div>
        <div v-else>
          Any of
          <span v-if="props.power.prerequisites.requiredAmount != 1">{{ props.power.prerequisites.requiredAmount }}</span>
          the following powers :
          <span v-for="(power, index) in props.power.prerequisites.powers" :key="makeIdSafe(power)">
            <a :href="'#' + makeIdSafe(power)" @click.prevent="scrollToSection(power)">{{ power }}</a>
            <span v-if="index != props.power.prerequisites.powers.length -1"> or </span>
          </span>
        </div>
      </div>

      <component
        :is="childHeader"
        v-if="!isNullOrWhiteSpace(props.power.other)"
      >
        Additional Information
      </component>
      <div v-if="!isNullOrWhiteSpace(props.power.other)" v-html="props.power.other" />
    </template>
  </Card>
</template>

<style>
  .card-body-fix .p-card-body{
    padding-left: 0 !important;
    padding-right: 0 !important;
  }
</style>
