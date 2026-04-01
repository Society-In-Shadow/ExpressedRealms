import { useConfirm } from 'primevue/useconfirm'
import { useDeleteArchetype } from '@/components/admin/archetypes/stores/archetypeStore.ts'

export const ConfirmationPopup = (id: number, name: string) => {
  const confirm = useConfirm()
  const { mutate: deleteArchetype } = useDeleteArchetype()
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
        label: 'Delete Archetype',
        severity: 'danger',
      },
      accept: () => {
        deleteArchetype(id)
      },
    })

  return { deleteConfirmation }
}
