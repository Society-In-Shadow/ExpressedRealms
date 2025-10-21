import {number, object, string} from 'yup'
import {useGenericForm} from '@/utilities/formUtilities'
import type {BlessingLevel} from '@/components/blessings/types.ts'

const validationSchema = object({
  level: string()
    .required()
    .max(250)
    .label('Level'),
  description: string()
    .required()
    .label('Description'),
  xpCost: number()
    .required()
    .label('XP Cost'),
  xpGain: number()
    .required()
    .label('XP Gain'),
})

export function getValidationInstance() {
  const form = useGenericForm(validationSchema)

  const setValues = (model: BlessingLevel) => {
    form.fields.level.field.value = model.level
    form.fields.description.field.value = model.description
    form.fields.xpCost.field.value = model.xpCost
    form.fields.xpGain.field.value = model.xpGain
  }

  const customResetForm = () => {
    form.handleReset()
  }

  return {
    handleSubmit: form.handleSubmit,
    customResetForm,
    setValues,
    setErrors: form.setErrors,
    fields: form.fields,
  }
}
