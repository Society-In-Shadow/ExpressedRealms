import { defineQueryOptions, useMutation, useQueryCache } from '@pinia/colada'
import toaster from '@/services/Toasters'
import { factionService } from '@/components/expressions/factions/services/factionService.ts'

export const FACTION_QUERY_KEYS = {
  root: ['faction'] as const,
  getFactionList: (expressionId: number) => ['faction', 'list', expressionId] as const,
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
