export interface UserStateResponse {
  userInfo: UserInfo | null
}

export interface UserInfo {
  name: string | null
  email: string
  setupState: SetupState
}

export enum SetupState {
  UnconfirmedEmail = 1,
  SetProfileName,
  Done,
}
