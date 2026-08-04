import type { ExpressionMenuItem } from '@/components/navbar/navMenuItems/types.ts'

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

export interface CmsSections {
  rulebookItems: ExpressionMenuItem[]
  worldBackgroundItems: ExpressionMenuItem[]
  expressionItems: ExpressionMenuItem[]
}
