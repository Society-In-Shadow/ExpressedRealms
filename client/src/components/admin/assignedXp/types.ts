import { DateTime } from 'luxon'
import type { ListItem } from '@/types/ListItem.ts'

export interface AssignedXpResponse {
  assignedXpInfo: Array<AssignedXpInfo>
}

export interface AssignedXpInfo {
  id: number
  event: ListItem
  character: ListItem
  xpType: ListItem
  assigner: ListItem
  player: ListItem
  amount: number
  notes?: string | null
  dateAssigned: DateTime
}

export interface EditEventRequest {
  id: number
  name: string
  description: string
  typeId: number
}

export interface EditAssignedXpItem {
  id: number
  event: ListItem
  character: ListItem
  xpType: ListItem
  assigner: ListItem
  player: ListItem
  amount: number
  notes?: string | null
  dateAssigned: DateTime
}
