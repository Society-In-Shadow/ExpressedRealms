import { useConfirm } from 'primevue/useconfirm'
import { assignedUsersStore } from '@/components/admin/roles/assignedUsers/stores/assignedUsersStore.ts'

export const ConfirmationPopup = () => {
  const confirm = useConfirm()
  const store = assignedUsersStore()

  const removeUserConfirmation = (event: MouseEvent, id: number, userId: string, name: string) =>
    confirm.require({
      target: event.target as HTMLElement,
      group: 'popup',
      message: `Do you want to remove ${name}?`,
      icon: 'pi pi-info-circle',
      rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptProps: {
        label: 'Remove User',
        severity: 'danger',
      },
      accept: () => {
        store.removeUserFromRole(id, userId)
      },
    })

  return { removeUserConfirmation }
}
