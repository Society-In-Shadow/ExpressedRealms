import axios from 'axios'
import type { EarlyCheckinInfo } from '@/components/conCheckin/types.ts'
import { defineQueryOptions } from '@pinia/colada'
import { DateTime } from 'luxon'

export const earlyCheckinService = {
  getCheckinInfo: (): Promise<EarlyCheckinInfo> => axios.get<EarlyCheckinInfo>(`/events/checkin/earlyCheckin`)
    .then(async (response) => {
      response.data.event.dueDate = DateTime.fromISO(`${response.data.event.startDate}`)
      return response.data
    }),
}

export const EARLY_CHECKIN_QUERY_KEYS = {
  root: ['earlyCheckin'] as const,
  dialogState: ['earlyCheckin', 'dialogState'] as const,
}

export const earlyCheckinQuery = defineQueryOptions({
  key: EARLY_CHECKIN_QUERY_KEYS.dialogState,
  query: earlyCheckinService.getCheckinInfo,
})
