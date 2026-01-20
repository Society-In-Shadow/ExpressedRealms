import { useDialog } from 'primevue/usedialog'
import AssignUserPopup from '@/components/admin/players/assignedRoles/AssignUserPopup.vue'

export const usrRoleAssignmentDialogs = () => {
  const dialog = useDialog()

  const showAddRolePopup = () => {
    dialog.open(AssignUserPopup, {
      props: {
        header: 'Assign Role',
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
    showAddRolePopup: showAddRolePopup,
  }
}
