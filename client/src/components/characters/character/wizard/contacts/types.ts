import type ListItem from 'quill/formats/list'

export interface ContactListResponse {
  contacts: Contact[]
}

export interface Contact {
  name: string
  knowledge: string
  knowledgeLevel: string
  id: number
  usesPerWeek: number
  isApproved: boolean
}

export interface ContactListCharacterSheetResponse {
  contacts: ContactCharacterSheet[]
}
export interface ContactCharacterSheet {
  name: string
  knowledge: string
  knowledgeLevel: string
  id: number
  usesPerWeek: number
  isApproved: boolean
  notes: string | null
  knowledgeDescription: string
}

export interface ContactKnowledgeLevels {
  name: string
  levelId: number
  cost: number
  isDisabled: boolean
}

export interface ContactFrequency {
  frequency: number
  cost: number
  isDisabled: boolean
}

export interface EditContactResponse {
  name: string
  knowledgeId: number
  knowledgeLevelId: number
  id: number
  notes: string | null
  usesPerWeek: number
  isApproved: boolean
}

export interface EditContact {
  name: string
  knowledge: ListItem
  knowledgeLevel: ContactKnowledgeLevels
  id: number
  notes: string | null
  usesPerWeek: ContactFrequency
  isApproved: boolean
}
