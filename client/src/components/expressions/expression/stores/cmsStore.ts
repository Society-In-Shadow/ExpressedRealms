import { defineQueryOptions, useMutation, useQueryCache } from '@pinia/colada'
import { handleValidationErrors } from '@/utilities/piniaColadaUtilities.ts'
import type {
  AddExpressionPost,
  CopyExpressionPost,
  EditExpressionPut,
} from '@/components/expressions/expression/types.ts'
import { cmsService } from '@/components/expressions/expression/services/cmsService.ts'
import { cmsStore } from '@/stores/cmsStore.ts'

export const CmsService_QUERY_KEYS = {
  root: ['cmsNav'] as const,
  getCmsItems: () => ['cmsNav', 'list'] as const,
  getCmsItem: (id: number) => ['cmsNav', 'list', id] as const,
}

export const cmsItemsQuery = defineQueryOptions(() => ({
  key: CmsService_QUERY_KEYS.getCmsItems(),
  query: () => cmsService.getCmsValues(),
}))

export const cmsItemQuery = defineQueryOptions((id: number) => ({
  key: CmsService_QUERY_KEYS.getCmsItem(id),
  query: () => cmsService.getEdit(id),
}))

export const copy = (onValidationError?: (errors: Record<string, any>) => void | undefined) => {
  const queryCache = useQueryCache()

  return useMutation({
    mutation: ({ id, data }: { id: number, data: CopyExpressionPost }) => cmsService.copy(id, data),
    async onSuccess() {
      await queryCache.invalidateQueries({ key: CmsService_QUERY_KEYS.root })

      // TODO: Temp work around till all expression data is stored in pinia colada
      const cmsData = cmsStore()
      await cmsData.refreshCmsInformation()
    },
    onError(error: any) {
      handleValidationErrors(error, onValidationError)
    },
  })
}

export const edit = (onValidationError?: (errors: Record<string, any>) => void | undefined) => {
  const queryCache = useQueryCache()

  return useMutation({
    mutation: ({ id, data }: { id: number, data: EditExpressionPut }) => cmsService.edit(id, data),
    async onSuccess() {
      await queryCache.invalidateQueries({ key: CmsService_QUERY_KEYS.root })

      // TODO: Temp work around till all expression data is stored in pinia colada
      const cmsData = cmsStore()
      await cmsData.refreshCmsInformation()
    },
    onError(error: any) {
      handleValidationErrors(error, onValidationError)
    },
  })
}

export const create = (onValidationError?: (errors: Record<string, any>) => void | undefined) => {
  const queryCache = useQueryCache()

  return useMutation({
    mutation: ({ data }: { data: AddExpressionPost }) => cmsService.create(data),
    async onSuccess() {
      await queryCache.invalidateQueries({ key: CmsService_QUERY_KEYS.root })

      // TODO: Temp work around till all expression data is stored in pinia colada
      const cmsData = cmsStore()
      await cmsData.refreshCmsInformation()
    },
    onError(error: any) {
      handleValidationErrors(error, onValidationError)
    },
  })
}
