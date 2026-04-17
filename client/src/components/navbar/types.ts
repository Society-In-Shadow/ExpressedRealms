export interface Character {
  id: number | string
  name: string
  expression: string
  isPrimaryCharacter: boolean
}

export type CharacterList = Character[]
