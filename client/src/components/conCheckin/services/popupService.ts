import { useConfirm } from 'primevue/useconfirm'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'

export const confirmationPopups = () => {
  const confirm = useConfirm()
  const store = EventCheckinStore()

  const retireConfirmation = (mouseEvent: MouseEvent, name: string) =>
    confirm.require({
      target: mouseEvent.target as HTMLElement,
      group: 'popup',
      message: `Do you want to Retire ${name}?`,
      icon: 'pi pi-info-circle',
      rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptProps: {
        label: `Retire ${name}?`,
        severity: 'danger',
      },
      accept: () => {
        store.retireCharacter()
      },
    })

  return { retireConfirmation }
}
