<script setup lang="ts">
import Button from 'primevue/button'
import Card from 'primevue/card'
import { computed, defineAsyncComponent, markRaw, onBeforeMount, ref, watch } from 'vue'
import KnowledgeStep from '@/components/characters/wizard/knowledges/KnowledgeStep.vue'
import { experienceStore } from '@/components/characters/character/stores/experienceBreakdownStore.ts'
import PowerStep from '@/components/characters/wizard/powers/PowerStep.vue'
import StatStep from '@/components/characters/wizard/stats/StatStep.vue'
import SkillStep from '@/components/characters/wizard/skills/SkillStep.vue'
import { useRoute, useRouter } from 'vue-router'
import DataTable from 'primevue/datatable'
import EditCharacterDetails from '@/components/characters/wizard/basicInfo/EditCharacterDetails.vue'
import AddCharacter from '@/components/characters/wizard/basicInfo/AddCharacter.vue'
import WizardContent from '@/components/characters/wizard/WizardContent.vue'
import { breakpointsBootstrapV5, useBreakpoints } from '@vueuse/core'
import { wizardContentStore } from '@/components/characters/wizard/stores/wizardContentStore.ts'
import ReviewCharacter from '@/components/characters/character/xp/ReviewCharacter.vue'
import AdvantageStep from '@/components/characters/wizard/blessings/AdvantageStep.vue'
import { characterStore } from '@/components/characters/character/stores/characterStore.ts'
import DisadvantageStep from '@/components/characters/wizard/blessings/DisadvantageStep.vue'
import ContactStep from '@/components/characters/wizard/contacts/ContactStep.vue'
import { userPermissionStore } from '@/stores/userPermissionStore.ts'

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
  { name: 'Basic Info', component: isAdd.value ? markRaw(AddCharacter) : markRaw(EditCharacterDetails) },
  { name: 'Stats', isDisabled: isAdd.value, component: markRaw(StatStep) },
  { name: 'Skills', isDisabled: isAdd.value, component: markRaw(SkillStep) },
  { name: 'Powers', isDisabled: isAdd.value, component: markRaw(PowerStep) },
  { name: 'Knowledges', isDisabled: isAdd.value, component: markRaw(KnowledgeStep) },
  { name: 'Contacts', isDisabled: isAdd.value, visible: () => !characterInfo.isInCharacterCreation && !isAdd.value, component: markRaw(ContactStep) },
  { name: 'Advantages', isDisabled: isAdd.value, component: defineAsyncComponent(async () => AdvantageStep) },
  { name: 'Disadvantages', isDisabled: isAdd.value, component: defineAsyncComponent(async () => DisadvantageStep) },
  { name: 'Review Character', isDisabled: isAdd.value, component: markRaw(ReviewCharacter) },
])

onBeforeMount(async () => {
  await fetchData()
})

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

    if (!isMobile.value) {
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

const selectedSection = ref<string>('')
const currentView = computed(() => sections.value.filter(x => x.name == selectedSection.value)[0].component)
const hasSelectedSection = computed(() => selectedSection.value !== '')

const selectSection = (name: string) => {
  wizardContentData.hideContent()
  selectedSection.value = name
}

const resetSection = () => {
  wizardContentData.hideContent()
  selectedSection.value = ''
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
  <div class="d-none">
    <DataTable />
  </div>
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
