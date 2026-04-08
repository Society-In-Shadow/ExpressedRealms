<script setup lang="ts">

import Card from 'primevue/card'
import { computed, onBeforeMount, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { characterStore } from '@/components/characters/character/stores/characterStore'
import Button from 'primevue/button'
import { experienceStore } from '@/components/characters/character/stores/experienceBreakdownStore.ts'
import { FeatureFlags, userStore } from '@/stores/userStore.ts'
import Tag from 'primevue/tag'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'
import { downloadFile } from '@/utilities/downloadUtility.ts'

const userPermissionInfo = userPermissionStore()
const permissionCheck = userPermissionInfo.permissionCheck
const route = useRoute()
const router = useRouter()
const characterInfo = characterStore()
const experienceInfo = experienceStore()
const userInfo = userStore()

const characterId = Number(route.params.id)
const showFactionInfo = ref(false)
const isOwner = ref(false)

onBeforeMount(async () => {
  await characterInfo.getCharacterDetails(characterId)
    .then(() => {
      name.value = characterInfo.name
      expression.value = characterInfo.expression
      faction.value = characterInfo.faction
      isPrimaryCharacter.value = characterInfo.isPrimaryCharacter
      isOwner.value = characterInfo.isOwner
      isInCharacterGeneration.value = characterInfo.isInCharacterCreation
      isRetired.value = characterInfo.isRetired
    })
  showFactionInfo.value = await userInfo.hasFeatureFlag(FeatureFlags.ShowFactionDropdown)
  await experienceInfo.updateExperience(characterId)
})

const name = ref('')
const faction = ref('')
const expression = ref('')
const isPrimaryCharacter = ref(false)
const isInCharacterGeneration = ref(false)
const isRetired = ref(false)

const redirectToEdit = () => {
  router.push({ name: 'characterWizard', params: { id: characterId } })
}

async function downloadCharacterBooklet() {
  await downloadFile(`/characters/${Number(characterId)}/getcrb`, `${name.value} - ${userInfo.name} - CRB.pdf`)
}

const canDownloadCrb = computed(() => permissionCheck.CharacterManagement.DownloadAllCrbs
  || (isPrimaryCharacter.value && permissionCheck.CharacterManagement.ViewCharacterSheet))
</script>

<template>
  <Card class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch">
    <template #content>
      <div class="d-flex flex-column flex-md-row justify-content-between">
        <div>
          <div class="d-flex flex-column flex-md-row gap-3">
            <div>
              <h1 class="mt-0 pt-0 pb-0 mb-0">
                {{ name }}
              </h1>
            </div>
            <div class="d-flex flex-row align-content-between gap-3">
              <div class="d-md-none d-block flex-fill">
                <div v-if="!experienceInfo.isLoading && !isInCharacterGeneration">
                  <em><span>XL: {{ experienceInfo.characterLevel }}</span> - {{ expression }}</em>
                </div>
              </div>
              <div class="align-content-center">
                <div><Tag v-if="isPrimaryCharacter" value="Primary" severity="info" /></div>
                <div><Tag v-if="isRetired" value="Retired" severity="warn" /></div>
              </div>
            </div>
          </div>
          <div v-if="!experienceInfo.isLoading && !isInCharacterGeneration" class="d-md-block d-none">
            <em><span>XL: {{ experienceInfo.characterLevel }}</span> - {{ expression }}</em>
          </div>
          <div v-if="!experienceInfo.isLoading && isInCharacterGeneration">
            <em>In Character Creation - {{ expression }}</em>
          </div>
          <div v-if="showFactionInfo">
            <em>{{ faction?.name ?? 'No Faction' }}</em>
          </div>
        </div>
        <div class="mt-3 text-right">
          <Button v-if="canDownloadCrb" label="Download CRB" class="mr-3" @click="downloadCharacterBooklet()" />
          <Button v-if="isOwner && !isRetired || permissionCheck.Archetypes.Edit" label="Edit" @click="redirectToEdit" />
        </div>
      </div>
    </template>
  </Card>
</template>
