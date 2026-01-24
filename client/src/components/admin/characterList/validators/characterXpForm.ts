import { type InferType, number, object } from 'yup'
import { useGenericForm } from '@/utilities/formUtilities'

const validationSchema = object({
  playerNumber: number()
    .max(999)
    .required()
    .label('Player Number'),
})

export type CharacterXpForm = InferType<typeof validationSchema>

export function getValidationInstance() {
  const form = useGenericForm(validationSchema)

  const setValues = (playerNumber: number) => {
    form.fields.playerNumber.field.value = playerNumber
  }

  return {
    ...form,
    setValues,
  }
}
