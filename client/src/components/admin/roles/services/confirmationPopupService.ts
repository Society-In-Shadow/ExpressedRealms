import { useConfirm } from 'primevue/useconfirm'
import { RoleStore } from '../stores/roleStore'
import { useRouter } from 'vue-router'

export const ConfirmationPopup = () => {
  const confirm = useConfirm()
  const store = RoleStore()
  const router = useRouter()
  const deleteConfirmation = (event: MouseEvent, id: number, name: string) =>
    confirm.require({
      target: event.target as HTMLElement,
      group: 'popup',
      message: `Do you want to delete ${name}?`,
      icon: 'pi pi-info-circle',
      rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptProps: {
        label: 'Delete Role',
        severity: 'danger',
      },
      accept: () => {
        store.deleteEvent(id)
        router.push({ name: 'adminRoleList' })
      },
    })

  return { deleteConfirmation }
}
