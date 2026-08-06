<script setup lang="ts">

import Button from 'primevue/button'
import Card from 'primevue/card'
import { useRouter } from 'vue-router'
import { overallCharacterConfirmationService } from '@/components/characters/services/confirmationService.ts'
import Tag from 'primevue/tag'
import ExpressionLogo from '@/components/common/ExpressionLogo.vue'
import type { CharacterListItem } from '@/components/characters/types.ts'
import type { PropType } from 'vue'

const Router = useRouter()
const popupService = overallCharacterConfirmationService()

const props = defineProps({
  character: {
    type: Object as PropType<CharacterListItem>,
    required: true,
  },
})

function editCharacter() {
  Router.push(`/characters/${props.character.id}`)
}

</script>

<template>
  <Card class="mb-3 card-width">
    <template #content>
      <div class="d-flex gap-3">
        <div class="align-self-center" style="max-width: 4em;">
          <ExpressionLogo :expression-sub-type-id="props.character.expressionSubTypeId" />
        </div>
        <div class="flex-fill align-self-center">
          <div>
            {{ props.character.name }}
          </div>
          <em class="mb-3">{{ props.character.expressionName }}</em>
          <div class="mt-1">
            <Tag v-if="props.character.isPrimaryCharacter" value="Primary" severity="info" />
            <Tag v-if="props.character.isRetired" value="Retired" severity="warn" />
          </div>
        </div>
        <div class="d-inline-flex flex-column">
          <Button data-cy="character-edit-button" size="small" label="Edit" class="m-1" @click="editCharacter" />
          <Button data-cy="character-delete-button" size="small" label="Delete" class="m-1" @click="popupService.deleteConfirmation($event, props.character.id)" />
        </div>
      </div>
    </template>
  </Card>
</template>

<style scoped>
  .characterTile >>> .p-card-content{
    padding: 0;
  }
  .card-width {
    width: 20em;
  }

  @media(max-width: 576px){
    .card-width {
      width: 100%;
    }
  }

</style>
