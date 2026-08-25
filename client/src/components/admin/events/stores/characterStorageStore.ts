import { defineQueryOptions } from '@pinia/colada'
import { characterStorageService } from '@/components/admin/events/services/characterStorageService.ts'

export const CHARACTER_STORAGE_QUERY_KEYS = {
  root: ['character_storage'] as const,
  characterStorageEventList: (id: number) => ['character_storage', 'event_list', id] as const,
}

export const characterStorageEventList
  = defineQueryOptions((id: number) => ({
    key: CHARACTER_STORAGE_QUERY_KEYS.characterStorageEventList(id),
    query: () => characterStorageService.getOptins(id),
  }),
  )
