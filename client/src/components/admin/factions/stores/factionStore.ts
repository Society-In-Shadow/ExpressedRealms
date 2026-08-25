import { defineQueryOptions } from '@pinia/colada'
import { adminFactionService } from '@/components/admin/factions/services/adminFactionService.ts'

export const ADMIN_FACTIONS_QUERY_KEYS = {
  root: ['admin_factions'] as const,
  summary: ['admin_factions', 'list'] as const,
}

export const participantsQuery = defineQueryOptions({
  key: ADMIN_FACTIONS_QUERY_KEYS.summary,
  query: adminFactionService.getFactionParticipants,
})
