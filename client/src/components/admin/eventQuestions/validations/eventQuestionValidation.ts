import { type InferType, object, string } from 'yup'
import { useGenericForm } from '@/utilities/formUtilities'
import type { ListItem } from '@/types/ListItem.ts'
import type { Question } from '@/components/admin/eventQuestions/types.ts'

const validationSchema = object({
  question: string()
    .required()
    .max(500)
    .label('Question'),
  questionType: object<ListItem>()
    .required()
    .label('Question Type'),
})

export type EventQuestionForm = InferType<typeof validationSchema>

export function getValidationInstance() {
  const form = useGenericForm(validationSchema)

  const setValues = (item: Question) => {
    form.fields.question.field.value = item.question
    form.fields.questionType.field.value = item.questionType
  }

  return {
    ...form,
    setValues,
  }
}
