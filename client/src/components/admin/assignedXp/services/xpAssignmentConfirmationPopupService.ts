import { useConfirm } from 'primevue/useconfirm'
import { AssignedXpStore } from '@/components/admin/assignedXp/stores/assignedXpStore.ts'

export const xpAssignmentConfirmationPopup = () => {
  const confirm = useConfirm()
  const store = AssignedXpStore()

  const deleteConfirmation = (event: MouseEvent, id: number, characterId: number) =>
    confirm.require({
      target: event.target as HTMLElement,
      group: 'popup',
      message: `Do you want to delete this?`,
      icon: 'pi pi-info-circle',
      rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptProps: {
        label: 'Delete Event',
        severity: 'danger',
      },
      accept: () => {
        store.delete(characterId, id)
      },
    })

  return { deleteConfirmation }
}
