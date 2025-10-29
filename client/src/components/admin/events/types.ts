import type { ListItem } from '@/types/ListItem'
import { DateTime } from 'luxon'

export interface EventResponse {
  events: Array<Event>
}

export interface Event {
  id: number
  name: string
  startDate: DateTime
  endDate: DateTime
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
  startDate: DateTime
  endDate: DateTime
  location: string
  websiteName: string
  websiteUrl: string
  additionalNotes: string
  timeZoneId: string
  conExperience: number
  isPublished: boolean
  timeZone: ListItem
}
