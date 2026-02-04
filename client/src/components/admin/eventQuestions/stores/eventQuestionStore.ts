import { defineStore } from 'pinia'
import axios from 'axios'

import toaster from '@/services/Toasters'
import type { EventQuestionResponse, Question } from '@/components/admin/eventQuestions/types.ts'
import type { ListItem } from '@/types/ListItem.ts'
import type { EventQuestionForm } from '@/components/admin/eventQuestions/validations/eventQuestionValidation.ts'

export const EventQuestionStore
  = defineStore(`EventQuestion`, {
    state: () => {
      return {
        questions: {} as Question[],
        questionTypes: [] as ListItem[],
      }
    },
    actions: {
      async getItems(eventId: number) {
        this.questionTypes = [{
          id: 2,
          name: 'Badge Info',
          description: '',
        },
        {
          id: 3,
          name: 'Text Field',
          description: '',
        },
        {
          id: 4,
          name: 'True False',
          description: '',
        }]
        const response = await axios.get<EventQuestionResponse>(`/events/${eventId}/questions/`)
        for (const item of response.data.questions) {
          item.questionType = this.questionTypes.find(type => type.id === item.questionTypeId)
        }
        this.questions = response.data.questions
      },
      updateItem: async function (eventId: number, values: EventQuestionForm, id: number): Promise<void> {
        await axios.put(`/events/${eventId}/questions/${id}`, {
          question: values.question,
        })
          .then(async () => {
            await this.getItems(eventId)
            toaster.success('Successfully Updated Question!')
          })
      },
      addItem: async function (eventId: number, values: EventQuestionForm): Promise<void> {
        await axios.post(`/events/${eventId}/questions/`, {
          question: values.question,
          questionTypeId: values.questionType.id,
        })
          .then(async () => {
            await this.getItems(eventId)
            toaster.success('Successfully Added Question!')
          })
      },
      deleteItem: async function (eventId: number, id: number) {
        await axios.delete(`/events/${eventId}/questions/${id}`)
          .then(async () => {
            await this.getItems(eventId)
            toaster.success(`Successfully Deleted Question!`)
          })
      },
    },
  })
