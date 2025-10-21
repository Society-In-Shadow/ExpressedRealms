import {object, string} from 'yup'
import {useGenericForm} from '@/utilities/formUtilities'
import type {ProgressionPath} from '@/components/expressions/progressionPaths/types.ts'

export function getValidationInstance() {
  const validationSchema = object({
    name: string()
      .required()
      .max(250)
      .label('Name'),
    description: string()
      .required()
      .max(5000)
      .label('Description'),
  })

  const form = useGenericForm(validationSchema)

  const setValues = (power: ProgressionPath) => {
    form.fields.name.field.value = power.name
    form.fields.description.field.value = power.description
  }

  const customResetForm = () => {
    form.fields.description.field.value = ''
    form.handleReset()
  }

  return {
    handleSubmit: form.handleSubmit,
    customResetForm,
    setValues,
    name: form.fields.name,
    description: form.fields.description,
  }
}
