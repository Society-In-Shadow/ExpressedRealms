<script setup lang="ts">

import Button from 'primevue/button'
import {type PropType} from 'vue'
import {isNullOrWhiteSpace} from '@/utilities/stringUtilities'
import Tag from 'primevue/tag'
import type {PowerPath} from '@/components/characters/character/powers/types.ts'
import EditCharacterPower from '@/components/characters/character/wizard/powers/supporting/EditCharacterPower.vue'
import type {WizardContent} from '@/components/characters/character/wizard/types.ts'
import {wizardContentStore} from '@/components/characters/character/wizard/stores/wizardContentStore.ts'

const wizardContentInfo = wizardContentStore()

const props = defineProps({
  powerPath: {
    type: Object as PropType<PowerPath>,
    required: true,
  },
  showEdit: {
    type: Boolean,
    required: false,
  },
})

const updateWizardContent = (power) => {
  wizardContentInfo.updateContent(
    {
      headerName: 'Power',
      component: EditCharacterPower,
      props: { power: power },
    } as WizardContent,
  )
}

</script>

<template>
  <div>
    <div v-for="power in props.powerPath.powers" :key="power.id" class="pb-3">
      <div class="d-flex flex-grow-1 pr-2">
        <div class="d-flex flex-column flex-grow-1 pr-3">
          <div class="d-flex flex-fill align-content-between d-block">
            <div class="flex-grow-1 font-bold text-900">
              {{ power.name }} - <em>{{ power.powerLevel.name }}</em>
            </div>
            <div>
              {{ power.areaOfEffect.name }} ({{ power.powerActivationType.name }})
            </div>
          </div>
          <div class="d-flex d-block mt-1">
            <div class="flex-grow-1">
              <Tag v-if="power.category.length == 0" severity="info" value="No Specializations" />
              <Tag v-for="special in power.category" v-else severity="info" class="mr-1" :value="special.name" />
            </div>
            <div>{{ power.powerDuration.name }}</div>
          </div>
          <div class="d-flex d-block mt-1">
            <div class="flex-grow-1">
              Is Power Use: {{ power.isPowerUse ? "Yes" : "No" }}
            </div>
            <div>
              <span v-if="!isNullOrWhiteSpace(power.cost)">{{ power.cost }}</span>
              <span v-else>N/A</span>
            </div>
          </div>
        </div>
        <div class="align-content-center">
          <Button label="View" size="small" @click="updateWizardContent(power)" />
        </div>
      </div>
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
