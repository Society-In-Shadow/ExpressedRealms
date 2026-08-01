import { useAppDialog } from '@/utilities/dialogUtilities.ts'
import type { ApprovePromotionInfo } from '@/components/characters/character/factions/types.ts'

const approvePromotionDialogLoader = () => import('@/components/characters/character/factions/ApprovePromotionDialog.vue')

export const factionDialogs = () => {
  const dialog = useAppDialog()

  return {
    approvePromotion: (data: ApprovePromotionInfo) => dialog.open(approvePromotionDialogLoader, { header: 'Approve Promotion', data: data }),
  }
}
