import {useConfirm} from 'primevue/useconfirm'
import {blessingsStore} from '@/components/blessings/stores/blessingsStore.ts'

export const blessingConfirmationPopup = () => {
  const confirm = useConfirm()
  const store = blessingsStore()

  const deleteConfirmation = (event: MouseEvent, blessingId: number) =>
    confirm.require({
      target: event.target as HTMLElement,
      group: 'popup',
      message: `Do you want to delete this blessing?`,
      icon: 'pi pi-info-circle',
      rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptProps: {
        label: 'Delete Blessing',
        severity: 'danger',
      },
      accept: () => {
        store.deleteBlessing(blessingId)
      },
    })

  const deleteBlessingLevelConfirmation = (event: MouseEvent, blessingId: number, levelId: number) =>
    confirm.require({
      target: event.target as HTMLElement,
      group: 'popup',
      message: `Do you want to delete this blessing level?`,
      icon: 'pi pi-info-circle',
      rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptProps: {
        label: 'Delete Blessing Level',
        severity: 'danger',
      },
      accept: () => {
        store.deleteBlessingLevel(blessingId, levelId)
      },
    })

  return { deleteConfirmation, deleteBlessingLevelConfirmation }
}
