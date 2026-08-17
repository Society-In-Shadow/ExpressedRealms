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

export interface QuestionResponse {
  questionId: number | string
  playerName: string
  approver: string
  answer: string
  question: string
  approvalDate?: string
}

export interface QuestionResponses {
  responses: QuestionResponse[]
}
