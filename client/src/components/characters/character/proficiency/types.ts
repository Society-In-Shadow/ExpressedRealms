export interface ProficienciesDto {
  name: string
  appliedModifiers: ModifierDescription[]
  value: number
  id: number
}

export interface ModifierDescription {
  value: number
  name: string
}

export interface ProficienciesResponse {
  offensive: ProficienciesDto[]
  defensive: ProficienciesDto[]
  secondary: ProficienciesDto[]
}
