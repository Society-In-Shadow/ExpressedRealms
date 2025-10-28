import { useConfirm } from 'primevue/useconfirm'
import { EventStore } from '@/components/admin/events/stores/eventStore'

export const EventConfirmationPopup = (id: number, name: string) => {
  const confirm = useConfirm()
  const store = EventStore()

  const deleteConfirmation = (event: MouseEvent) =>
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
        label: 'Delete Event',
        severity: 'danger',
      },
      accept: () => {
        store.deleteEvent(id)
      },
    })

  return { deleteConfirmation }
}
