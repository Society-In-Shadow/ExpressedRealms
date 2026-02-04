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
