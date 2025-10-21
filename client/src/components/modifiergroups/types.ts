export enum SourceTableEnum {
  ProgressionLevels = 1,
  Blessings = 2,
  Powers = 3,
}

export interface CreateStatModifier {
  sourceTable: SourceTableEnum
  sourceId: number
  scaleWithLevel: boolean
  modifier: number
  creationSpecificBonus: boolean
  statModifierId: number
}

export interface StatModifiersResponse {
  modifiers: Array<StatModifierReturnModel>
}

export interface StatModifier {
  id: number
  name: string
}

export interface StatModifierGroup {
  groupId: number
  modifiers: Array<StatModifierReturnModel>
}

export interface StatModifierReturnModel {
  id: number
  statModifierId: number
  statModifier: StatModifier
  targetExpression: StatModifier
  targetExpressionId: number | null
  modifier: number
  scaleWithLevel: boolean
  creationSpecificBonus: boolean
}
