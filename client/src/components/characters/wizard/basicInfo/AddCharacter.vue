<script setup lang="ts">

import Button from 'primevue/button'
import axios from 'axios'
import { useForm } from 'vee-validate'
import { object, string } from 'yup'
import Card from 'primevue/card'
import InputTextWrapper from '@/FormWrappers/InputTextWrapper.vue'
import { computed, onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import DropdownWrapper from '@/FormWrappers/DropdownWrapper.vue'
import { makeIdSafe } from '@/utilities/stringUtilities'
import DropdownInfoWrapper from '@/FormWrappers/DropdownInfoWrapper.vue'
import { FeatureFlags, userStore } from '@/stores/userStore.ts'
import HighLevelExpressionInfo from '@/components/characters/wizard/basicInfo/supporting/HighLevelExpressionInfo.vue'
import { wizardContentStore } from '@/components/characters/wizard/stores/wizardContentStore.ts'
import type { WizardContent } from '@/components/characters/wizard/types.ts'
import { breakpointsBootstrapV5, useBreakpoints } from '@vueuse/core'
import ArchetypeInfo from '@/components/characters/wizard/basicInfo/supporting/ArchetypeInfo.vue'

const userInfo = userStore()
const router = useRouter()
const route = useRoute()
const activeBreakpoint = useBreakpoints(breakpointsBootstrapV5)
const isMobile = activeBreakpoint.smaller('md')

const { defineField, handleSubmit, errors, values } = useForm({
  validationSchema: object({
    name: string().required()
      .max(150)
      .label('Name'),
    background: string().nullable()
      .label('Background'),
    expressionId: object().required()
      .label('Expression'),
    factionId: object().nullable()
      .label('Faction'),
  }),
})

const [name] = defineField('name')
const [background] = defineField('background')
const [expression] = defineField('expressionId')
const [faction] = defineField('factionId')
const expressions = ref([])
const factions = ref([])
const isLoadingFactions = ref(true)
const isLoadingExpressions = ref(true)
const showFactionDropdown = ref(false)
const showArchetypeSelection = ref(false)

onMounted(async () => {
  await axios.get(`/characters/options`)
    .then((response) => {
      expressions.value = response.data.expressions
      isLoadingExpressions.value = false
    })
  showFactionDropdown.value = await userInfo.hasFeatureFlag(FeatureFlags.ShowFactionDropdown)
  showArchetypeSelection.value = await userInfo.hasFeatureFlag(FeatureFlags.ShowArchetypeSelection)
})
const onSubmit = handleSubmit((values) => {
  axios.post('/characters', {
    name: values.name,
    expressionId: values.expressionId.id,
    background: values.background,
    factionId: values.factionId?.id,
    isArchetype: route.query.src === 'archetype_add',
  })
    .then((response) => {
      router.push({ name: 'characterWizard', params: { id: response.data } })
    })
})

async function loadFactions() {
  isLoadingFactions.value = true
  await axios.get(`/characters/factionOptions/${expression.value.id}`)
    .then((response) => {
      faction.value = null
      factions.value = response.data
      isLoadingFactions.value = false
    })
  if (!isMobile.value) {
    updateWizardContent()
  }
}

const expressionRedirectURL = computed(() => {
  if (!isLoadingFactions.value && faction.value) {
    return `/expressions/${expression.value.name.toLowerCase()}#${makeIdSafe(faction.value.name)}`
  }
  return ''
})

const wizardContentInfo = wizardContentStore()
const updateWizardContent = () => {
  wizardContentInfo.updateContent(
    {
      headerName: 'Expression Info',
      component: HighLevelExpressionInfo,
      props: { expressionId: expression.value.id },
    } as WizardContent,
  )
}

async function copyCharacter(characterId: number) {
  const response = await axios.post<number>(`/characters/${characterId}/copy`, {
    characterName: values.name,
    isArchetype: route.query.src === 'archetype_add',
  })

  await router.push({ name: 'characterSheet', params: { id: response.data }, query: { src: 'archetype_copy' } })
}

</script>

<template>
  <div class="flex flex-xs-column flex-sm-column flex-lg-row flex-md-row gap-3">
    <Card class="mb-3 w-100">
      <template #title>
        <h1 class="m-0 p-0">
          Character Creation
        </h1>
      </template>
      <template #content>
        <form @submit="onSubmit">
          <p>For character creation, you first need to pick an Expression.  An Expression will define all of your abilities and powers.</p>
          <p>
            There are 6 to choose from, and you can find out more about them through the dropdown below.
          </p>
          <InputTextWrapper v-model="name" field-name="Name" :error-text="errors.name" />
          <DropdownWrapper
            v-model="expression" option-label="name" :options="expressions" field-name="Expression" :error-text="errors.expressionId"
            :show-skeleton="isLoadingExpressions" @change="loadFactions()"
          />

          <HighLevelExpressionInfo v-if="isMobile && expression" :expression-id="expression.id" />

          <DropdownInfoWrapper
            v-if="expression && showFactionDropdown" v-model="faction" option-label="name" :options="factions" field-name="Faction"
            :error-text="errors.factionId" :disabled="!expression" :redirect-url="expressionRedirectURL" :show-skeleton="isLoadingFactions" :redirect-to-different-page="true"
          />

          <div v-if="expression">
            <h1>Creation Type</h1>
            <h2>Make Custom</h2>
            <p>You can create your own from scratch.  This provides you with full flexibilty to build as you wish.</p>
            <Button data-cy="add-character-button" label="Create Custom Character" class="w-100 mb-2" type="submit" />
            <div v-if="showArchetypeSelection">
              <h2>Choose Archetype</h2>
              <p>Or select an archetype and tweak or use that build directly.  This will limit some of the choices you have.</p>
              <ArchetypeInfo :expression-id="expression.id" @selected-archetype="copyCharacter" />
            </div>

            <Button label="Back" class="w-100 mt-2" severity="secondary" @click="router.push({name: 'characters'})" />
          </div>
        </form>
      </template>
    </Card>
  </div>
</template>
