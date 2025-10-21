interface ProficienciesDto {
  name: string
  description: string
  modifiers: number[]
  appliedModifiers: ModifierDescription[]
  correspondingId: number // byte maps to number in TypeScript
  value: number
  maxValue: number
  id: number
  type: string
}

interface ModifierDescription {
  message: string
  value: number
  type: number
  name: string
}
