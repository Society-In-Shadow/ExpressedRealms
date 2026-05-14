<script setup lang="ts">

import { computed, onMounted, type PropType, ref, watch } from 'vue'
import Button from 'primevue/button'
import modifierGroupStore from '@/components/modifiergroups/stores/modifierGroupStore.ts'
import { SourceTableEnum } from '@/components/modifiergroups/types.ts'
import AddModifier from '@/components/modifiergroups/AddModifier.vue'
import ModifierItem from '@/components/modifiergroups/ModifierItem.vue'

const store = modifierGroupStore()

onMounted(async () => {
  store.sourceType = props.source
  store.sourceTypeName = SourceTableEnum[props.source]
})

const showAdd = ref(false)
const newGroupId = ref(0)

const toggleAdd = () => {
  showAdd.value = !showAdd.value
}

const updateGroupId = (groupId: number) => {
  newGroupId.value = groupId
}

const props = defineProps({
  source: {
    type: Number as PropType<SourceTableEnum>,
    required: true,
    validator: (value: number): value is SourceTableEnum =>
      Object.values(SourceTableEnum).includes(value as SourceTableEnum),
  },
  sourceId: {
    type: Number,
    required: true,
  },
  groupId: {
    type: Number,
    required: false,
    default: null,
  },
})

const groupId = computed(() => props.groupId ?? newGroupId.value)

watch([() => props.groupId, () => newGroupId], async (oldValue, newValue) => {
  store.sourceType = props.source
  store.sourceTypeName = SourceTableEnum[props.source]
  if (groupId.value !== null && groupId.value !== 0 && store.canViewModifiers())
    await store.getModifiers(groupId.value)
}, { immediate: true })

</script>

<template>
  <div v-if="store.canViewModifiers()">
    <h1>Modifiers</h1>
    <div v-for="modifier in store.getModifierList(groupId)" :key="modifier.id">
      <ModifierItem :modifier="modifier" :group-id="groupId" />
    </div>

    <AddModifier
      v-if="showAdd"
      :group-id="groupId"
      :source="props.source"
      :source-id="props.sourceId"
      @canceled="toggleAdd"
      @update-group-id="updateGroupId"
    />
    <Button
      v-if="!showAdd" class="w-100 m-2"
      label="Add Modifier" @click="toggleAdd"
    />
  </div>
</template>
