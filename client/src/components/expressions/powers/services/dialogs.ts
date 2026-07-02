import { useAppDialog } from '@/utilities/dialogUtilities.ts'
import type { AddPowerType } from '@/components/expressions/powers/types.ts'

const AddPower = () => import('@/components/expressions/powers/AddPower.vue')

export const powerDialogs = () => {
  const dialog = useAppDialog()

  return {
    showAddPower: (data: AddPowerType) => dialog.open(AddPower, { header: 'Add Power', data }),
  }
}
