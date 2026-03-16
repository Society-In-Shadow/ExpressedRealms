export interface CheckinInfo {
  lookupId: string
  checkinStage: BasicInfo | null
  event: ActiveEvent
}

export interface GoCheckinInfo {
  wasFound: boolean
  userName: string | null
}

export interface ApproveCheckinInfo {
  isFirstTimeUser: boolean
  playerNumber: number
  assignedXp: AssignedXpType
  questions: Array<Question>
  primaryCharacterInfo: PrimaryCharacterInfo | null
  currentStage: BasicInfo | null
}

export interface GetCheckinQuestionsResponse {
  hasCompletedStage: boolean
  questions: Array<Question>
}

export interface Question {
  id: number
  question: string
  response: string | null | undefined
  typeId: number
}

export interface PrimaryCharacterInfo {
  characterId: number
  characterName: string
}

export interface AssignedXpType {
  amount: number
  typeId: number
  typeName: string
}

export interface BasicInfo {
  id: number
  name: string
}

export interface ActiveEvent {
  id: number
  name: string
}

export interface AgeInfo {
  ageGroupId: AgeGroupId | null
  hasBeenVerified: boolean
}

export const AgeGroupId = {
  Child: 1,
  Teen: 2,
  Adult: 3,
}
