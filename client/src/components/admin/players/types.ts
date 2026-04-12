import type { DateTime } from 'luxon'

export interface Log {
  id: number
  location: string
  timeStamp: Date
  action: string
  changedProperties: string
  changedPropertiesList: ChangedProperty[]
}

export interface ChangedProperty {
  id: number
  propertyName: string
  oldValue?: string | null
  newValue?: string | null
  friendlyName?: string | null
  message?: string | null
}

export interface PlayerListItem {
  id: string
  username: string
  email: string
  roles: AssignedRoleInfo[]
  legacyRoles: string[]
  emailConfirmed: boolean
  isDisabled: boolean
  lockedOut: boolean
  lockedOutExpires: Date
}

export interface AssignedRoleInfo {
  name: string
  expirationDate: DateTime | null
}

export interface RoleInfo {
  name: string
  isEnabled: boolean
}

export interface BasicUserInfoResponse {
  playerNumber: number
}

export interface EditUserRequest {
  playerNumber: number
}
