import { useConfirm } from 'primevue/useconfirm'
import { EventCheckinStore } from '@/components/conCheckin/stores/eventCheckinStore.ts'
import { AgeGroupId } from '@/components/conCheckin/types.ts'

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

  const childConfirmation = (mouseEvent: MouseEvent) =>
    confirm.require({
      target: mouseEvent.target as HTMLElement,
      group: 'popup',
      message: `You sure this user is under 13 years old?  Inform them their account will be deleted once confirmed, they are not allowed to play the game per terms of service.`,
      icon: 'pi pi-info-circle',
      rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptProps: {
        label: `Confirmed Under 13`,
        severity: 'warning',
      },
      accept: () => {
        store.verifiedAge(AgeGroupId.Child, false)
      },
    })

  return { retireConfirmation, childConfirmation }
}
