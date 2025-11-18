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

  const publishConfirmation = (event: MouseEvent) =>
    confirm.require({
      target: event.target as HTMLElement,
      group: 'popup',
      message: `Do you want to publish ${name}?  This cannot be undone and will send automated messages in Discord.`,
      icon: 'pi pi-info-circle',
      rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptProps: {
        label: 'Publish Event',
        severity: 'warning',
      },
      accept: () => {
        store.publishEvent(id)
      },
    })

  return { deleteConfirmation, publishConfirmation }
}
