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
