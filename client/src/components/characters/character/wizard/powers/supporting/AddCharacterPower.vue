<script setup lang="ts">

import Message from 'primevue/message'
import FormTextAreaWrapper from '@/FormWrappers/FormTextAreaWrapper.vue'
import Button from 'primevue/button'
import { getValidationInstance } from '@/components/characters/character/powers/validations/powerValidations.ts'
import { useRoute } from 'vue-router'
import { computed, type PropType, ref, watch } from 'vue'
import { characterPowersStore } from '@/components/characters/character/powers/stores/characterPowerStore.ts'
import type { Power } from '@/components/characters/character/powers/types.ts'
import PowerDetails from '@/components/characters/character/wizard/powers/supporting/PowerDetails.vue'
import ShowXPCosts from '@/components/characters/character/wizard/ShowXPCosts.vue'
import { experienceStore, XpSectionTypes } from '@/components/characters/character/stores/experienceBreakdownStore.ts'
import type { CalculatedExperience } from '@/components/characters/character/types.ts'

const store = characterPowersStore()
const form = getValidationInstance()
const route = useRoute()
const xpInfo = experienceStore()

const props = defineProps({
  power: {
    type: Object as PropType<Power>,
    required: true,
  },
})

const availableXp = ref(0)
const powerXp = ref(0)
const sectionInfo = ref<CalculatedExperience>({})

const disabled = computed(() => {
  return sectionInfo.value.availableXp < powerXp.value
})

watch(() => props.power, async (newValue, oldValue) => {
  const values = await store.getPowerOptions(route.params.id, props.power.id)
  availableXp.value = values.availableXp
  powerXp.value = values.powerXp
  sectionInfo.value = xpInfo.getExperienceInfoForSection(XpSectionTypes.powers)
  console.log('triggered')
}, { immediate: true, deep: true })

/* onMounted(async () => {
  const values = await store.getPowerOptions(route.params.id, props.power.id);
  availableXp.value = values.availableXp;
  powerXp.value = values.powerXp;
  sectionInfo.value = xpInfo.getExperienceInfoForSection(XpSectionTypes.powers);
}) */

const onSubmit = form.handleSubmit(async (values) => {
  await store.addPower(values, route.params.id, props.power.id)
})

</script>

<template>
  <PowerDetails :power="props.power" />

  <ShowXPCosts :section-type="XpSectionTypes.powers" />
  <div><strong>Cost:</strong> {{ powerXp }}</div>
  <Message v-if="disabled" severity="warn" class="my-3">
    You do not have enough experience to add this power
  </Message>
  <form class="mt-3" @submit="onSubmit">
    <FormTextAreaWrapper v-model="form.notes" :disabled="disabled" />

    <div class="m-3 text-right">
      <Button label="Add" class="m-2" type="submit" :disabled="disabled" />
    </div>
  </form>
</template>
