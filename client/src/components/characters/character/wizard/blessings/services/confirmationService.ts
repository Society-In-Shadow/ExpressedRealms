import {useConfirm} from 'primevue/useconfirm'
import {
    characterBlessingsStore,
} from '@/components/characters/character/wizard/blessings/stores/characterBlessingStore.ts'

export const confirmationPopup = (characterId: number) => {
  const confirm = useConfirm()
  const store = characterBlessingsStore()

  const deleteConfirmation = (event: MouseEvent, mappingId: number) =>
    confirm.require({
      target: event.target as HTMLElement,
      group: 'popup',
      message: `Do you want to delete this Advantage / Disadvantage?`,
      icon: 'pi pi-info-circle',
      rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptProps: {
        label: 'Delete Advantage / Disadvantage',
        severity: 'danger',
      },
      accept: () => {
        store.deleteBlessing(characterId, mappingId)
      },
    })

  const deleteSpecializationConfirmation = (event: MouseEvent, mappingId: number, specializationId: number) =>
    confirm.require({
      target: event.target as HTMLElement,
      group: 'popup',
      message: `Do you want to delete this specialization?`,
      icon: 'pi pi-info-circle',
      rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptProps: {
        label: 'Delete Specialization',
        severity: 'danger',
      },
      accept: () => {
        store.deleteSpecialization(characterId, mappingId, specializationId)
      },
    })

  return { deleteConfirmation, deleteSpecializationConfirmation }
}
