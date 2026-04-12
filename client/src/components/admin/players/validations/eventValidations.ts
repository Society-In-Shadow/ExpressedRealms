import { type InferType, number, object } from 'yup'
import { useGenericForm } from '@/utilities/formUtilities'
import type { BasicUserInfoResponse } from '@/components/admin/players/types.ts'

const validationSchema = object({
  playerNumber: number()
    .required()
    .label('Player Number'),
})

export type EventForm = InferType<typeof validationSchema>

export function getValidationInstance() {
  const form = useGenericForm(validationSchema)

  const setValues = (item: BasicUserInfoResponse) => {
    form.fields.playerNumber.field.value = item.playerNumber
  }

  return {
    ...form,
    setValues,
  }
}
