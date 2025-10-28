import { date, type InferType, number, object, string } from 'yup'
import { useGenericForm } from '@/utilities/formUtilities'
import type { ListItem } from '@/types/ListItem'
import type { EditEvent } from '@/components/admin/events/types.ts'

const validationSchema = object({
  name: string()
    .required()
    .max(250)
    .label('Name'),
  startDate: date()
    .required()
    .label('Start Date'),
  endDate: date()
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
