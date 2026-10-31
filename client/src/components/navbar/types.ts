export interface Character {
  id: number | string
  name: string
  expression: string
  expressionSubTypeId: number | undefined
  state: CharacterState
}

export enum CharacterState {
  Primary = 1,
  Retired = 2,
  Regular = 0,
}

export interface SearchResult {
  entryId: number
  sectionTypeId: number
  section: string
  slug: string
  NavName: string
  sectionName: string
  sectionContent: string
}

export type CharacterList = Character[]
