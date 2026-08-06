export interface CharacterListItem {
  id: number
  name: string
  expressionName: string
  expressionSubTypeId: number | undefined
  isPrimaryCharacter: boolean
  isInCharacterCreate: boolean
  isRetired: boolean
}
