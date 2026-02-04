import { useConfirm } from 'primevue/useconfirm'
import { EventQuestionStore } from '@/components/admin/eventQuestions/stores/eventQuestionStore.ts'

export const confirmationPopups = (id: number, name: string) => {
  const confirm = useConfirm()
  const store = EventQuestionStore()

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
        store.deleteItem(eventId, id)
      },
    })

  return { deleteConfirmation }
}
