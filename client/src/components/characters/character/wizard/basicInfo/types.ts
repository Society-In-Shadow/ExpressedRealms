export interface HighLevelExpressionInfoResponse {
  name: string
  description: string
  archetypes: string
  background: string
}

export interface ProgressionPath {
  id: number
  name: string
  description: string
}
export interface Archetype {
  id: number
  name: string
  background?: string | null
}

export interface ArchetypesResponse {
  archetypes: Archetype[]
}
