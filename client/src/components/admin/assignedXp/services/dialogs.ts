import { useDialog } from 'primevue/usedialog'
import AssignedXpList from '@/components/admin/assignedXp/AssignedXpList.vue'

export const adminXpScheduleDialogs = () => {
  const dialog = useDialog()

  const showAssignedXp = (characterId: number, isReadOnly: boolean) => {
    dialog.open(AssignedXpList, {
      props: {
        header: 'Assigned XP',
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
        isReadOnly: isReadOnly,
      },
    })
  }
  return {
    showAssignedXp,
  }
}
