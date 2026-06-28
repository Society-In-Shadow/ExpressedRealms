import { object, string } from 'yup'
import { type GenericForm, useGenericForm } from '@/utilities/formUtilities'
import type { EditSingleFactionInfo } from '@/components/expressions/factions/types.ts'

const validationSchema = object({
  name: string()
    .required()
    .max(250)
    .label('Name'),
  background: string()
    .required()
    .max(20_000)
    .label('Background'),
}).transform(values => ({
  name: values.name,
  background: values.background,
} as EditSingleFactionInfo))

export function getValidationInstance(): GenericForm<EditSingleFactionInfo> {
  const form = useGenericForm(validationSchema)

  const setValues = (data: EditSingleFactionInfo) => {
    form.fields.name.field.value = data.name
    form.fields.background.field.value = data.background
  }

  return {
    ...form,
    setValues,
  }
}
