import { object, string } from 'yup'
import { type GenericForm, useGenericForm } from '@/utilities/formUtilities'
import type { AddExpressionFields } from '@/components/expressions/expression/types.ts'

const validationSchema = object({
  name: string()
    .required()
    .max(50)
    .label('Name'),
  shortDescription: string()
    .required()
    .max(125)
    .label('Short Description'),
  navMenuImage: string()
    .required()
    .label('Nav Menu Icon'),
}).transform(values => ({
  name: values.name,
  shortDescription: values.shortDescription,
  navMenuImage: values.navMenuImage,
} as AddExpressionFields))

export function getValidationInstance(): GenericForm<AddExpressionFields> {
  const form = useGenericForm(validationSchema)

  const setDefaultValues = (data) => {
    form.fields.navMenuImage.field.value = 'emergency_home'
  }

  return {
    ...form,
    setValues: setDefaultValues,
  }
}
