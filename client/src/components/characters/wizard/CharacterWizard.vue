<script setup lang="ts">
import Button from 'primevue/button'
import Card from 'primevue/card'
import { computed, defineAsyncComponent, onBeforeMount, ref, watch } from 'vue'
import { experienceStore } from '@/components/characters/character/stores/experienceBreakdownStore.ts'
import { useRoute, useRouter } from 'vue-router'
import WizardContent from '@/components/characters/wizard/WizardContent.vue'
import { breakpointsBootstrapV5, useBreakpoints } from '@vueuse/core'
import { wizardContentStore } from '@/components/characters/wizard/stores/wizardContentStore.ts'
import { characterStore } from '@/components/characters/character/stores/characterStore.ts'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'
import { hasFlag } from '@/stores/featureFlags/featureFlagStore.ts'

const BasicInfoStep = defineAsyncComponent(() =>
  import('@/components/characters/wizard/basicInfo/EditCharacterDetails.vue'),
)
const AddCharacterStep = defineAsyncComponent(() =>
  import('@/components/characters/wizard/basicInfo/AddCharacter.vue'),
)
const StatStep = defineAsyncComponent(() =>
  import('@/components/characters/wizard/stats/StatStep.vue'),
)
const SkillStep = defineAsyncComponent(() =>
  import('@/components/characters/wizard/skills/SkillStep.vue'),
)
const FactionSelectList = defineAsyncComponent(() =>
  import('@/components/characters/wizard/factions/FactionSelectionList.vue'),
)
const PowerStep = defineAsyncComponent(() =>
  import('@/components/characters/wizard/powers/PowerStep.vue'),
)
const KnowledgeStep = defineAsyncComponent(() =>
  import('@/components/characters/wizard/knowledges/KnowledgeStep.vue'),
)
const ContactStep = defineAsyncComponent(() =>
  import('@/components/characters/wizard/contacts/ContactStep.vue'),
)
const AdvantageStep = defineAsyncComponent(() =>
  import('@/components/characters/wizard/blessings/AdvantageStep.vue'),
)
const DisadvantageStep = defineAsyncComponent(() =>
  import('@/components/characters/wizard/blessings/DisadvantageStep.vue'),
)
const ReviewCharacterStep = defineAsyncComponent(() =>
  import('@/components/characters/character/xp/ReviewCharacter.vue'),
)

const xpData = experienceStore()
const route = useRoute()
const router = useRouter()
const characterInfo = characterStore()
const isAdd = computed(() => route.name == 'addWizard')
const wasAdd = ref(false)

const userPermissionInfo = userPermissionStore()
const permissionCheck = userPermissionInfo.permissionCheck
const activeBreakpoint = useBreakpoints(breakpointsBootstrapV5)
const isMobile = activeBreakpoint.smaller('md')
const wizardContentData = wizardContentStore()

const sections = computed(() => [
  { name: 'Basic Info', component: isAdd.value ? AddCharacterStep : BasicInfoStep },
  { name: 'Stats', isDisabled: isAdd.value, component: StatStep },
  { name: 'Skills', isDisabled: isAdd.value, component: SkillStep },
  { name: 'Faction', isDisabled: isAdd.value, component: FactionSelectList, visible: () => hasFlag.ShowFactions },
  { name: 'Powers', isDisabled: isAdd.value, component: PowerStep },
  { name: 'Knowledges', isDisabled: isAdd.value, component: KnowledgeStep },
  { name: 'Contacts', isDisabled: isAdd.value, visible: () => !characterInfo.isInCharacterCreation && !isAdd.value, component: ContactStep },
  { name: 'Advantages', isDisabled: isAdd.value, component: AdvantageStep },
  { name: 'Disadvantages', isDisabled: isAdd.value, component: DisadvantageStep },
  { name: 'Review Character', isDisabled: isAdd.value, component: ReviewCharacterStep },
])

onBeforeMount(async () => {
  await fetchData()
})

const routeSection = computed(() => {
  const section = route.query.section

  return typeof section === 'string' ? section : null
})

const visibleSections = computed(() =>
  sections.value.filter(section => section.visible ? section.visible() : true),
)

const isValidRouteSection = computed(() =>
  visibleSections.value.some(section => section.name === routeSection.value && !section.isDisabled),
)

