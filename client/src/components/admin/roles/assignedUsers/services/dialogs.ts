import { useDialog } from 'primevue/usedialog'
import AssignUserPopup from '@/components/admin/roles/assignedUsers/AssignUserPopup.vue'

export const roleUserAssignmentDialogs = () => {
  const dialog = useDialog()

  const showAddUserPopup = () => {
    dialog.open(AssignUserPopup, {
      props: {
        header: 'Assign User',
        style: {
          width: '500px',
        },
        breakpoints: {
          '960px': '75vw',
          '640px': '90vw',
        },
        modal: true,
      },
    })
  }
  return {
    showAddUserPopup,
  }
}
