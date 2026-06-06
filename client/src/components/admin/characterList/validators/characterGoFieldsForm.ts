import { type InferType, number, object } from 'yup'
import { useGenericForm } from '@/utilities/formUtilities'
import type { GoFields } from '@/components/admin/characterList/types.ts'

const validationSchema = object({
  wealthLevel: number()
    .required()
    .label('Wealth Level'),
  voidFragments: number()
    .required()
    .label('Void Fragments'),
  motes: number()
    .required()
    .label('Motes'),
  primaFragments: number()
    .required()
    .label('Prima Fragments'),
})

export type CharacterGoFieldsForm = InferType<typeof validationSchema>

export function getValidationInstance() {
  const form = useGenericForm(validationSchema)

  const setValues = (data: GoFields) => {
    form.fields.wealthLevel.field.value = data.wealthLevel
    form.fields.motes.field.value = data.motes
    form.fields.primaFragments.field.value = data.primaFragments
    form.fields.voidFragments.field.value = data.voidFragments
  }

  return {
    ...form,
    setValues,
  }
}
