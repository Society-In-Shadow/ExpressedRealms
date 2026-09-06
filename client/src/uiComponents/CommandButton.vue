<script setup lang="ts">

import { computed } from 'vue'
import SplitButton from 'primevue/splitbutton'
import Button from 'primevue/button'
import type { HintedString } from '@primevue/core'

export interface Command {
  label: string
  command: (event: any) => void
  isVisible?: () => boolean
  severity?: string | 'secondary' | 'success' | 'info' | 'warn' | 'help' | 'danger' | 'contrast'
}

const props = defineProps<{
  commands: Command[]
  size?: HintedString<'small' | 'large'>
}>()
const visibleCommands = computed(() =>
  props.commands.filter(command => command.isVisible?.() ?? true),
)
const mainCommand = computed(() => visibleCommands.value[0])
const subCommands = computed(() => visibleCommands.value.slice(1).map(command => ({ label: command.label, command: command.command })))

</script>

<template>
  <div v-if="visibleCommands.length == 0" />
  <div v-else-if="visibleCommands.length == 1">
    <Button :label="mainCommand.label" :severity="mainCommand.severity" :size="props.size" @click="mainCommand.command" />
  </div>
  <div v-else>
    <SplitButton :label="mainCommand.label" :severity="mainCommand.severity" :model="subCommands" :size="props.size" @click="mainCommand.command" />
  </div>
</template>
