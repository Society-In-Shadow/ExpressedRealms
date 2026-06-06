import { useDialog } from 'primevue/usedialog'
import UpdateGoFields from '@/components/admin/characterList/UpdateGoFields.vue'

export const characterGoFieldsDialog = () => {
  const dialog = useDialog()

  const showUpdateGoFields = (characterId: number) => {
    dialog.open(UpdateGoFields, {
      props: {
        header: 'Update Character GO Fields',
        style: {
          width: '500px',
        },
        breakpoints: {
          '960px': '75vw',
          '640px': '90vw',
        },
        modal: true,
      },
      data: {
        characterId: characterId,
      },
    })
  }
  return {
    showUpdateGoFields,
  }
}
