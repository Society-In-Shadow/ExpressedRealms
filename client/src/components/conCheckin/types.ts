export const CheckinStage = {
  ShqApproval: 1,
  GoApproval: 2,
  CrbCreation: 3,
  CrbReadForPickup: 4,
  CrbPickedUp: 5,
  Day2Checkin: 6,
  Day3Checkin: 7,
  AgeCheckApproval: 8,
  EventQuestionsCheck: 9,
  AssignedXpCheck: 10,
  PrintedCrb: 11,
  PlayerNeedsReapproval: 12,
} as const

export type CheckinStage = typeof CheckinStage[keyof typeof CheckinStage]

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
  playerNumber: number
  questions: Array<Question>
  primaryCharacterInfo: PrimaryCharacterInfo | null
  currentStage: BasicInfo | null
}

export interface GetCheckinQuestionsResponse {
  hasCompletedStage: boolean
  questions: Array<Question>
}

export interface GetStonePullInfoResponse {
  hasCompletedStep: boolean
  isFirstTimeUser: boolean
  assignedXp: AssignedXpType
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

export interface GetBreakOfDawnInfoResponse {
  vitality: number
  health: number
  blood: number
  rwp: number
  psyche: number
  mortis: number
  characterLevel: number
  expressionId: number
}
