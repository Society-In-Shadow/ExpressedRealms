export interface CharacterListResponse {
  characters: PrimaryCharacter[]
}

export interface PrimaryCharacter {
  id: number
  name: string
  playerStageId?: number | null
  expression: string
  playerName: string
  playerNumber: number
  hasPromotionRequest: boolean
}

export interface GoFields {
  wealthLevel: number
  voidFragments: number
  motes: number
  primaFragments: number
  statModifierGroupId: number | undefined
}