async function fetchData() {
  if (!characterInfo.isOwner && !permissionCheck.Archetypes.Edit && !isAdd.value) {
    await router.push({ name: 'characterSheet', params: { id: Number.parseInt(route.params.id as string) } })
  }

  if (isAdd.value) {
    selectSection('Basic Info')
    wasAdd.value = true
  }
  else {
    await characterInfo.getCharacterDetails(Number(route.params.id))
    if (characterInfo.isRetired)
      await router.push({ name: 'characters' })

    await xpData.getExperience(route.params.id)

    if (isValidRouteSection.value) {
      selectSection(routeSection.value!)
    }
    else if (!isMobile.value) {
      selectSection('Basic Info')
    }
  }
}

watch(
  () => route.path,
  async (newPath, oldPath) => {
    if (newPath !== oldPath) {
      await fetchData()
    }
  },
)

watch(
  () => route.query.section,
  () => {
    if (isValidRouteSection.value) {
      selectedSection.value = routeSection.value!
    }
    else if (!routeSection.value) {
      selectedSection.value = ''
    }
  },
)

const selectedSection = ref<string>('')
const currentView = computed(() => sections.value.filter(x => x.name == selectedSection.value)[0].component)
const hasSelectedSection = computed(() => selectedSection.value !== '')

const selectSection = async (name: string) => {
  wizardContentData.hideContent()
  selectedSection.value = name

  await router.push({
    query: {
      ...route.query,
      section: name,
    },
  })
}

const resetSection = async () => {
  wizardContentData.hideContent()
  selectedSection.value = ''

  const query = { ...route.query }
  delete query.section

  await router.replace({ query })
}

const redirectToEdit = () => {
  router.push({ name: 'characterSheet', params: { id: route.params.id } })
}

const previousSection = computed(() => {
  const index = sections.value.findIndex(x => x.name == selectedSection.value)
  if (index > 0) {
    return sections.value[index - 1].name
  }
  return null
})

const nextSection = computed(() => {
  const index = sections.value.findIndex(x => x.name == selectedSection.value)
  if (index < sections.value.length - 1) {
    return sections.value[index + 1].name
  }
})

</script>

<template>
  <div v-if="!isAdd" class="d-flex justify-content-between">
    <div>
      <Button v-if="isMobile && hasSelectedSection" label="Back to Sections" @click="resetSection" />
    </div>
  </div>
  <div class="row pt-3">
    <div v-if="!(isMobile && hasSelectedSection) || !isMobile" class="col col-md-2 custom-toc">
      <Card>
        <template #content>
          <div v-for="section in sections" :key="section.name">
            <Button
              v-if="section.visible ? section.visible() : true"
              class="w-100 mb-2 mt-2" :label="section.name"
              :outlined="selectedSection !== section.name"
              :disabled="section.isDisabled"
              @click="selectSection(section.name)"
            />
          </div>
          <div v-if="!isAdd">
            <Button
              class="w-100 mb-2 mt-2" label="Character Sheet" :outlined="true" icon="pi pi-arrow-left"
              @click="redirectToEdit"
            />
          </div>
        </template>
      </Card>
    </div>
    <div v-if="!isMobile &&!hasSelectedSection" class="col custom-toc">
      <Card>
        <template #content>
          <div>Please select a section on the left to get started!</div>
        </template>
      </Card>
    </div>
    <div v-if="hasSelectedSection" class="col custom-toc">
      <Card>
        <template #content>
          <!-- Dynamically load the chosen section in the middle column -->
          <component :is="currentView" />
          <div v-if="!isAdd" class="d-flex flex-row justify-content-between mt-3">
            <div>
              <Button v-if="previousSection" size="small" :label="'<< ' + previousSection" @click="selectSection(previousSection)" />
            </div>
            <div>
              <Button v-if="nextSection" size="small" :label="nextSection + ' >>'" @click="selectSection(nextSection)" />
            </div>
          </div>
        </template>
      </Card>
    </div>
    <div class="col custom-toc col-12 col-md">
      <WizardContent />
    </div>
  </div>
</template>

<style>
@media(min-width: 768px){
  .custom-toc {
    max-height: calc(100vh - 1rem);
    overflow-y: auto;
    height:100%;
    min-width: 18em;
  }
}

@media(max-width: 768px){
  .custom-toc > .p-card-body{
    padding-left: 1rem !important;
    padding-right: 1rem !important;
  }
}

.main-container{
  max-width: 1500px !important;
}
</style>
