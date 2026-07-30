import { object, string } from 'yup'
import { type GenericForm, useGenericForm } from '@/utilities/formUtilities'
import type { CreateSingleFactionInfo } from '@/components/characters/wizard/factions/types.ts'

const validationSchema = object({
  requestReason: string()
    .max(20_000)
    .label('Request Reason'),
})

export function getValidationInstance(): GenericForm<CreateSingleFactionInfo> {
  const form = useGenericForm(validationSchema)

  return {
    ...form,
  }
}
