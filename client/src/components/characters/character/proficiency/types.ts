interface ProficienciesDto {
  name: string
  description: string
  appliedModifiers: ModifierDescription[]
  value: number
  id: number
  type: string
}

interface ModifierDescription {
  value: number
  name: string
}
