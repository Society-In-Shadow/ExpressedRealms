import { defineQuery, useMutation, useQueryCache } from '@pinia/colada'

import toaster from '@/services/Toasters'
import { goFieldsService } from '@/components/admin/characterList/services/goFieldsService.ts'
import type { GoFields } from '@/components/admin/characterList/types.ts'

export const GO_FIELDS_QUERY_KEYS = {
  root: ['goFields'] as const,
}

export const goFieldQuery = (characterId: number) =>
  defineQuery(() => ({
    key: ['goFields', characterId],
    query: () => goFieldsService.getGoFields(characterId),
  }))

export const useUpdateGoFields = () => {
  const queryCache = useQueryCache()

  return useMutation({
    mutation: ({ id, data }: { id: number, data: GoFields }) => goFieldsService.updateGoFields(id, data),
    async onSuccess() {
      toaster.success('Successfully Updated Go Fields!')
      await queryCache.invalidateQueries({ key: GO_FIELDS_QUERY_KEYS.root })
    },
  })
}
