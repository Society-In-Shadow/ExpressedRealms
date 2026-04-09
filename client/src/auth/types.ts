export interface UserStateResponse {
  userInfo: UserInfo | null
}

export interface UserInfo {
  name: string | null
  email: string
  setupStep: SetupState
}

export enum SetupState {
  UnconfirmedEmail,
  SetProfileName,
  Done,
}
