import { defineQueryOptions, useMutation, useQueryCache } from '@pinia/colada'
import { characterFactionService } from '@/components/characters/wizard/factions/services/factionService.ts'
import { handleValidationErrors } from '@/utilities/piniaColadaUtilities.ts'
import type { PickFactionInfo } from '@/components/characters/wizard/factions/types.ts'

export const CHARACTER_FACTION_QUERY_KEYS = {
  root: ['character_faction'] as const,
  getCharacterFactions: (characterId: number) => ['character_faction', 'list', characterId] as const,
}

export const pickedFactionQuery = defineQueryOptions((characterId: number) => ({
  key: CHARACTER_FACTION_QUERY_KEYS.getCharacterFactions(characterId),
  query: () => characterFactionService.getCharacterFaction(characterId),
  enabled: () => characterId != 0,
}))

export const pickFaction = (onValidationError?: (errors: Record<string, any>) => void | undefined) => {
  const queryCache = useQueryCache()

  return useMutation({
    mutation: ({ data }: { data: PickFactionInfo }) => characterFactionService.pickFaction(data.characterId, data.factionId),
    async onSuccess() {
      await queryCache.invalidateQueries({ key: CHARACTER_FACTION_QUERY_KEYS.root })
    },
    onError(error: any) {
      handleValidationErrors(error, onValidationError)
    },
  })
}
