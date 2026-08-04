import { useAppDialog } from '@/utilities/dialogUtilities.ts'

const AddExpression = () => import ('@/components/expressions/expression/AddExpression.vue')
const EditExpression = () => import('@/components/expressions/expression/EditExpression.vue')

const CopyExpression = () => import('@/components/expressions/expression/CopyExpression.vue')

export const expressionDialogService = () => {
  const dialog = useAppDialog()

  function getDialogTitle(expressionTypeId: number) {
    switch (expressionTypeId) {
      case 1: return 'Expression'
      case 13: return 'Rule Book Section'
      case 14: return 'World Background Section'
    }
  }
  const showAddExpression = (expressionTypeId: number) => {
    dialog.open(AddExpression, { header: `Add ${getDialogTitle(expressionTypeId)}`,
      data: {
        expressionTypeId: expressionTypeId,
      },
    })
  }

  const showEditExpression = (expressionId: number, expressionTypeId: number) => {
    dialog.open(EditExpression, { header: `Edit ${getDialogTitle(expressionTypeId)}`,
      data: {
        expressionId: expressionId,
      },
    })
  }

  const showCopyExpression = (expressionId: number, expressionTypeId: number) => {
    dialog.open(CopyExpression, { header: `Copy ${getDialogTitle(expressionTypeId)}`,
      data: {
        expressionId: expressionId,
      },
    })
  }

  return {
    showAddExpression,
    showEditExpression,
    showCopyExpression,
  }
}
