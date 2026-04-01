export interface ArchetypesResponse {
  archetypes: Array<Archetype>
}

export interface Archetype {
  id: number
  name: string
  description: string | null
  expressionName: string
}
