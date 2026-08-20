import { boolean, type InferType, number, object, string } from 'yup'
import { useGenericForm } from '@/utilities/formUtilities'
import type { ListItem } from '@/types/ListItem'
import type { ExpressionInfo, StatModifierReturnModel } from '@/components/modifiergroups/types.ts'

const validationSchema = object({
  modifierType: object<ListItem>().nullable()
    .required()
    .label('Modifier Type'),
  targetExpression: object<ExpressionInfo>().nullable()
    .label('Target Expression'),
  targetProgressionPath: object<ListItem>().nullable()
    .label('Target Path'),
  modifier: number().required()
    .label('Modifier'),
  creationSpecificBonus: boolean()
    .label('Include Level 0 With Scale'),
  scaleWithLevel: boolean()
    .label('Scale With Level'),
  notes: string()
    .max(1000)
    .label('Notes'),
})

export type ModifierForm = InferType<typeof validationSchema>

export function getValidationInstance() {
  const form = useGenericForm(validationSchema)

  const setValues = (model: StatModifierReturnModel) => {
    form.fields.modifier.field.value = model.modifier
    form.fields.creationSpecificBonus.field.value = model.creationSpecificBonus
    form.fields.scaleWithLevel.field.value = model.scaleWithLevel
    form.fields.modifierType.field.value = model.statModifier
    form.fields.targetExpression.field.value = model.targetExpression
    form.fields.targetProgressionPath.field.value = model.targetProgressionPath
    form.fields.notes.field.value = model.notes
  }

  const customResetForm = () => {
    form.fields.modifier.field.value = 0
    form.fields.creationSpecificBonus.field.value = false
    form.fields.scaleWithLevel.field.value = false
    form.fields.modifierType.field.value = null
    form.handleReset()
  }

  return {
    handleSubmit: form.handleSubmit,
    customResetForm,
    setValues,
    fields: form.fields,
  }
}
