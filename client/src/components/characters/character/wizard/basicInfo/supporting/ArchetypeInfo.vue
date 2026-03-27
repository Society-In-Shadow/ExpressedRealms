<script setup lang="ts">

import Button from 'primevue/button'
import { ref, watch } from 'vue'
import axios from 'axios'
import type { ArchetypesResponse } from '@/components/characters/character/wizard/basicInfo/types.ts'
import { useRouter } from 'vue-router'
import { breakpointsBootstrapV5, useBreakpoints } from '@vueuse/core'

const router = useRouter()

const activeBreakpoint = useBreakpoints(breakpointsBootstrapV5)
const isMobile = activeBreakpoint.smaller('md')

const emit = defineEmits<{
  selectedArchetype: [characterId: number]
}>()

const props = defineProps({
  expressionId: {
    type: Number,
    required: true,
  },
})

const archetypeInfo = ref<ArchetypesResponse>({})

async function loadInfo() {
  const response = await axios.get<ArchetypesResponse>(`/characters/archetypes/${props.expressionId}`)
  archetypeInfo.value = response.data
}

watch(() => props.expressionId, async (newValue) => {
  await loadInfo()
}, { immediate: true, deep: true })

function redirectToCharacter(characterId: number) {
  const routeData = router.resolve({ name: 'characterSheet', params: { id: characterId } })
  window.open(routeData.href, '_blank')
}

</script>

<template>
  <div>
    <div v-for="archetype in archetypeInfo.archetypes" :key="archetype.id">
      <div v-if="!isMobile">
        <div class="d-flex flex-row align-items-center">
          <h3 class="flex-fill">
            {{ archetype.name }}
          </h3>
          <div class="align-content-between align-content-md-end">
            <Button
              label="Show Info" size="small" class="mr-2" severity="info" icon="pi pi-external-link"
              icon-pos="right" @click="redirectToCharacter(archetype.id)"
            />
            <Button label="Use This Build" size="small" @click="emit('selectedArchetype', archetype.id)" />
          </div>
        </div>

        <p>{{ archetype.background ?? 'No background provided for this archetype' }}</p>
      </div>

      <div v-else>
        <div class="d-flex flex-row align-items-center">
          <h3 class="flex-fill">
            {{ archetype.name }}
          </h3>
        </div>

        <p>{{ archetype.background ?? 'No background provided for this archetype' }}</p>

        <div class="text-right">
          <Button
            label="Show Info" size="small" class="mr-2" severity="info" icon="pi pi-external-link"
            icon-pos="right" @click="redirectToCharacter(archetype.id)"
          />
          <Button label="Use This Build" size="small" @click="emit('selectedArchetype', archetype.id)" />
        </div>
      </div>
    </div>
  </div>
</template>
