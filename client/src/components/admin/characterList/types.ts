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
}
