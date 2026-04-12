import { defineQueryOptions } from '@pinia/colada'
import { userService } from '@/components/admin/players/services/userService.ts'

export const USER_QUERY_KEYS = {
  root: ['users'] as const,
  basicInfoById: (id: string) => ['users', 'basicInfo', id] as const,
}

export const basicUserInfoById
  = defineQueryOptions((id: string) => ({
    key: USER_QUERY_KEYS.basicInfoById(id),
    query: () => userService.getUserInfo(id),
  }),
  )
