import { type InferType, object, string } from 'yup'
import { useGenericForm } from '@/utilities/formUtilities'
import type { EditRole } from '../types.ts'

const validationSchema = object({
  name: string()
    .required()
    .max(250)
    .label('Name'),
  description: string()
    .nullable()
    .optional()
    .label('Description'),
})

export type RoleForm = InferType<typeof validationSchema>

export function getValidationInstance() {
  const form = useGenericForm(validationSchema)

  const setValues = (item: EditRole) => {
    form.fields.name.field.value = item.name
    form.fields.description.field.value = item.description
  }

  return {
    ...form,
    setValues,
  }
}
