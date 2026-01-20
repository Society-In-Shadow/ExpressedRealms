import { type InferType, mixed, object } from 'yup'
import { useGenericForm } from '@/utilities/formUtilities'
import type { ListItem } from '@/types/ListItem.ts'
import { DateTime } from 'luxon'

const validationSchema = object({
  expireDate: mixed<DateTime>()
    .test('is-valid', 'Invalid date', val => val?.isValid ?? false)
    .optional()
    .label('Expire Date'),
  user: object<ListItem>()
    .required()
    .label('User'),
})

export type AssignedUserForm = InferType<typeof validationSchema>

export function getValidationInstance() {
  const form = useGenericForm(validationSchema)

  return {
    ...form,
  }
}
