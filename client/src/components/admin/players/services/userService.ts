import axios from 'axios'
import type { BasicUserInfoResponse, EditUserRequest } from '@/components/admin/players/types.ts'

export const userService = {
  getUserInfo: (userId: string): Promise<BasicUserInfoResponse> => axios.get<BasicUserInfoResponse>(`/admin/user/${userId}`)
    .then(async (response) => { return response.data }),
  edit: (userId: string, request: EditUserRequest) => axios.put(`/admin/user/${userId}`, request)
    .then(async (response) => { return response.data }),
}
