import axios from 'axios'
import type { UserStateResponse } from '@/auth/types.ts'

export const authEndpoints = {
  userState: (): Promise<UserStateResponse> => axios.get(`/auth/user`)
    .then(async (response) => { return response.data }),
}
