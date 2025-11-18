import { type InferType, number, object, string } from 'yup'
import { useGenericForm } from '@/utilities/formUtilities'
import type { ListItem } from '@/types/ListItem'
import type { EditAssignedXpItem } from '@/components/admin/assignedXp/types.ts'

const validationSchema = object({
  notes: string()
    .nullable()
    .optional()
    .label('Notes'),
  amount: number()
    .required()
    .label('Amount'),
  event: object<ListItem>()
    .required()
    .label('Event'),
  xpType: object<ListItem>()
    .required()
    .label('Xp Type'),
})

export type AssignedXpForm = InferType<typeof validationSchema>

export function getValidationInstance() {
  const form = useGenericForm(validationSchema)

  const setValues = (item: EditAssignedXpItem) => {
    form.fields.notes.field.value = item.notes
    form.fields.amount.field.value = item.amount
    form.fields.event.field.value = item.event
    form.fields.xpType.field.value = item.xpType
  }

  return {
    ...form,
    setValues,
  }
}
