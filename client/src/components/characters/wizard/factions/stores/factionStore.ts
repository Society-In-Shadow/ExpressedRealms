import { defineQueryOptions, useMutation, useQueryCache } from '@pinia/colada'
import { characterFactionService } from '@/components/characters/wizard/factions/services/factionService.ts'
import { handleValidationErrors } from '@/utilities/piniaColadaUtilities.ts'
import type {
  LeaveFactionInfo,
  PickFactionInfo,
  RequestPromotionInfo,
} from '@/components/characters/wizard/factions/types.ts'
import toaster from '@/services/Toasters'
import type { ApprovePromotionInfo } from '@/components/characters/character/factions/types.ts'

export const CHARACTER_FACTION_QUERY_KEYS = {
  root: ['character_faction'] as const,
  getCharacterFactions: (characterId: number) => [...CHARACTER_FACTION_QUERY_KEYS.root, 'list', characterId.toString()] as const,
}

export const pickedFactionQuery = defineQueryOptions((characterId: number) => ({
  key: CHARACTER_FACTION_QUERY_KEYS.getCharacterFactions(characterId),
  query: () => characterFactionService.getCharacterFaction(characterId),
}))

export const pickFaction = (onValidationError?: (errors: Record<string, any>) => void | undefined) => {
  const queryCache = useQueryCache()

  return useMutation({
    mutation: ({ data }: { data: PickFactionInfo }) => characterFactionService.pickFaction(data.characterId, data.factionId),
    async onSuccess() {
      await queryCache.invalidateQueries({ key: CHARACTER_FACTION_QUERY_KEYS.root })
      toaster.success('Successfully joined the faction!')
    },
    onError(error: any) {
      handleValidationErrors(error, onValidationError)
    },
  })
}

export const leaveFaction = (onValidationError?: (errors: Record<string, any>) => void | undefined) => {
  const queryCache = useQueryCache()

  return useMutation({
    mutation: ({ data }: { data: LeaveFactionInfo }) => characterFactionService.leaveFaction(data.characterId),
    async onSuccess() {
      await queryCache.invalidateQueries({ key: CHARACTER_FACTION_QUERY_KEYS.root })
      toaster.success('Successfully left the faction!')
    },
    onError(error: any) {
      handleValidationErrors(error, onValidationError)
    },
  })
}

export const requestPromotion = (onValidationError?: (errors: Record<string, any>) => void | undefined) => {
  const queryCache = useQueryCache()

  return useMutation({
    mutation: ({ data }: { data: RequestPromotionInfo }) => characterFactionService.requestPromotion(data.characterId, data.factionLevelId, data.requestReason),
    async onSuccess() {
      await queryCache.invalidateQueries({ key: CHARACTER_FACTION_QUERY_KEYS.root })
    },
    onError(error: any) {
      handleValidationErrors(error, onValidationError)
    },
  })
}

export const approvePromotion = (onValidationError?: (errors: Record<string, any>) => void | undefined) => {
  const queryCache = useQueryCache()

  return useMutation({
    mutation: ({ data }: { data: ApprovePromotionInfo }) => characterFactionService.approvePromotion(data.characterId, data.factionLevelId, data.approvalReason),
    async onSuccess() {
      await queryCache.invalidateQueries({ key: CHARACTER_FACTION_QUERY_KEYS.root })
    },
    onError(error: any) {
      handleValidationErrors(error, onValidationError)
    },
  })
}
