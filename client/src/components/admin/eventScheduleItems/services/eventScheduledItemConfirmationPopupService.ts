import { useConfirm } from 'primevue/useconfirm'
import { EventScheduleItemStore } from '@/components/admin/eventScheduleItems/stores/eventScheduleItemStore'

export const EventScheduleItemConfirmationPopup = (id: number, name: string) => {
  const confirm = useConfirm()
  const store = EventScheduleItemStore()

  const deleteConfirmation = (eventId: number, EventScheduleItem: MouseEvent) =>
    confirm.require({
      target: EventScheduleItem.target as HTMLElement,
      group: 'popup',
      message: `Do you want to delete ${name}?`,
      icon: 'pi pi-info-circle',
      rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptProps: {
        label: 'Delete This Item?',
        severity: 'danger',
      },
      accept: () => {
        store.deleteEventScheduleItem(eventId, id)
      },
    })

  return { deleteConfirmation }
}
