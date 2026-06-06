import { defineQuery, useMutation, useQueryCache } from '@pinia/colada'

import { goFieldsService } from '@/components/admin/characterList/services/goFieldsService.ts'
import { handleValidationErrors } from '@/utilities/piniaColadaUtilities.ts'
import type { GoFields } from '@/components/admin/characterList/types.ts'

export const GO_FIELDS_QUERY_KEYS = {
  root: ['goFields'] as const,
}

export const goFieldQuery = (characterId: number) =>
  defineQuery(() => ({
    key: ['goFields', characterId],
    query: () => goFieldsService.getGoFields(characterId),
  }))

export const useUpdateGoFields = (onValidationError?: (errors: Record<string, any>) => void | undefined) => {
  const queryCache = useQueryCache()

  return useMutation({
    mutation: ({ id, data }: { id: number, data: GoFields }) => goFieldsService.updateGoFields(id, data),
    async onSuccess() {
      await queryCache.invalidateQueries({ key: GO_FIELDS_QUERY_KEYS.root })
    },
    onError(error: any) {
      handleValidationErrors(error, onValidationError)
    },
  })
}
