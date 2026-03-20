import { type InferType, number, object } from 'yup'
import { useGenericForm } from '@/utilities/formUtilities'
import type { ListItem } from '@/types/ListItem.ts'

const validationSchema = object({
  expression: object<ListItem>()
    .required()
    .label('Expression'),
  xpLevel: number()
    .required()
    .min(0)
    .max(8)
    .label('XP Level'),
  blood: number()
    .required()
    .min(0)
    .default(0)
    .label('Blood'),
  health: number()
    .required()
    .min(0)
    .default(0)
    .label('Health'),
  vitality: number()
    .required()
    .min(0)
    .default(0)
    .label('Vitality'),
  rwp: number()
    .required()
    .min(0)
    .default(0)
    .label('RWP'),
  psyche: number()
    .required()
    .min(0)
    .default(0)
    .label('Psyche'),
  mortis: number()
    .required()
    .min(0)
    .default(0)
    .label('Mortis'),
})

export type BreakOfDawnForm = InferType<typeof validationSchema>

export function getValidationInstance() {
  const form = useGenericForm(validationSchema)

  form.fields.xpLevel.field.value = 0
  form.fields.psyche.field.value = 0
  form.fields.blood.field.value = 0
  form.fields.health.field.value = 0
  form.fields.vitality.field.value = 0
  form.fields.rwp.field.value = 0
  form.fields.mortis.field.value = 0

  return {
    ...form,
  }
}
