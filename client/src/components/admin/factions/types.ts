export interface ExpressionListDto {
  expressions?: ExpressionDto[]
}

export interface ExpressionDto {
  id: number
  name: string
  factions?: FactionDto[]
}

export interface FactionDto {
  id: number
  name: string
  players?: PlayerDto[]
}

export interface PlayerDto {
  id?: number
  level?: number
  levelName: string
  characterName: string
  player: string
}
