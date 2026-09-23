import type { DateTime } from 'luxon'

export const CheckinStage = {
  ShqApproval: 1,
  GoApproval: 2,
  CrbPrinted: 3,
  CrbReadForPickup: 4,
  CrbPickedUp: 5,
  Day2Checkin: 6,
  Day3Checkin: 7,
  AgeCheckApproval: 8,
  EventQuestionsCheck: 9,
  AssignedXpCheck: 10,
  CrbAssembled: 11,
  PlayerNeedsReapproval: 12,
  CharacterStorageQuestion: 13,
  PlayerEarlyCheckin: 14,
  FinalStage: 15,
  AwaitingCheckin: 16,
} as const

export type CheckinStage = typeof CheckinStage[keyof typeof CheckinStage]

export interface CheckinInfo {
  lookupId: string
  checkinStage: BasicInfo | null
  eventName: string
  sendPickupCrbEmail: boolean
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
  currentEventDay: number
}

export interface GetCheckinQuestionsResponse {
  hasCompletedStage: boolean
  questions: Array<Question>
}

export interface GetStonePullInfoResponse {
  hasCompletedStep: boolean
  isFirstTimeUser: boolean
  broughtFriend: boolean
  hasCharacterStorage: boolean
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

export interface GoChecksResponse {
  isLegacyExpression: boolean
  xpSpentPercentage: number
  dealWithDevil: boolean
  stillInCharacterCreation: boolean
  spentTooMuchXp: boolean
  knowledgeChecks: KnowledgeCheck[]
  contacts: ContactCheck[]
}

export interface KnowledgeCheck {
  id: number
  name: string
  isDoctorateLevel?: boolean
  isUnknownKnowledge?: boolean
  isReviewed: boolean
}

export interface ContactCheck {
  id: number
  name: string
  isReviewed: boolean
}

export interface KeyValue {
  key: number | string
  value: string | null
}

export interface Event {
  eventId?: number | string
  eventName?: string
  startDate?: string
  dueDate?: DateTime
}

export interface EarlyCheckinInfo {
  showBanner: boolean
  character: KeyValue | null
  event: Event | null
  nextStage: KeyValue | null
}
