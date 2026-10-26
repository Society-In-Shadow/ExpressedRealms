import type { ListItem } from '@/types/ListItem.ts'

export interface EditExpressionSectionTypeFormFields {
  name: string
  content: string
  sectionType: ListItem
}

export interface EditExpressionSection {
  name: string
  content: string
  sectionType: ListItem
}

export interface EditSectionType {
  id: number
  isHeaderSection: boolean
}
