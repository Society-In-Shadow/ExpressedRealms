import type { ExpressionMenuItem } from '@/components/navbar/navMenuItems/types.ts'
import type { ListItem } from '@/types/ListItem'

export interface AddExpressionFields {
  name: string
  shortDescription: string
  navMenuImage: string
}

export interface AddExpressionPost {
  name: string
  shortDescription: string
  navMenuImage: string
  expressionTypeId: number
}

export interface CopyExpressionFields {
  name: string
}

export interface CopyExpressionPost {
  name: string
}

export interface EditExpressionFields {
  name: string
  shortDescription: string
  navMenuImage: string
  publishStatus: ListItem
  sortOrder: number
}

export interface EditExpressionPut {
  id: number
  name: string
  shortDescription: string
  navMenuImage: string
  publishStatus: number
  sortOrder: number
}

export interface EditItem {
  id: number
  name: string
  shortDescription: string
  navMenuImage: string
  publishStatus: number
  publishTypes: ListItem[]
  sortOrder: number
}

export interface CmsSections {
  rulebookItems: ExpressionMenuItem[]
  worldBackgroundItems: ExpressionMenuItem[]
  expressionItems: ExpressionMenuItem[]
}
