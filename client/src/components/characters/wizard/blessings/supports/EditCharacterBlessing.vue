<script setup lang="ts">

import FormTextAreaWrapper from '@/FormWrappers/FormTextAreaWrapper.vue'
import Button from 'primevue/button'
import { useRoute } from 'vue-router'
import { computed, type PropType, ref, watch } from 'vue'
import type { Blessing, BlessingLevel } from '@/components/blessings/types.ts'
import RadioButton from 'primevue/radiobutton'
import { getValidationInstance } from '@/components/characters/wizard/blessings/validators/blessingValidations.ts'
import { characterBlessingsStore } from '@/components/characters/wizard/blessings/stores/characterBlessingStore.ts'
import { confirmationPopup } from '@/components/characters/wizard/blessings/services/confirmationService.ts'
import {
  experienceStore,
  type XpSectionType,
  XpSectionTypes,
} from '@/components/characters/character/stores/experienceBreakdownStore.ts'
import Message from 'primevue/message'
import ShowXPCosts from '@/components/characters/wizard/ShowXPCosts.vue'
import { characterStore } from '@/components/characters/character/stores/characterStore.ts'

const store = characterBlessingsStore()
const form = getValidationInstance()
const route = useRoute()
const popupService = confirmationPopup(route.params.id)
const experienceInfo = experienceStore()
const characterInfo = characterStore()

const props = defineProps({
  blessing: {
    type: Object as PropType<Blessing>,
    required: true,
  },
})

const mappingId = ref(0)
const currentLevel = ref<BlessingLevel>({})
const currentCost = computed(() => {
  return props.blessing.type.toLowerCase() == 'disadvantage' ? currentLevel.value.xpGain : currentLevel.value.xpCost
})
const availableXp = ref(0)

const loadData = async () => {
  const currentBlessing = store.blessings.find(x => x.blessingId == props.blessing?.id)
  currentLevel.value = props.blessing.levels.find(x => x.id == currentBlessing.blessingLevelId) ?? {}
  mappingId.value = currentBlessing.id
  form.setValues(currentBlessing, currentLevel.value)
  let sectionType: XpSectionType = props.blessing.type.toLowerCase() == 'disadvantage' ? XpSectionTypes.disadvantage : XpSectionTypes.advantage
  let xpInfo = experienceInfo.getExperienceInfoForSection(sectionType)
  availableXp.value = xpInfo.availableXp + currentCost.value
}

watch(() => props.blessing.id, async () => {
  await loadData()
}, { immediate: true })

const onSubmit = form.handleSubmit(async (values) => {
  const currentBlessing = store.blessings.filter(x => x.blessingId == props.blessing?.id)[0]
  await store.updateBlessing(values, route.params.id, currentBlessing.id)
})

function disableOption(level: BlessingLevel) {
  if (props.blessing.type.toLowerCase() == 'disadvantage') {
    return level.xpGain > availableXp.value && level.xpGain > currentLevel.value.xpGain
  }
  return level.xpCost > availableXp.value && level.xpCost > currentLevel.value.xpCost
}

const canOnlyDelete = computed(() => {
  const levelId = form.blessingLevel.field.value?.id ?? -1
  let indexLevel = props.blessing.levels.findIndex(x => x.id === levelId)
  return indexLevel == 0 && availableXp.value == 0
})

const canLowerOrDelete = computed(() => {
  const levelId = form.blessingLevel.field.value?.id ?? -1
  let indexLevel = props.blessing.levels.findIndex(x => x.id === levelId)
  return indexLevel > 0
})

const xpSectionType = computed(() => {
  return props.blessing.type.toLowerCase() == 'disadvantage' ? XpSectionTypes.disadvantage : XpSectionTypes.advantage
})

</script>

<template>
  <form @submit="onSubmit">
    <div class="d-flex flex-column flex-md-row align-self-center justify-content-between">
      <div>
        <h2 class="p-0 m-0">
          {{ props.blessing.name }}
        </h2>
      </div>
      <div class="p-0 m-2 d-inline-flex align-items-start align-items-center gap-2">
        <Button v-if="characterInfo.isInCharacterCreation" label="Delete" size="small" severity="danger" @click="popupService.deleteConfirmation($event, mappingId )" />
        <Button label="Update" size="small" type="submit" />
      </div>
    </div>

    <div v-html="props.blessing?.description" />
    <ShowXPCosts v-if="characterInfo.isInCharacterCreation" :section-type="xpSectionType" class="pt-3" :additional-available-xp="currentCost" />
    <div v-if="!characterInfo.isInCharacterCreation">
      <Message severity="warn" class="mt-4">
        You may only modify this {{ props.blessing.type.toLowerCase() }} during character creation.
      </Message>
    </div>
    <div v-else-if="canLowerOrDelete">
      <Message severity="warn" class="mt-4">
        You may lower the level of this {{ props.blessing.type.toLowerCase() }} or delete it, but you cannot increase it
        due to insufficient XP.
      </Message>
    </div>
    <div v-else-if="canOnlyDelete">
      <Message severity="warn" class="mt-4">
        You may only delete this {{ props.blessing.type.toLowerCase() }}.
      </Message>
    </div>
    <div v-for="level in props.blessing.levels" :key="level.id" class="mt-3">
      <div class="d-flex flex-column flex-md-row align-self-center">
        <RadioButton v-model="form.blessingLevel.field.value" :input-id="level.id.toString()" :value="level" class="mr-4" :disabled="disableOption(level) || !characterInfo.isInCharacterCreation" />
        <label :for="level.id.toString()" :class="disableOption(level) ? 'non-selectable' : ''">{{ level.name }} – {{ level.description }}</label>
      </div>
    </div>

    <div class="mt-4">
      <FormTextAreaWrapper v-model="form.notes" />
    </div>
  </form>
</template>

<style>
:deep(th.text-center .p-datatable-column-header-content) {
  justify-content: center;
}
.non-selectable { opacity:.6; pointer-events:none; }
</style>
