export interface CheckinInfo {
  lookupId: string
  checkinStageId: number
  eventId: number
}

export interface GoCheckinInfo {
  userName: string
  isFirstTimeUser: boolean
}

export interface ApproveCheckinInfo {
  playerName: string
  isFirstTimeUser: boolean
  checkinId: number
  playerNumber: number
  assignedXp: number
  questions: Array<Question>
  primaryCharacterInfo: PrimaryCharacterInfo | null
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
