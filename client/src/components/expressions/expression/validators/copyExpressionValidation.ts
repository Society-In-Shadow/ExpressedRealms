import { object, string } from 'yup'
import { type GenericForm, useGenericForm } from '@/utilities/formUtilities'
import type { CopyExpressionFields } from '@/components/expressions/expression/types.ts'

const validationSchema = object({
  name: string()
    .required()
    .max(50)
    .label('Name'),
}).transform(values => ({
  name: values.name,
} as CopyExpressionFields))

export function getValidationInstance(): GenericForm<CopyExpressionFields> {
  const form = useGenericForm(validationSchema)

  return {
    ...form,
  }
}
