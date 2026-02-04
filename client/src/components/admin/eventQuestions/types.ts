import type { DateTime } from 'luxon'
import type { ListItem } from '@/types/ListItem.ts'

export interface EventQuestionResponse {
  questions: Array<Question>
}

export interface Question {
  id: number
  question: string
  questionTypeId: number
  questionType: ListItem | null
}

export interface EditEventScheduleItem {
  id: number
  eventId: number
  description: string
  date: DateTime
  startTime: DateTime
  endTime: DateTime
}
