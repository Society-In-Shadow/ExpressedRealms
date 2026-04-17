export interface Character {
  id: number | string
  name: string
  expression: string
  state: CharacterState
}

export enum CharacterState {
  Primary = 1,
  Retired = 2,
  Regular = 0,
}

export type CharacterList = Character[]
