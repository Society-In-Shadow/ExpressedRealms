import { useAppDialog } from '@/utilities/dialogUtilities.ts'
import type { AddPowerType, EditPowerPopup } from '@/components/expressions/powers/types.ts'

const AddPower = () => import('@/components/expressions/powers/AddPower.vue')
const EditPower = () => import('@/components/expressions/powers/EditPower.vue')

export const powerDialogs = () => {
  const dialog = useAppDialog()

  return {
    showAddPower: (data: AddPowerType) => dialog.open(AddPower, { header: 'Add Power', data }),
    showEditPower: (data: EditPowerPopup) => dialog.open(EditPower, { header: 'Edit Power', data }),
  }
}
