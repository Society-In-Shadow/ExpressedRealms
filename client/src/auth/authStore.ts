import { defineQueryOptions } from '@pinia/colada'
import { authEndpoints } from '@/auth/authEndpoints.ts'

export const LOGIN_QUERY_KEYS = {
  root: ['userState'] as const,
  summary: ['login', 'list'] as const,
}

export const userInfoQuery = defineQueryOptions({
  key: LOGIN_QUERY_KEYS.root,
  query: authEndpoints.userState,
})
