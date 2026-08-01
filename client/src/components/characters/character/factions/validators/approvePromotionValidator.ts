import { object, string } from 'yup'
import { type GenericForm, useGenericForm } from '@/utilities/formUtilities'
import type { ApprovePromotionForm } from '@/components/characters/character/factions/types.ts'

const validationSchema = object({
  approvalReason: string().required()
    .min(20)
    .max(20_000)
    .label('Approval Reason'),
})

export function getValidationInstance(): GenericForm<ApprovePromotionForm> {
  const form = useGenericForm(validationSchema)

  return {
    ...form,
  }
}
