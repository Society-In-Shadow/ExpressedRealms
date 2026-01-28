import { useConfirm } from 'primevue/useconfirm'
import { contactStore } from '@/components/characters/character/wizard/contacts/stores/contactStore.ts'

export const confirmationPopup = (characterId: number, contactId: number) => {
  const confirm = useConfirm()
  const store = contactStore()

  const deleteConfirmation = (event: MouseEvent) =>
    confirm.require({
      target: event.target as HTMLElement,
      group: 'popup',
      message: `Do you want to delete this?`,
      icon: 'pi pi-info-circle',
      rejectProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptProps: {
        label: 'Delete Contact',
        severity: 'danger',
      },
      accept: () => {
        store.deleteContact(characterId, contactId)
      },
    })

  return { deleteConfirmation }
}
