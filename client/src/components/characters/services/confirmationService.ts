import {useConfirm} from 'primevue/useconfirm'
import {charactersStore} from '@/components/characters/stores/charactersStore.ts'

export const overallCharacterConfirmationService = () => {
  const confirm = useConfirm()
  const store = charactersStore()

  const deleteConfirmation = (event: MouseEvent, characterId: number) =>
    confirm.require({
      target: event.target as HTMLElement,
      group: 'popup',
      message: `Do you want to delete this character?`,
      icon: 'pi pi-info-circle',
      rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptProps: {
        label: 'Delete Character',
        severity: 'danger',
      },
      accept: () => {
        store.deleteCharacter(characterId)
      },
    })

  return { deleteConfirmation }
}
