import { type InferType, mixed, number, object, string } from 'yup'
import { useGenericForm } from '@/utilities/formUtilities'
import type { ListItem } from '@/types/ListItem'
import type { EditEvent } from '@/components/admin/events/types.ts'
import type { DateTime } from 'luxon'

const validationSchema = object({
  name: string()
    .required()
    .max(250)
    .label('Name'),
  startDate: mixed<DateTime>()
    .test('is-valid', 'Invalid date', val => val?.isValid ?? false)
    .required()
    .label('Start Date'),
  endDate: mixed<DateTime>()
    .test('is-valid', 'Invalid date', val => val?.isValid ?? false)
    .required()
    .label('End Date'),
  location: string()
    .required()
    .label('Location'),
  websiteName: string()
    .required()
    .label('Website Name'),
  websiteUrl: string()
    .required()
    .label('Website Url'),
  additionalNotes: string()
    .nullable()
    .optional()
    .label('Additional Notes'),
  conExperience: number()
    .required()
    .label('Con Experience'),
  timeZone: object<ListItem>()
    .required()
    .label('Time Zone'),
})

export type EventForm = InferType<typeof validationSchema>

export function getValidationInstance() {
  const form = useGenericForm(validationSchema)

  const setValues = (item: EditEvent) => {
    form.fields.name.field.value = item.name
    form.fields.startDate.field.value = item.startDate
    form.fields.endDate.field.value = item.endDate
    form.fields.location.field.value = item.location
    form.fields.websiteName.field.value = item.websiteName
    form.fields.websiteUrl.field.value = item.websiteUrl
    form.fields.additionalNotes.field.value = item.additionalNotes
    form.fields.conExperience.field.value = item.conExperience
    form.fields.timeZone.field.value = item.timeZone
  }

  return {
    ...form,
    setValues,
  }
}
