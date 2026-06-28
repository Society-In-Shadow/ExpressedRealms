<script setup lang="ts">

import Card from 'primevue/card'
import { computed, onBeforeMount } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { characterStore } from '@/components/characters/character/stores/characterStore'
import Button from 'primevue/button'
import { experienceStore } from '@/components/characters/character/stores/experienceBreakdownStore.ts'
import Tag from 'primevue/tag'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'
import { downloadFile } from '@/utilities/downloadUtility.ts'
import { hasFlag } from '@/stores/featureFlags/featureFlagStore.ts'

const userPermissionInfo = userPermissionStore()
const permissionCheck = userPermissionInfo.permissionCheck
const route = useRoute()
const router = useRouter()
const characterInfo = characterStore()
const experienceInfo = experienceStore()

const characterId = Number(route.params.id)

onBeforeMount(async () => {
  await characterInfo.getCharacterDetails(characterId)
  await experienceInfo.getExperience(characterId)
})

const redirectToEdit = () => {
  router.push({ name: 'characterWizard', params: { id: characterId } })
}

async function downloadCharacterBooklet() {
  await downloadFile(`/characters/${Number(characterId)}/getcrb`, `${characterInfo.name} - CRB.pdf`)
}

const canDownloadCrb = computed(() => permissionCheck.CharacterManagement.DownloadAllCrbs
  || (characterInfo.isPrimaryCharacter && permissionCheck.CharacterManagement.ViewCharacterSheet))

const canEditCharacterSheet = computed(() => (characterInfo.isOwner && !characterInfo.isRetired) || (characterInfo.isArchetypeCharacter && permissionCheck.Archetypes.Edit))
</script>

<template>
  <Card class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch">
    <template #content>
      <div class="d-flex flex-column flex-md-row justify-content-between">
        <div>
          <div class="d-flex flex-column flex-md-row gap-3">
            <div>
              <h1 class="mt-0 pt-0 pb-0 mb-0">
                {{ characterInfo.name }}
              </h1>
            </div>
            <div class="d-flex flex-row align-content-between gap-3">
              <div class="d-md-none d-block flex-fill">
                <div v-if="!experienceInfo.isLoading && !characterInfo.isInCharacterCreation">
                  <em><span>XL: {{ experienceInfo.characterLevel }}</span> - {{ characterInfo.expression }}</em>
                </div>
              </div>
              <div class="align-content-center">
                <div><Tag v-if="characterInfo.isPrimaryCharacter" value="Primary" severity="info" /></div>
                <div><Tag v-if="characterInfo.isRetired" value="Retired" severity="warn" /></div>
              </div>
            </div>
          </div>
          <div v-if="!experienceInfo.isLoading && !characterInfo.isInCharacterCreation" class="d-md-block d-none">
            <em><span>XL: {{ experienceInfo.characterLevel }}</span> - {{ characterInfo.expression }}</em>
          </div>
          <div v-if="!experienceInfo.isLoading && characterInfo.isInCharacterCreation">
            <em>In Character Creation - {{ characterInfo.expression }}</em>
          </div>
          <div v-if="hasFlag.ShowFactionDropdown">
            <em>{{ characterInfo.faction?.name ?? 'No Faction' }}</em>
          </div>
        </div>
        <div class="mt-3 text-right">
          <Button v-if="canDownloadCrb" label="Download CRB" class="mr-3" @click="downloadCharacterBooklet()" />
          <Button v-if="canEditCharacterSheet" label="Edit" @click="redirectToEdit" />
        </div>
      </div>
    </template>
  </Card>
</template>
