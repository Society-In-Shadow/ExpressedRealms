import axios from 'axios'
import type { CharacterStorageOptinsResponse } from '@/components/admin/events/types.ts'
import { DateTime } from 'luxon'

export const characterStorageService = {
  getOptins: (id: number): Promise<CharacterStorageOptinsResponse> => axios.get<CharacterStorageOptinsResponse>(`/events/${id}/characterStorageOptins`)
    .then(response => ({
      ...response.data,
      characterStorageOptins: response.data.characterStorageOptins.map(
        optin => ({
          ...optin,
          timestampAssigned: DateTime.fromISO(`${optin.timestamp}`),
        }),
      ),
    })),
}
