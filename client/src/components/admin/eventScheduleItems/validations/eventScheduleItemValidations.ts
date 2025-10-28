import { date, type InferType, object, string } from 'yup'
import { useGenericForm } from '@/utilities/formUtilities'
import type { EditEventScheduleItem } from '@/components/admin/eventScheduleItems/types.ts'

const validationSchema = object({
  description: string()
    .required()
    .max(250)
    .label('Description'),
  startTime: string()
    .required()
    .label('Start Time'),
  endTime: string()
    .required()
    .label('End Time'),
  date: date()
    .required()
    .label('Day'),

})

export type EventScheduleItemForm = InferType<typeof validationSchema>

export function getValidationInstance() {
  const form = useGenericForm(validationSchema)

  const setValues = (item: EditEventScheduleItem) => {
    form.fields.description.field.value = item.description
    form.fields.startTime.field.value = item.startTime
    form.fields.endTime.field.value = item.endTime
    form.fields.date.field.value = item.date
  }

  return {
    ...form,
    setValues,
  }
}
