import { defineQueryOptions, useMutation, useQueryCache } from '@pinia/colada'
import { archetypeService } from '@/components/admin/archetypes/services/archetypeService.ts'

export const ARCHETYPE_QUERY_KEYS = {
  root: ['archetype'] as const,
  summary: ['archetype', 'list'] as const,
}

export const archetypeListQuery = defineQueryOptions({
  key: ARCHETYPE_QUERY_KEYS.summary,
  query: archetypeService.getArchetypes,
})

export const useDeleteArchetype = () => {
  const queryCache = useQueryCache()

  return useMutation({
    mutation: (id: number) => archetypeService.delete(id),
    async onSuccess() {
      await queryCache.invalidateQueries({ key: ARCHETYPE_QUERY_KEYS.root })
    },
  })
}
