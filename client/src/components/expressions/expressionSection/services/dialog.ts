import { useAppDialog } from '@/utilities/dialogUtilities.ts'
import type { EditSectionType } from '@/components/expressions/expressionSection/types.ts'

const EditExpression = () => import('@/components/expressions/expressionSection/EditExpression.vue')

export const expressionSectionDialogs = () => {
  const dialog = useAppDialog()

  return {
    editExpressionSection: (data: EditSectionType) => dialog.open(EditExpression, { header: 'Edit Section', data }),
  }
}
