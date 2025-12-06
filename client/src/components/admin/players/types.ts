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
  roles: string[]
  emailConfirmed: boolean
  isDisabled: boolean
  lockedOut: boolean
  lockedOutExpires: Date
}

export interface RoleInfo {
  name: string
  isEnabled: boolean
}
