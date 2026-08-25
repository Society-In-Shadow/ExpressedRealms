import axios from 'axios'
import type { ExpressionListDto } from '@/components/admin/factions/types.ts'

export const adminFactionService = {
  getFactionParticipants: (): Promise<ExpressionListDto> => axios.get<ExpressionListDto>(`/factions/participants`)
    .then(async (response) => { return response.data }),
}
