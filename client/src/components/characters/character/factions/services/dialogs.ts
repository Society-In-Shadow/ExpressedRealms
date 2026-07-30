import { useAppDialog } from '@/utilities/dialogUtilities.ts'
import type { RequestPromotionInfo } from '@/components/characters/wizard/factions/types.ts'

const requestPromotionDialogLoader = () => import('@/components/characters/wizard/factions/RequestPromotionDialog.vue')

export const factionDialogs = () => {
  const dialog = useAppDialog()

  return {
    requestPromotion: (data: RequestPromotionInfo) => dialog.open(requestPromotionDialogLoader, { header: 'Request Promotion', data: data }),
  }
}
