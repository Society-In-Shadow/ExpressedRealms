<script setup lang="ts">

import { computed, type PropType } from 'vue'
import SplitButton from 'primevue/splitbutton'
import Button from 'primevue/button'

export interface Command {
  label: string
  command: (event: any) => void
  isVisible?: boolean
  severity?: string | 'secondary' | 'success' | 'info' | 'warn' | 'help' | 'danger' | 'contrast'
}

const props = defineProps({
  commands: {
    type: Object as PropType<Command[]>,
    required: true,
  },
})

const mainCommand = computed(() => props.commands[0])
const subCommands = computed(() => props.commands.slice(1).map(command => ({ label: command.label, command: command.command })))

</script>

<template>
  <div v-if="props.commands.length == 0" />
  <div v-else-if="props.commands.length == 1">
    <Button :label="mainCommand.label" :severity="mainCommand.severity" @click="mainCommand.command" />
  </div>
  <div v-else>
    <SplitButton :label="mainCommand.label" :severity="mainCommand.severity" :model="subCommands" @click="mainCommand.command" />
  </div>
</template>
