export interface FactionResponse {
  factions: Array<Faction>
}

export interface Faction {
  id: number
  name: string
  background: string
}

export interface EditSingleFactionInfo {
  name: string
  background: string
}
