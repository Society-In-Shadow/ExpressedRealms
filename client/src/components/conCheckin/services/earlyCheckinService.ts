import axios from 'axios'
import type { EarlyCheckinInfo } from '@/components/conCheckin/types.ts'
import { defineQueryOptions, useMutation, useQueryCache } from '@pinia/colada'
import { DateTime } from 'luxon'
import toaster from '@/services/Toasters.ts'

const earlyCheckinService = {
  getCheckinInfo: (): Promise<EarlyCheckinInfo> => axios.get<EarlyCheckinInfo>(`/events/checkin/earlyCheckin`)
    .then(async (response) => {
      response.data.event.dueDate = DateTime.fromISO(`${response.data.event.startDate}`)
      return response.data
    }),
  requestApproval: () => axios.post('/events/checkin/earlyCheckin/requestApproval')
    .then((response) => { return response.data }),
}

export const EARLY_CHECKIN_QUERY_KEYS = {
  root: ['earlyCheckin'] as const,
  dialogState: ['earlyCheckin', 'dialogState'] as const,
}

export const earlyCheckinQuery = defineQueryOptions({
  key: EARLY_CHECKIN_QUERY_KEYS.dialogState,
  query: earlyCheckinService.getCheckinInfo,
})

export const requestGoApproval = () => {
  const queryCache = useQueryCache()

  return useMutation({
    mutation: () => earlyCheckinService.requestApproval(),
    async onSuccess() {
      toaster.success('Successfully Requested Approval')
      await queryCache.invalidateQueries({ key: EARLY_CHECKIN_QUERY_KEYS.dialogState })
    },
  })
}
