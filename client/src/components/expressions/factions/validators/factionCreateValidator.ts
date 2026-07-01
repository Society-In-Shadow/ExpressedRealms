import { object, string } from 'yup'
import { type GenericForm, useGenericForm } from '@/utilities/formUtilities'
import type { CreateSingleFactionInfo } from '@/components/expressions/factions/types.ts'
import type { ListItem } from '@/types/ListItem.ts'

const validationSchema = object({
  name: string()
    .required()
    .max(250)
    .label('Name'),
  background: string()
    .required()
    .max(20_000)
    .label('Background'),
  knowledge: object<ListItem>()
    .required()
    .label('Faction Knowledge'),
  specialization: string()
    .required()
    .label('Faction Specialization')
    .max(250),
})

export function getValidationInstance(): GenericForm<CreateSingleFactionInfo> {
  const form = useGenericForm(validationSchema)

  return {
    ...form,
  }
}
