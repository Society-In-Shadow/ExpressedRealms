import { defineQueryOptions } from '@pinia/colada'
import { userService } from '@/components/admin/archetypes/services/archetypeService.ts'

export const ARCHETYPE_QUERY_KEYS = {
  root: ['archetype'] as const,
  summary: ['archetype', 'list'] as const,
}

export const archetypeListQuery = defineQueryOptions({
  key: ARCHETYPE_QUERY_KEYS.summary,
  query: userService.getArchetypes,
})
