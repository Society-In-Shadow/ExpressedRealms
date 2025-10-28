import type { ListItem } from '@/types/ListItem'

export interface EventResponse {
  events: Array<Event>
}

export interface Event {
  id: number
  name: string
  startDate: string // ISO date string (e.g. "2025-10-28")
  endDate: string // same as above
  location: string
  websiteName: string
  websiteUrl: string
  additionalNotes: string
  timeZoneId: string
  conExperience: number
  isPublished: boolean
}

export interface EditEventRequest {
  id: number
  name: string
  description: string
  typeId: number
}

export interface EditEvent {
  id: number
  name: string
  startDate: string // ISO date string (e.g. "2025-10-28")
  endDate: string // same as above
  location: string
  websiteName: string
  websiteUrl: string
  additionalNotes: string
  timeZoneId: string
  conExperience: number
  isPublished: boolean
  timeZone: ListItem
}
