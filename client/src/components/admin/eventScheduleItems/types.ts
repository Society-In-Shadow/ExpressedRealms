import type { DateTime } from 'luxon'

export interface EventScheduleItemResponse {
  eventScheduleItems: Array<EventScheduleItem>
}

export interface EventScheduleItem {
  id: number
  eventId: number
  description: string
  date: DateTime
  startTime: DateTime
  endTime: DateTime
}

export interface EditEventScheduleItem {
  id: number
  eventId: number
  description: string
  date: DateTime
  startTime: DateTime
  endTime: DateTime
}
