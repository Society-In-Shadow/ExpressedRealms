import {type InferType, object, string} from 'yup'
import {useGenericForm} from '@/utilities/formUtilities'
import type {Specialization} from '@/components/characters/character/knowledges/types'

const validationSchema = object({
  name: string()
    .required()
    .max(250)
    .label('Name'),
  description: string()
    .required()
    .max(5000)
    .label('Description'),
  notes: string().nullable()
    .max(10000)
    .label('Notes'),
})

export type SpecializationForm = InferType<typeof validationSchema>

export function getValidationInstance() {
  const form = useGenericForm(validationSchema)

  const setValues = (model: Specialization) => {
    form.fields.name.field.value = model.name
    form.fields.description.field.value = model.description
    form.fields.notes.field.value = model.notes
  }

  const customResetForm = () => {
    form.fields.name.field.value = ''
    form.fields.description.field.value = ''
    form.fields.notes.field.value = ''
    form.handleReset()
  }

  return {
    handleSubmit: form.handleSubmit,
    customResetForm,
    setValues,
    name: form.fields.name,
    description: form.fields.description,
    notes: form.fields.notes,
  }
}
