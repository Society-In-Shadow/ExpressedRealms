import type { DateTime } from 'luxon'

export interface UserRole {
  userId: string
  name: string
  /**
     * ISO date string (YYYY-MM-DD) or null
     */
  expireDate?: DateTime | null
}

export interface UserRolesResponse {
  roles: UserRole[]
}
