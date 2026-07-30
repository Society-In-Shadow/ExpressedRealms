import { useConfirm } from 'primevue/useconfirm'
import { leaveFaction } from '@/components/characters/wizard/factions/stores/factionStore.ts'

export const ConfirmationPopup = (id: number) => {
  const confirm = useConfirm()
  const deleteConfirmation = async (event: MouseEvent) =>
    confirm.require({
      target: event.target as HTMLElement,
      group: 'popup',
      message: `Do you want to leave this faction?  You WILL lose all existing ranks.`,
      icon: 'pi pi-info-circle',
      rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptProps: {
        label: 'Leave Faction',
        severity: 'danger',
      },
      accept: async () => {
        const action = leaveFaction()
        await action.mutateAsync({ data: { characterId: id } })
      },
    })

  return { deleteConfirmation }
}
