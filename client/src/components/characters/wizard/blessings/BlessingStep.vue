<script setup lang="ts">

import { computed, onBeforeMount, ref } from 'vue'
import { blessingsStore } from '@/components/blessings/stores/blessingsStore'
import { UserRoles, userStore } from '@/stores/userStore.ts'
import { makeIdSafe } from '@/utilities/stringUtilities.ts'
import SelectBlessingItem from '@/components/characters/wizard/blessings/supports/SelectBlessingItem.vue'
import { characterBlessingsStore } from '@/components/characters/wizard/blessings/stores/characterBlessingStore.ts'
import { useRoute } from 'vue-router'
import type { Blessing, SubCategory } from '@/components/blessings/types.ts'
import Button from 'primevue/button'
import EditCharacterBlessing from '@/components/characters/wizard/blessings/supports/EditCharacterBlessing.vue'
import { wizardContentStore } from '@/components/characters/wizard/stores/wizardContentStore.ts'
import type { WizardContent } from '@/components/characters/wizard/types.ts'
import { experienceStore, XpSectionTypes } from '@/components/characters/character/stores/experienceBreakdownStore.ts'
import ShowXPCosts from '@/components/characters/wizard/ShowXPCosts.vue'
import { characterStore } from '@/components/characters/character/stores/characterStore.ts'
import BlessingInfo from '@/components/characters/wizard/blessings/BlessingInfo.vue'
import { breakpointsBootstrapV5, useBreakpoints } from '@vueuse/core'
import Skeleton from 'primevue/skeleton'

const route = useRoute()
const store = blessingsStore()
const characterBlessingData = characterBlessingsStore()
const userInfo = userStore()
const xpInfo = experienceStore()
const characterInfo = characterStore()

const props = defineProps({
  type: {
    type: String,
    required: true,
  },
  isReadOnly: {
    type: Boolean,
    required: true,
  },
})

const showEdit = ref(false)
const characterId = Number.parseInt(route.params.id as string)

const activeBreakpoint = useBreakpoints(breakpointsBootstrapV5)
const isMobile = activeBreakpoint.smaller('md')

onBeforeMount(async () => {
  await store.getBlessings()
  showEdit.value = await userInfo.hasUserRole(UserRoles.BlessingsManagementRole)
  await characterBlessingData.getCharacterBlessings(characterId)
  await xpInfo.getExperience(characterId)
  if (!isMobile.value) {
    showAboutInfo()
  }
})

const filteredTypes = computed(() => {
  const selectedIds = characterBlessingData.selectedBlessingIds

  return store.types.map((type) => {
    const subCategories = type.subCategories.map((subCategory) => {
      return {
        name: subCategory.name,
        blessings: subCategory.blessings.filter(x => !selectedIds.includes(x.id)),
      } as SubCategory
    })

    return {
      ...type,
      subCategories: subCategories,
    }
  })
})

const wizardContentInfo = wizardContentStore()

const currentType = computed(() => {
  return filteredTypes.value.find(y => y.name.toLowerCase() == props.type.toLowerCase())
})

const selectedType = computed(() => {
  return characterBlessingData.types.find(y => y.name.toLowerCase() == props.type.toLowerCase())
})

const updateWizardContent = (blessing: Blessing) => {
  wizardContentInfo.updateContent(
    {
      headerName: 'Edit Blessing',
      component: EditCharacterBlessing,
      props: { blessing: blessing },
    } as WizardContent,
  )
}

const showAboutInfo = () => {
  wizardContentInfo.updateContent(
    {
      headerName: (props.type.toLowerCase() == 'disadvantage' ? 'Disadvantage Info' : 'Advantage Info'),
      component: BlessingInfo,
      props: { type: props.type },
    } as WizardContent,
  )
}

const xpSectionType = computed(() => {
  return props.type.toLowerCase() == 'disadvantage' ? XpSectionTypes.disadvantage : XpSectionTypes.advantage
})

function getBlessingLevelName(blessing: Blessing) {
  const currentBlessing = characterBlessingData.blessings.find(x => x.blessingId == blessing?.id)
  return blessing.levels.find(x => x.id == currentBlessing.blessingLevelId).name
}

</script>

<template>
  <div v-if="characterBlessingData.isLoading">
    <Skeleton height="40px" />
    <Skeleton height="30px" class="mt-3" />
    <Skeleton height="30px" class="mt-3" />
    <Skeleton height="40px" />
    <Skeleton height="30px" class="mt-3" />
    <Skeleton height="30px" class="mt-3" />
    <Skeleton height="40px" />
    <Skeleton height="30px" class="mt-3" />
    <Skeleton height="30px" class="mt-3" />
  </div>
  <div v-else>
    <Button label="Help" class="mb-2 float-end" @click="showAboutInfo" />

    <div v-if="!selectedType && characterInfo.isInCharacterCreation">
      No selected {{ props.type.toLowerCase() == 'advantage' ? 'advantages' : 'disadvantages' }} detected, please pick one below!
    </div>
    <div v-else-if="!selectedType && !characterInfo.isInCharacterCreation">
      Below is a list of all available {{ props.type.toLowerCase() == 'advantage' ? 'advantages' : 'disadvantages' }}.
    </div>
    <div v-else>
      <h1>Selected {{ selectedType!.name }}s</h1>
      <div v-for="trait in selectedType!.subCategories" :key="trait.name">
        <h3 class="ml-3 pb-2">
          {{ trait.name }}
        </h3>
        <div v-for="blessing in trait.blessings" :key="blessing.id" class="mb-3">
          <div class="pl-5 d-flex flex-row align-self-center justify-content-between">
            <div>
              <h3 class="p-0 m-0">
                {{ blessing.name }} - {{ getBlessingLevelName(blessing) }}
              </h3>
            </div>
            <div class="p-0">
              <Button label="View" size="small" @click="updateWizardContent(blessing)" />
            </div>
          </div>
        </div>
      </div>
    </div>

    <h1 class="pb-0 mb-0">
      Available {{ currentType!.name }}s
    </h1>
    <ShowXPCosts v-if="characterInfo.isInCharacterCreation" :section-type="xpSectionType" />
    <div v-for="subCategory in currentType!.subCategories" :key="subCategory.name">
      <h2 :id="makeIdSafe(subCategory.name)" class="pl-md-3 pb-2">
        {{ subCategory.name }}
      </h2>
      <div v-for="blessing in subCategory.blessings" :key="blessing.id">
        <SelectBlessingItem :blessing="blessing" :is-read-only="props.isReadOnly" />
      </div>
    </div>
  </div>
</template>
