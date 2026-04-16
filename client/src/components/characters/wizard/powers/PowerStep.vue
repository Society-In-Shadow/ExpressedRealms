<script setup lang="ts">

import { computed, onBeforeMount, ref } from 'vue'
import { useRoute } from 'vue-router'
import { characterPowersStore } from '@/components/characters/character/powers/stores/characterPowerStore.ts'
import PowerCard from '@/components/characters/wizard/powers/supporting/PowerCard.vue'
import PickPowerCard from '@/components/characters/wizard/powers/supporting/PickPowerCard.vue'
import ShowXPCosts from '@/components/characters/wizard/ShowXPCosts.vue'
import { XpSectionTypes } from '@/components/characters/character/stores/experienceBreakdownStore.ts'
import { characterStore } from '@/components/characters/character/stores/characterStore.ts'
import Message from 'primevue/message'

const characterKnowledgeData = characterPowersStore()
const characterData = characterStore()
const route = useRoute()

onBeforeMount(async () => {
  await characterKnowledgeData.getSelectableCharacterPowers(route.params.id)
  await characterKnowledgeData.getCharacterPowers(route.params.id)
  if (characterKnowledgeData.powers.length === 0) {
    noPowers.value = true
  }
})

const noPowers = ref(false)

const sorcererNeedsPaths = computed(() => characterData.expression === 'Sorcerers'
  && (characterData.primaryProgressionId == null || characterData.secondaryProgressionId == null),
)

</script>

<template>
  <div style="max-width: 650px; margin: 0 auto;">
    <Message v-if="sorcererNeedsPaths" severity="warn">
      You need to choose a primary and secondary elemental path on the basic info page in order to see all powers
      you can choose for your sorcerer
    </Message>
    <h1>Selected Powers</h1>
    <div v-for="path in characterKnowledgeData.powers">
      <h2>{{ path.name }}</h2>
      <PowerCard :power-path="path" :show-edit="true" />
    </div>

    <div class="mb-2">
      <hr v-if="characterKnowledgeData.powers.length !== 0">
      <h1 class="pb-0 mb-0">
        Choose Powers
      </h1>
      <ShowXPCosts :section-type="XpSectionTypes.powers" />
      <div v-if="characterKnowledgeData.powers.length === 0">
        <p>No Powers detected, please pick one below.</p>
      </div>
      <div v-for="path in characterKnowledgeData.selectablePowers">
        <h2>{{ path.name }}</h2>
        <div v-for="power in path.powers" :key="power.id" class="pb-2">
          <PickPowerCard :power="power" :show-pick-button="true" />
        </div>
      </div>
    </div>
  </div>
</template>
