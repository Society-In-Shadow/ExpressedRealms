import { defineQueryOptions } from '@pinia/colada'
import { goService } from '@/components/conCheckin/services/goService.ts'

export const GO_CHECKS_QUERY_KEYS = {
  root: ['goChecks'] as const,
  getChecks: (characterId: number) => ['goChecks', 'list', characterId] as const,
}

export const goCheckList = defineQueryOptions((characterId: number) => ({
  key: GO_CHECKS_QUERY_KEYS.getChecks(characterId),
  query: () => goService.getGoChecks(characterId),
  select: data => ({
    contacts: data.contacts.map(x => ({
      ...x,
      isReviewed: false,
    })),
    knowledgeChecks: data.knowledgeChecks.map(x => ({
      ...x,
      isReviewed: false,
    })),
  }),
}))
