export interface CharacterListResponse {
  characters: PrimaryCharacter[]
}

export interface PrimaryCharacter {
  id: number
  name: string
  background?: string
  expression: string
  playerName: string
  playerNumber: number
}
