<script setup lang="ts">

import {onMounted, ref} from 'vue'
import {useRoute} from 'vue-router'
import Button from 'primevue/button'
import Popover from 'primevue/popover'
import {proficiencyStore} from '@/components/characters/character/proficiency/stores/proficiencyStore'

const route = useRoute()
const profStore = proficiencyStore()

onMounted(() => {
  profStore.getUpdateProficiencies(route.params.id)
})

const op = ref()
const toggle = (event) => {
  op.value.toggle(event)
}

</script>

<template>
  <Button type="button" label="Sec. Prof." class="d-block d-md-none" @click="toggle" />

  <div class="d-md-flex flex-row justify-content-end flex-wrap d-none">
    <div v-for="proficiency in profStore.secondary" class="statTile pt-2 pb-2 pr-3 pl-3 mr-2 ml-2 mt-3">
      <span class="mr-3">{{ proficiency.name }}</span>
      <span>{{ proficiency.maxValue }}</span>
    </div>
  </div>

  <Popover ref="op" class="d-md-none">
    <div class="d-flex flex-row justify-content-end flex-wrap">
      <div v-for="proficiency in profStore.secondary" class="statTile pt-2 pb-2 pr-3 pl-3 mr-2 ml-2 mt-3">
        <span class="mr-3">{{ proficiency.name }}</span>
        <span>{{ proficiency.maxValue }}</span>
      </div>
    </div>
  </Popover>
</template>

<style scoped>
  .statTile {
    border-radius: 4px; /* Rounding of corners */
    border: 1px solid var(--p-form-field-disabled-background);
    background-color: var(--p-card-background);
  }

</style>
