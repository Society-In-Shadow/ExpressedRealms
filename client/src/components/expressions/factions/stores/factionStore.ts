import { defineQuery, defineQueryOptions, useMutation, useQueryCache } from '@pinia/colada'
import toaster from '@/services/Toasters'
import { factionService } from '@/components/expressions/factions/services/factionService.ts'
import { handleValidationErrors } from '@/utilities/piniaColadaUtilities.ts'
import type { EditSingleFactionInfo } from '@/components/expressions/factions/types.ts'

export const FACTION_QUERY_KEYS = {
  root: ['faction'] as const,
  getFactionList: (expressionId: number) => ['faction', 'list', expressionId] as const,
  getFaction: (id: number) => ['faction', id] as const,
}

export const factionListQuery = defineQueryOptions((expressionId: number) => ({
  key: FACTION_QUERY_KEYS.getFactionList(expressionId),
  query: () => factionService.getFactions(expressionId),
}))

export const useDeleteFaction = () => {
  const queryCache = useQueryCache()

  return useMutation({
    mutation: (id: number) => factionService.delete(id),
    async onSuccess() {
      toaster.success('Faction deleted successfully')
      await queryCache.invalidateQueries({ key: FACTION_QUERY_KEYS.root })
    },
  })
}

export const factionQuery = (id: number) =>
  defineQuery(() => ({
    key: FACTION_QUERY_KEYS.getFaction(id),
    query: () => factionService.getFaction(id),
  }))

export const factionUpdate = (onValidationError?: (errors: Record<string, any>) => void | undefined) => {
  const queryCache = useQueryCache()

  return useMutation({
    mutation: ({ id, data }: { id: number, data: EditSingleFactionInfo }) => factionService.editFaction(id, data),
    async onSuccess() {
      await queryCache.invalidateQueries({ key: FACTION_QUERY_KEYS.root })
    },
    onError(error: any) {
      handleValidationErrors(error, onValidationError)
    },
  })
}
