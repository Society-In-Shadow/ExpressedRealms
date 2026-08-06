import type { ExpressionSubTypes } from '@/components/expressions/expressionSubTypes'

export interface ExpressionMenuItem {
  id: number
  name: string
  shortDescription: string
  navMenuImage: string
  statusName: string | null
  statusId: number
  slug: string
  expressionTypeId: number
  expressionSubTypeId: ExpressionSubTypes
  orderIndex: number
}

export interface ExpressionMenuResponse {
  menuItems: Array<ExpressionMenuItem>
  canEdit: boolean
}

export interface SimpleMenuItem {
  label: string
  description: string
  navMenuIcon: string
  pushComponentRouteName: string
}
