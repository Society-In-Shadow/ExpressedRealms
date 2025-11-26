<script setup lang="ts">

import axios from 'axios'
import Card from 'primevue/card'
import { computed, onBeforeMount, ref } from 'vue'
import { useRoute } from 'vue-router'
import toaster from '@/services/Toasters'
import { makeIdSafe } from '@/utilities/stringUtilities'
import { characterStore } from '@/components/characters/character/stores/characterStore'
import { FeatureFlags, userStore } from '@/stores/userStore.ts'
import HighLevelExpressionInfo
  from '@/components/characters/character/wizard/basicInfo/supporting/HighLevelExpressionInfo.vue'
import { wizardContentStore } from '@/components/characters/character/wizard/stores/wizardContentStore.ts'
import type { WizardContent } from '@/components/characters/character/wizard/types.ts'
import Button from 'primevue/button'
import { breakpointsBootstrapV5, useBreakpoints } from '@vueuse/core'
import {
  getValidationInstance,
} from '@/components/characters/character/wizard/basicInfo/validators/characterValidation.ts'
import FormTextAreaWrapper from '@/FormWrappers/FormTextAreaWrapper.vue'
import FormTextWrapper from '@/FormWrappers/FormInputTextWrapper.vue'
import FormCheckboxWrapper from '@/FormWrappers/FormCheckboxWrapper.vue'
import SelectProgressionPaths from '@/components/characters/character/wizard/basicInfo/SelectProgressionPaths.vue'
import Message from 'primevue/message'
import { experienceStore } from '@/components/characters/character/stores/experienceBreakdownStore.ts'

const route = useRoute()

const characterInfo = characterStore()
const userInfo = userStore()
const xpInfo = experienceStore()
const form = getValidationInstance()

const activeBreakpoint = useBreakpoints(breakpointsBootstrapV5)
const isMobile = activeBreakpoint.smaller('md')

const showFactionInfo = ref(false)

onBeforeMount(async () => {
  await characterInfo.getCharacterDetails(Number(route.params.id))
    .then(() => {
      form.fields.name.field.value = characterInfo.name
      form.fields.background.field.value = characterInfo.background
      form.fields.expression.field.value = characterInfo.expression
      form.fields.faction.field.value = characterInfo.faction
      form.fields.isPrimaryCharacter.field.value = characterInfo.isPrimaryCharacter
    })
  showFactionInfo.value = await userInfo.hasFeatureFlag(FeatureFlags.ShowFactionDropdown)
  await characterInfo.getEditOptions(Number(route.params.id))
  if (!isMobile.value) {
    updateWizardContent()
  }
})

const expression = ref('')
const isLoading = ref(true)
const factions = ref([])

const onSubmit = form.handleSubmit((values) => {
  axios.put(`/characters/${route.params.id}`, {
    name: values.name,
    background: values.background,
    factionId: values.faction?.id,
    isPrimaryCharacter: values.isPrimaryCharacter,
    primaryProgressionId: values.primaryProgression?.id,
    secondaryProgressionId: values.secondaryProgression?.id,
  }).then(async () => {
    characterInfo.name = values.name
    characterInfo.background = values.background
    characterInfo.faction = values.faction
    characterInfo.isPrimaryCharacter = values.isPrimaryCharacter
    await xpInfo.updateExperience(route.params.id)
    await characterInfo.getEditOptions(Number(route.params.id))
    toaster.success('Successfully Updated Character Info!')
  })
})

let expressionRedirectURL = computed(() => {
  if (!isLoading.value) {
    return `/expressions/${expression.value.toLowerCase()}#${makeIdSafe(form.field.faction.value.name)}`
  }
  return ''
})

const wizardContentInfo = wizardContentStore()
const updateWizardContent = () => {
  wizardContentInfo.updateContent(
    {
      headerName: 'Expression Info',
      component: HighLevelExpressionInfo,
      props: { expressionId: characterInfo.expressionId },
    } as WizardContent,
  )
}

</script>

<template>
  <Card class="mb-3 align-self-lg-start align-self-md-start align-self-xl-start align-self-sm-stretch" style="max-width: 30em">
    <template #content>
      <form @submit="onSubmit">
        <FormTextWrapper v-model="form.fields.name" :show-skeleton="characterInfo.isLoading" @change="onSubmit" />
        <FormTextWrapper v-model="form.fields.expression" disabled :show-skeleton="characterInfo.isLoading" @change="onSubmit" />
        <!--        <FormDropdownWrapper v-if="showFactionInfo"
          v-model="form.fields.faction" option-label="name" :options="factions" field-name="Faction"
          :show-skeleton="characterInfo.isLoading" :redirect-url="expressionRedirectURL" @change="onSubmit"
        />-->
        <FormTextAreaWrapper v-model="form.fields.background" :show-skeleton="characterInfo.isLoading" @change="onSubmit" />
        <FormCheckboxWrapper v-if="characterInfo.canModifyPrimaryCharacter" v-model="form.fields.isPrimaryCharacter" :show-skeleton="characterInfo.isLoading" @change="onSubmit" />
        <Message v-if="characterInfo.canModifyPrimaryCharacter" severity="info" class="mb-3">
          Toggle above to denote the character you intend on having in play. Only one of these are allowed per account.
          This will make this character visible to GOs for printing the booklet and handing out XP.  It does not give them the ability to modify the character.
        </Message>
        <SelectProgressionPaths :primary-progression="form.fields.primaryProgression" :secondary-progression="form.fields.secondaryProgression" :expression-type-id="characterInfo.expressionId" @change="onSubmit" />
      </form>
      <Button label="Show High Level Expression Info" class="w-100 mb-2 d-block d-md-none " :disabled="characterInfo.isLoading && characterInfo.expressionId !== 0" @click="updateWizardContent" />
    </template>
  </Card>
</template>
