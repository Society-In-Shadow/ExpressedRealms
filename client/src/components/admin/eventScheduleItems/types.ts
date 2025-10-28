export interface EventScheduleItemResponse {
  eventScheduleItems: Array<EventScheduleItem>
}

export interface EventScheduleItem {
  id: number
  eventId: number
  description: string
  date: string
  startTime: Date
  endTime: Date
}

export interface EditEventScheduleItemRequest {
  id: number
  name: string
  description: string
  typeId: number
}

export interface EditEventScheduleItem {
  id: number
  eventId: number
  description: string
  date: string
  startTime: string
  endTime: string
}
